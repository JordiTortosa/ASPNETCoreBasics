using ASPNETCoreBasics.Validators;
using ASPNETCoreBasicsTests.Mocks;
using FluentAssertions;
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
            var date = !string.IsNullOrEmpty(dateString) && DateOnly.TryParse(dateString, out var parsedDate) ? parsedDate : DateOnly.Parse("1000-10-10");
            var temperatureCTrue = temperatureC ?? 1000;
            var dto = DtoMockCreator.CreateWeatherForecastDTO(date, temperatureCTrue, summary);
            var validator = new WeatherForecastDTORequestValidator();
            validator.Validate(dto).IsValid.Should().Be(isValid);
        }
    }
}