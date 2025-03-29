using Microsoft.Extensions.DependencyInjection;

using NetArch.Template.Application.Contracts.Services;
using NetArch.Template.Application.Services;
using NetArch.Template.Domain.Repositories;
using NetArch.Template.Persistence.EntityFrameworkCore.Repositories;

namespace NetArch.Template.HttpApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerService, CustomerService>();

            return services;
        }

        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }
    }
}
