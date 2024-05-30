namespace ASPNETCoreBasics.Validators
{
    using ASPNETCoreBasics.Models;
    using FluentValidation;

    public class TestValidator : AbstractValidator<TestModel>
    {
        public TestValidator()
        {
            RuleFor(x => x.Password).Equal("Test").WithMessage("Password debe ser 'Test'");
        }
    }
}