using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;

using System.Globalization;

namespace NetArch.Template.Infrastructure;

public static class SerilogExtensions
{
    public static ILogger CreateBootstrapLogger()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
            .CreateBootstrapLogger();
    }

    public static void AddSerilogConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var useApplicationInsights = configuration.GetValue<bool>("Logging:Providers:UseApplicationInsights", true);
        var useSeq = configuration.GetValue<bool>("Logging:Providers:UseSeq", true);
        var seqUrl = configuration.GetValue<string>("Logging:Providers:SeqUrl");

        if (useSeq && string.IsNullOrWhiteSpace(seqUrl))
        {
            throw new ArgumentException("SeqUrl cannot be null or empty when UseSeq is enabled.");
        }

        if (useApplicationInsights)
        {
            services.AddApplicationInsightsTelemetry();
        }

        services.AddSerilog((_, loggerConfiguration) =>
        {
            loggerConfiguration
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Destructure.ToMaximumDepth(4)
                .Destructure.ToMaximumStringLength(100)
                .Destructure.ToMaximumCollectionCount(10)
                .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture);

            if (useSeq)
            {
                string nonNullSeqUrl = seqUrl ?? throw new InvalidOperationException("SeqUrl should not be null at this point");
                loggerConfiguration.WriteTo.Seq(
                    serverUrl: nonNullSeqUrl,
                    batchPostingLimit: 50,
                    period: TimeSpan.FromSeconds(2),
                    formatProvider: CultureInfo.InvariantCulture
                );
            }

            if (useApplicationInsights)
            {
                var connectionString = configuration.GetSection("Serilog:WriteTo")
                    .GetChildren()
                    .FirstOrDefault(x => x.GetValue<string>("Name") == "ApplicationInsights")?
                    .GetSection("Args")?
                    .GetValue<string>("connectionString");

                if (!string.IsNullOrWhiteSpace(connectionString) && !connectionString.Contains("{INSTRUMENTATION_KEY}", StringComparison.Ordinal))
                {
                    loggerConfiguration.WriteTo.ApplicationInsights(
                        connectionString: connectionString,
                        telemetryConverter: new TraceTelemetryConverter()
                    );
                }
            }
        });

        Serilog.Debugging.SelfLog.Enable(Console.Error);
    }

    public static void UseSerilogRequestLoggingConfiguration(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
    }
}
