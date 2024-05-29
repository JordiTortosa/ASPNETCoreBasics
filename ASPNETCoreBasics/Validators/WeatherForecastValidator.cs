namespace ASPNETCoreBasics.Validators
{
    using ASPNETCoreBasics.Models;
    using FluentValidation;

    public class WeatherForecastRequestValidator : AbstractValidator<WeatherForecastModel>
    {
        public WeatherForecastRequestValidator()
        {
            RuleFor(x => x.Date).NotEmpty().WithMessage("Date es obligatorio");
            RuleFor(x => x.TemperatureC).InclusiveBetween(-100, 100).WithMessage("TemperatureC debe estar entre -20 y 55");
            RuleFor(x => x.Summary).NotEmpty().WithMessage("Summary es obligatorio");
        }
    }
}
