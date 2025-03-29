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
        try
        {
            var (useApplicationInsights, useSeq, seqUrl) = GetProviderSettings(configuration);

            if (useApplicationInsights)
            {
                services.AddApplicationInsightsTelemetry();
            }

            services.AddSerilog((provider, loggerConfiguration) =>
            {
                // Setup base configuration
                ConfigureBaseSettings(loggerConfiguration);

                // Configure minimum level overrides
                ConfigureLevelOverrides(loggerConfiguration, configuration);

                // Configure destructurers
                ConfigureDestructurers(loggerConfiguration);

                // Add enrichers
                ConfigureEnrichers(loggerConfiguration, configuration);

                // Add Seq if enabled
                if (useSeq && !string.IsNullOrWhiteSpace(seqUrl))
                {
                    ConfigureSeq(loggerConfiguration, seqUrl);
                }

                // Add Application Insights if enabled
                if (useApplicationInsights)
                {
                    ConfigureApplicationInsights(loggerConfiguration, configuration);
                }
            });

            Serilog.Debugging.SelfLog.Enable(Console.Error);
        }
        catch (ArgumentException ex)
        {
            Log.Error(ex, "Configuration error in Serilog setup");
            throw new ArgumentException($"Invalid Serilog configuration: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unexpected error in Serilog setup");
            throw new InvalidOperationException($"Failed to configure Serilog: {ex.Message}", ex);
        }
    }

    private static (bool useApplicationInsights, bool useSeq, string? seqUrl) GetProviderSettings(IConfiguration configuration)
    {
        var useApplicationInsights = configuration.GetValue<bool>("Logging:Providers:UseApplicationInsights", false);
        var useSeq = configuration.GetValue<bool>("Logging:Providers:UseSeq", false);
        var seqUrl = configuration.GetValue<string>("Logging:Providers:SeqUrl");

        if (useSeq && string.IsNullOrWhiteSpace(seqUrl))
        {
            throw new ArgumentException("SeqUrl cannot be null or empty when UseSeq is enabled.");
        }

        return (useApplicationInsights, useSeq, seqUrl);
    }

    private static void ConfigureBaseSettings(LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture);
    }

    private static void ConfigureLevelOverrides(LoggerConfiguration loggerConfiguration, IConfiguration configuration)
    {
        var minimumLevelOverrides = configuration.GetSection("Serilog:MinimumLevel:Override");
        foreach (var overrideConfig in minimumLevelOverrides.GetChildren())
        {
            var levelName = overrideConfig.Value;
            if (Enum.TryParse<LogEventLevel>(levelName, out var level))
            {
                loggerConfiguration.MinimumLevel.Override(overrideConfig.Key, level);
            }
        }
    }

    private static void ConfigureDestructurers(LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration
            .Destructure.ToMaximumDepth(4)
            .Destructure.ToMaximumStringLength(100)
            .Destructure.ToMaximumCollectionCount(10);
    }

    private static void ConfigureEnrichers(LoggerConfiguration loggerConfiguration, IConfiguration configuration)
    {
        var enrichers = configuration.GetSection("Serilog:Enrich").GetChildren().Select(x => x.Value).ToArray();

        if (enrichers.Contains("WithMachineName"))
        {
            EnvironmentLoggerConfigurationExtensions.WithMachineName(loggerConfiguration.Enrich);
        }

        if (enrichers.Contains("WithThreadId"))
        {
            loggerConfiguration.Enrich.WithThreadId();
        }
    }

    private static void ConfigureSeq(LoggerConfiguration loggerConfiguration, string? seqUrl)
    {
        if (string.IsNullOrWhiteSpace(seqUrl))
        {
            return;
        }

        loggerConfiguration.WriteTo.Seq(
            serverUrl: seqUrl,
            batchPostingLimit: 50,
            period: TimeSpan.FromSeconds(2),
            formatProvider: CultureInfo.InvariantCulture
        );
    }

    private static void ConfigureApplicationInsights(LoggerConfiguration loggerConfiguration, IConfiguration configuration)
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
            Log.Warning("Application Insights is enabled but no valid connection string was found");
        }
    }

    private static string? GetAppInsightsConnectionString(IConfiguration configuration)
    {
        // Try from environment variable
        var connectionString = TryGetConnectionStringFromEnvironment();
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            return connectionString;
        }

        // Try from dedicated ApplicationInsights section
        connectionString = TryBuildConnectionStringFromConfig(configuration);
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            return connectionString;
        }

        // As a last resort, check for direct connection string in Serilog config
        return TryGetConnectionStringFromSerilogConfig(configuration);
    }

    private static string? TryGetConnectionStringFromEnvironment()
    {
        return Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING");
    }

    private static string? TryBuildConnectionStringFromConfig(IConfiguration configuration)
    {
        var instrumentationKey = configuration.GetValue<string>("ApplicationInsights:InstrumentationKey");
        var ingestionEndpoint = configuration.GetValue<string>("ApplicationInsights:IngestionEndpoint");

        if (string.IsNullOrWhiteSpace(instrumentationKey) || string.IsNullOrWhiteSpace(ingestionEndpoint))
        {
            return null;
        }

        var liveEndpoint = configuration.GetValue<string>("ApplicationInsights:LiveEndpoint");
        var applicationId = configuration.GetValue<string>("ApplicationInsights:ApplicationId");

        var connectionString = $"InstrumentationKey={instrumentationKey};IngestionEndpoint={ingestionEndpoint}";

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

    private static string? TryGetConnectionStringFromSerilogConfig(IConfiguration configuration)
    {
        var aiSink = configuration.GetSection("Serilog:WriteTo")
            .GetChildren()
            .FirstOrDefault(x => x.GetValue<string>("Name") == "ApplicationInsights");

        if (aiSink == null)
        {
            return null;
        }

        var rawConnString = aiSink.GetSection("Args").GetValue<string>("connectionString");
        if (!string.IsNullOrWhiteSpace(rawConnString) &&
            !rawConnString.Contains('{', StringComparison.Ordinal) &&
            !rawConnString.Contains('}', StringComparison.Ordinal))
        {
            return rawConnString;
        }

        return null;
    }

    public static void UseSerilogRequestLoggingConfiguration(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
    }
}
