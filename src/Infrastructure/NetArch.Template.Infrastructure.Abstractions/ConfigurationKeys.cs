namespace NetArch.Template.Infrastructure.Abstractions
{
    public static class ConfigurationKeys
    {
        public const string DefaultConnectionEnv = "ConnectionStrings__DefaultConnection";
        public const string DefaultConnectionConfig = "ConnectionStrings:DefaultConnection";

        public const string RedisConnectionEnv = "ConnectionStrings__RedisConnection";
        public const string RedisConnectionConfig = "ConnectionStrings:RedisConnection";
    }
}