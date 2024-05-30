using ASPNETCoreBasics.Models;
using FluentValidation;

namespace ASPNETCoreBasics.Validators
{
    public class UserDTOValidator : AbstractValidator<UserDto>
    {
        public UserDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name es obligatorio");
            RuleFor(x => x.Orders).NotNull().WithMessage("Orders No Válido");
        }
    }
}