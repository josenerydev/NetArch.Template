using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using NetArch.Template.Application.Validators;

namespace NetArch.Template.HttpApi.Extensions;

public static class ValidationServiceCollectionExtensions
{
    public static IServiceCollection AddValidationServices(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CustomerCreateDtoValidator>();
        return services;
    }
}
