using Microsoft.Extensions.Configuration;
using NetArch.Template.Infrastructure.Abstractions.Utils;

namespace NetArch.Template.Infrastructure.Abstractions.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetDatabaseConnectionString(this IConfiguration configuration)
        {
            return configuration.GetRequiredConfiguration(
                ConfigurationKeys.DefaultConnectionEnv,
                ConfigurationKeys.DefaultConnectionConfig
            );
        }

        public static string GetRedisConnectionString(this IConfiguration configuration)
        {
            return configuration.GetRequiredConfiguration(
                ConfigurationKeys.RedisConnectionEnv,
                ConfigurationKeys.RedisConnectionConfig
            );
        }

        public static int GetMaxConcurrentWorkflows(this IConfiguration configuration)
        {
            return configuration.GetValue<int>("MaxConcurrentWorkflows");
        }

        private static string GetRequiredConfiguration(
            this IConfiguration configuration,
            string envVariable,
            string configKey
        )
        {
            var value =
                Environment.GetEnvironmentVariable(envVariable)
                ?? configuration.GetValue<string>(configKey);

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException(
                    $"Configuração obrigatória ausente: '{configKey}' ou variável de ambiente '{envVariable}'"
                );
            }

            return value;
        }
    }
}
