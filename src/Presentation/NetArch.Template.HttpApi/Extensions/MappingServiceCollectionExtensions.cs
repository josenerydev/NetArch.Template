using Mapster;

using MapsterMapper;

using Microsoft.Extensions.DependencyInjection;

using NetArch.Template.Application.MappingProfiles;
using NetArch.Template.Infrastructure.Abstractions.Mapping;
using NetArch.Template.Infrastructure.Mapping;

namespace NetArch.Template.HttpApi.Extensions
{
    public static class MappingServiceCollectionExtensions
    {
        public static IServiceCollection AddMapster(this IServiceCollection services)
        {
            MapsterConfig.Configure();

            services.AddSingleton(TypeAdapterConfig.GlobalSettings);
            services.AddSingleton<IMapper, ServiceMapper>();
            services.AddSingleton<IObjectMapper, MapsterObjectMapper>();

            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(AutoMapperConfig).Assembly);
            });

            services.AddSingleton<IObjectMapper, AutoMapperObjectMapper>();

            return services;
        }
    }
}
