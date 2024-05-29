using ASPNETCoreBasics.Validators;
using ASPNETCoreBasics.Validators;
using ASPNETCoreBasicsTests.Mocks;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ASPNETCoreBasicsTests.Validators
{
    public class WeatherForecastValidatorTest
    {
        [Theory]
        [InlineData("2024-05-28", 15, "SuperCold", true)] // Caso válido
        [InlineData("", 15, "SuperCold", false)] // Caso Date vacío
        [InlineData("2024-05-28", null, "SuperCold", false)] // Caso TemperatureC vacío
        [InlineData("2024-05-28", 15, "", false)] // Caso Summary vacío

        public void ValidateMessageTest(string dateString, int? temperatureC, string summary, bool isValid)
        {
            var date = DateOnly.Parse("1000-10-10");
            if (dateString != "")
            {
                date = DateOnly.Parse(dateString);
            }
            var temperatureCTrue = 1000;
            if (temperatureC != null)
            {
                temperatureCTrue = temperatureC.Value;
            }
            var dto = DtoMockCreator.CreateWeatherForecastDTO(date, temperatureCTrue, summary);

            var validator = new WeatherForecastDTORequestValidator();

            var validationResult = validator.Validate(dto);
            validationResult.IsValid.Should().Be(isValid);
        }
    }
}
