using ASPNETCoreBasics.Validators;
using ASPNETCoreBasicsTests.Mocks;
using FluentAssertions;
using Xunit;

namespace ASPNETCoreBasicsTests.Validators
{
    public class OrderValidatorTest
    {
        [Theory]
        [InlineData("Test Order", true)] // Caso válido
        [InlineData("", false)] // Caso Description vacío
        public void ValidateMessageTest(string description, bool isValid)
        {
            var dto = DtoMockCreator.CreateOrderDTO(description);

            var validator = new OrderDTOValidator();

            var validationResult = validator.Validate(dto);
            validationResult.IsValid.Should().Be(isValid);
        }
    }
}