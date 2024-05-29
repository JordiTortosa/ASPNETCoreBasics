using ASPNETCoreBasics.Models;
using ASPNETCoreBasics.Validators;
using ASPNETCoreBasicsTests.Mocks;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ASPNETCoreBasicsTests.Validators
{
    public class UserValidatorTest
    {
        public static IEnumerable<object[]> GetUserData()
        {
            yield return new object[] { "Jordi", new List<OrderDto>(), true }; // Caso válido
            yield return new object[] { "", new List<OrderDto>(), false }; // Caso Name vacío
            yield return new object[] { "Jordi", null, false }; // Caso Orders vacío
        }

        [Theory]
        [MemberData(nameof(GetUserData))]
        public void ValidateMessageTest(string name, List<OrderDto> order, bool isValid)
        {
            var dto = DtoMockCreator.CreateUserDTO(name, order);

            var validator = new UserDTOValidator();

            var validationResult = validator.Validate(dto);
            validationResult.IsValid.Should().Be(isValid);
        }
    }
}
