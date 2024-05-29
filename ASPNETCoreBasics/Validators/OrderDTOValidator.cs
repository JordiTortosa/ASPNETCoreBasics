using ASPNETCoreBasics.Models;
using FluentValidation;

namespace ASPNETCoreBasics.Validators
{
    public class OrderDTOValidator : AbstractValidator<OrderDto>
    {
        public OrderDTOValidator()
        {
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description es obligatorio");
        }
    }
}
