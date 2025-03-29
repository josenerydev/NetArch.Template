using Mapster;

using NetArch.Template.Application.Contracts.DTOs;
using NetArch.Template.Domain.Entities;
using NetArch.Template.Domain.Shared.Enums;

namespace NetArch.Template.Application.MappingProfiles;

public static class MapsterConfig
{
    public static void Configure()
    {
        // Customer -> CustomerDto
        TypeAdapterConfig<Customer, CustomerDto>.NewConfig()
            .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}")
            .Map(dest => dest.Age, src => CalculateAge(src.BirthDate))
            .Map(dest => dest.IsActive, src => src.Status == CustomerStatus.Active);

        // CustomerCreateDto -> Customer
        TypeAdapterConfig<CustomerCreateDto, Customer>.NewConfig()
            .Ignore(dest => dest.Id)
            .Map(dest => dest.Status, _ => CustomerStatus.Active)
            .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
            .Ignore(dest => dest.UpdatedAt);

        // CustomerUpdateDto -> Customer
        TypeAdapterConfig<CustomerUpdateDto, Customer>.NewConfig()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Email)
            .Ignore(dest => dest.BirthDate)
            .Ignore(dest => dest.Status)
            .Ignore(dest => dest.CreatedAt)
            .Map(dest => dest.UpdatedAt, _ => DateTime.UtcNow);
    }

    private static int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;
        return age;
    }
}
