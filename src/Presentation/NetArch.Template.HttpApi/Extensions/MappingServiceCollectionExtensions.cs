using Microsoft.Extensions.DependencyInjection;

using NetArch.Template.Infrastructure.Abstractions.Mapping;
using NetArch.Template.Infrastructure.Mapping;

namespace NetArch.Template.HttpApi.Extensions
{
    public static class MappingServiceCollectionExtensions
    {
        public static IServiceCollection AddMappingServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => {
                cfg.AddMaps(typeof(Application.MappingProfiles.ApplicationMappingProfile).Assembly);
            });

            services.AddSingleton<IObjectMapper, AutoMapperObjectMapper>();

            return services;
        }
    }
}
