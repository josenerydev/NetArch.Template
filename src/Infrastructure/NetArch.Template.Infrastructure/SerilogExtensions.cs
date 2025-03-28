using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;
using Serilog.Events;

namespace NetArch.Template.Infrastructure;

public static class SerilogExtensions
{
    /// <summary>
    /// Configura o bootstrap logger do Serilog
    /// </summary>
    /// <returns>ILogger configurado para bootstrap</returns>
    public static ILogger CreateBootstrapLogger()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();
    }

    /// <summary>
    /// Adiciona e configura o Serilog no pipeline do ASP.NET Core
    /// </summary>
    /// <param name="services">IServiceCollection para adicionar o Serilog</param>
    /// <param name="configuration">Configuração da aplicação</param>
    /// <remarks>
    /// Opções de configuração disponíveis em appsettings.json:
    /// - Logging:Providers:UseSeq (bool): Habilita ou desabilita o Seq (sempre desabilitado em produção)
    /// - Logging:Providers:SeqUrl (string): URL do servidor Seq (padrão: http://seq:5341)
    /// - Serilog:WriteTo:ApplicationInsights:Args:connectionString: String de conexão do Application Insights
    /// - Serilog: Configurações padrão do Serilog
    /// 
    /// O sink do Seq é sempre desabilitado em ambiente de produção, independentemente 
    /// da configuração em appsettings.json.
    /// </remarks>
    public static void AddSerilogConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Configura Application Insights (padrão via configuração)
        services.AddApplicationInsightsTelemetry();

        services.AddSerilog((serviceProvider, loggerConfiguration) =>
        {
            // Configuração básica e leitura da configuração existente
            var logConfig = loggerConfiguration
                .ReadFrom.Configuration(configuration)
                .ReadFrom.Services(serviceProvider);

            // Verifica o ambiente atual
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var isProdEnvironment = environment.Equals("Production", StringComparison.OrdinalIgnoreCase);

            // Determina se o Seq deve ser habilitado
            bool useSeq;
            if (isProdEnvironment)
            {
                // Em produção, o Seq está sempre desabilitado
                useSeq = false;
            }
            else
            {
                // Em ambientes não-produção, usa a configuração ou habilita por padrão
                useSeq = configuration.GetSection("Logging:Providers")
                    .GetValue<bool>("UseSeq", true);
            }

            if (useSeq)
            {
                // Obter a URL do servidor Seq da configuração
                var seqUrl = configuration.GetSection("Logging:Providers")
                    .GetValue<string>("SeqUrl") ?? "http://seq:5341";

                // Adiciona o Seq com configurações robustas para evitar bloqueios se estiver indisponível
                logConfig.WriteTo.Seq(
                    serverUrl: seqUrl,
                    batchPostingLimit: 50,
                    period: TimeSpan.FromSeconds(2)
                );
            }
        });

        Serilog.Debugging.SelfLog.Enable(Console.Error);
    }

    /// <summary>
    /// Configura o middleware de registro de requisições do Serilog
    /// </summary>
    /// <param name="app">WebApplication para configurar o middleware</param>
    public static void UseSerilogRequestLoggingConfiguration(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
    }
}
