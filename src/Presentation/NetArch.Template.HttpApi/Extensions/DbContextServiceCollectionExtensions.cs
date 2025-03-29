using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetArch.Template.Domain.Repositories;
using NetArch.Template.Persistence.EntityFrameworkCore;
using NetArch.Template.Persistence.EntityFrameworkCore.Repositories;

namespace NetArch.Template.HttpApi.Extensions;

public static class DbContextServiceCollectionExtensions
{
    public static IServiceCollection AddInMemoryPersistence(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("NetArchTemplateDb")
        );

        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}
