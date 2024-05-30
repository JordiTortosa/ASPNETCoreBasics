﻿namespace ASPNETCoreBasics.Validators
{
    using ASPNETCoreBasics.Models;
    using FluentValidation;

    public class WeatherForecastDTORequestValidator : AbstractValidator<WeatherForecastDto>
    {
        public WeatherForecastDTORequestValidator()
        {
            RuleFor(x => x.Date).NotEmpty().WithMessage("Date es obligatorio");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Date No Válido").Must(date => date >= new DateOnly(1970, 01, 01));
            RuleFor(x => x.TemperatureC).NotEmpty().WithMessage("TemperatureC es obligatorio");
            RuleFor(x => x.TemperatureC).NotEmpty().WithMessage("TemperatureC No Válido").Must(temperature => temperature != 1000);
            RuleFor(x => x.Summary).NotEmpty().WithMessage("Summary es obligatorio");
        }
    }
}