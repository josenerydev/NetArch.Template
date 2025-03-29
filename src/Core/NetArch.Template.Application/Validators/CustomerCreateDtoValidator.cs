using FluentValidation;
using NetArch.Template.Application.Contracts.DTOs;

namespace NetArch.Template.Application.Validators
{
    public class CustomerCreateDtoValidator : AbstractValidator<CustomerCreateDto>
    {
        public CustomerCreateDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255);
            RuleFor(x => x.PhoneNumber).MaximumLength(20);
            RuleFor(x => x.BirthDate).NotEmpty().LessThan(DateTime.Today);
        }
    }
}
