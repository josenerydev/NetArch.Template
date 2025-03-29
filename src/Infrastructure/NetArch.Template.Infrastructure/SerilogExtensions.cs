using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;
using Serilog.Enrichers;
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
        var useApplicationInsights = configuration.GetValue<bool>("Logging:Providers:UseApplicationInsights", false);
        var useSeq = configuration.GetValue<bool>("Logging:Providers:UseSeq", false);
        var seqUrl = configuration.GetValue<string>("Logging:Providers:SeqUrl");

        if (useSeq && string.IsNullOrWhiteSpace(seqUrl))
        {
            throw new ArgumentException("SeqUrl cannot be null or empty when UseSeq is enabled.");
        }

        if (useApplicationInsights)
        {
            services.AddApplicationInsightsTelemetry();
        }

        services.AddSerilog((provider, loggerConfiguration) =>
        {
            loggerConfiguration
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture);

            // Configure minimum level overrides from config
            var minimumLevelOverrides = configuration.GetSection("Serilog:MinimumLevel:Override");
            foreach (var overrideConfig in minimumLevelOverrides.GetChildren())
            {
                var levelName = overrideConfig.Value;
                if (Enum.TryParse<LogEventLevel>(levelName, out var level))
                {
                    loggerConfiguration.MinimumLevel.Override(overrideConfig.Key, level);
                }
            }

            // Configure destructurers
            loggerConfiguration
                .Destructure.ToMaximumDepth(4)
                .Destructure.ToMaximumStringLength(100)
                .Destructure.ToMaximumCollectionCount(10);

            // Add enrichers
            var enrichers = configuration.GetSection("Serilog:Enrich").GetChildren().Select(x => x.Value).ToArray();

            if (enrichers.Contains("WithMachineName"))
            {
                EnvironmentLoggerConfigurationExtensions.WithMachineName(loggerConfiguration.Enrich);
            }

            if (enrichers.Contains("WithThreadId"))
            {
                loggerConfiguration.Enrich.WithThreadId();
            }

            // Add seq if enabled
            if (useSeq && !string.IsNullOrWhiteSpace(seqUrl))
            {
                loggerConfiguration.WriteTo.Seq(
                    serverUrl: seqUrl,
                    batchPostingLimit: 50,
                    period: TimeSpan.FromSeconds(2),
                    formatProvider: CultureInfo.InvariantCulture
                );
            }

            // Add App Insights only if explicitly enabled
            if (useApplicationInsights)
            {
                var connectionString = GetAppInsightsConnectionString(configuration);

                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    loggerConfiguration.WriteTo.ApplicationInsights(
                        connectionString: connectionString,
                        telemetryConverter: new TraceTelemetryConverter()
                    );
                }
                else
                {
                    Log.Warning("Application Insights is enabled but no valid connection string was found.");
                }
            }
        });

        Serilog.Debugging.SelfLog.Enable(Console.Error);
    }

    private static string GetAppInsightsConnectionString(IConfiguration configuration)
    {
        // Try from environment variable
        var connectionString = Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING");
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            return connectionString;
        }

        // Try from dedicated ApplicationInsights section
        var instrumentationKey = configuration.GetValue<string>("ApplicationInsights:InstrumentationKey");
        var ingestionEndpoint = configuration.GetValue<string>("ApplicationInsights:IngestionEndpoint");

        if (!string.IsNullOrWhiteSpace(instrumentationKey) &&
            !string.IsNullOrWhiteSpace(ingestionEndpoint))
        {
            var liveEndpoint = configuration.GetValue<string>("ApplicationInsights:LiveEndpoint");
            var applicationId = configuration.GetValue<string>("ApplicationInsights:ApplicationId");

            connectionString = $"InstrumentationKey={instrumentationKey};IngestionEndpoint={ingestionEndpoint}";

            if (!string.IsNullOrWhiteSpace(liveEndpoint))
            {
                connectionString += $";LiveEndpoint={liveEndpoint}";
            }

            if (!string.IsNullOrWhiteSpace(applicationId))
            {
                connectionString += $";ApplicationId={applicationId}";
            }

            return connectionString;
        }

        // As a last resort, check for direct connection string in Serilog config
        var aiSink = configuration.GetSection("Serilog:WriteTo")
            .GetChildren()
            .FirstOrDefault(x => x.GetValue<string>("Name") == "ApplicationInsights");

        if (aiSink != null)
        {
            var rawConnString = aiSink.GetSection("Args").GetValue<string>("connectionString");
            if (!string.IsNullOrWhiteSpace(rawConnString) &&
                !rawConnString.Contains('{', StringComparison.Ordinal) &&
                !rawConnString.Contains('}', StringComparison.Ordinal))
            {
                return rawConnString;
            }
        }

        return string.Empty;
    }

    public static void UseSerilogRequestLoggingConfiguration(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
    }
}
