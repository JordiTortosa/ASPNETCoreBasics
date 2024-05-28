using ASPNETCoreBasics.Configurations;
using ASPNETCoreBasics.Controllers;
using ASPNETCoreBasics.Models;
using ASPNETCoreBasics.Services;
using ASPNETCoreBasicsTests.Mocks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ASPNETCoreBasics.Configurations;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ASPNETCoreBasicsTests.Controllers
{
    public class WeatherForecastControllerTest
    {

        private readonly WeatherForecastController _controller;
        private readonly Mock<ILogger<WeatherForecastController>> _loggerMock;
        private readonly Mock<IWeatherForecastService> _weatherForecastServiceMock;
        private readonly Mock<HealthCheckService> _healthCheckMock;
        private readonly UserDto _userTestDto;
        private readonly OrderDto _orderDto;
        private readonly WeatherForecastDto _weatherForecastTestDto;
        private readonly MyService _myService;
        private readonly IConfiguration _configuration;

        public WeatherForecastControllerTest()
        {
            _loggerMock = new Mock<ILogger<WeatherForecastController>>();
            _weatherForecastServiceMock = new Mock<IWeatherForecastService>();
            _healthCheckMock = new Mock<HealthCheckService>();
            _myService = new MyService((Microsoft.Extensions.Configuration.IConfiguration)_configuration);

            _controller = new WeatherForecastController(_weatherForecastServiceMock.Object, _loggerMock.Object, _myService);

            _weatherForecastTestDto = DtoMockCreator.CreateWeatherForecastDTO(new DateOnly(2024, 05, 28), 20, "Cold");

            List<OrderDto> orders = new List<OrderDto>
        {
            DtoMockCreator.CreateOrderDTO("Pedido de Prueba 1"),
            DtoMockCreator.CreateOrderDTO("Pedido de Prueba 2"),
            DtoMockCreator.CreateOrderDTO("Pedido de Prueba 3"),
        };
            _userTestDto = DtoMockCreator.CreateUserDTO("Carlos", orders);

            _orderDto = DtoMockCreator.CreateOrderDTO("Pedido de Prueba 4");
        }

        [Fact]
        public async Task GetWeatherForecast_Success()
        {
            // Arrange
            var weatherForecastList = new List<WeatherForecastDto>
            {
                DtoMockCreator.CreateWeatherForecastDTO(new DateOnly(2024, 05, 28), 15, "SuperCold"),
                DtoMockCreator.CreateWeatherForecastDTO(new DateOnly(2024, 05, 29), 30, "SuperHot"),
            };
            _weatherForecastServiceMock.Setup(s => s.GetWeatherForecastsAsync()).ReturnsAsync(weatherForecastList);

            // Act
            var result = await _controller.GetWeatherForecasts();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<WeatherForecastDto>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<List<WeatherForecastDto>>(okResult.Value);

            Assert.Equal(weatherForecastList.Count, returnValue.Count);
            Assert.Equal(weatherForecastList[0].Date, returnValue[0].Date);
            Assert.Equal(weatherForecastList[0].TemperatureC, returnValue[0].TemperatureC);
            Assert.Equal(weatherForecastList[0].Summary, returnValue[0].Summary);
            Assert.Equal(weatherForecastList[1].Date, returnValue[1].Date);
            Assert.Equal(weatherForecastList[1].TemperatureC, returnValue[1].TemperatureC);
            Assert.Equal(weatherForecastList[1].Summary, returnValue[1].Summary);

            _weatherForecastServiceMock.Verify(s => s.GetWeatherForecastsAsync(), Times.Once);
        }
        [Fact]
        public async Task GetWeatherForecast_NotFound()
        {
            // Arrange
            _weatherForecastServiceMock.Setup(s => s.GetWeatherForecastsAsync()).ReturnsAsync((List<WeatherForecastDto>)null);

            // Act
            var result = await _controller.GetWeatherForecasts();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<WeatherForecastDto>>>(result);
            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult.Result);

            _weatherForecastServiceMock.Verify(s => s.GetWeatherForecastsAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateWeatherForecast_Success()
        {
            // Arrange
            var weatherForecastDto = _weatherForecastTestDto;
            _weatherForecastServiceMock.Setup(s => s.CreateWeatherForecastAsync(It.IsAny<WeatherForecastDto>())).ReturnsAsync(weatherForecastDto);

            // Act
            var result = await _controller.CreateWeatherForecast(weatherForecastDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<WeatherForecastDto>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<WeatherForecastDto>(createdAtActionResult.Value);

            Assert.Equal(weatherForecastDto.Date, returnValue.Date);
            Assert.Equal(weatherForecastDto.TemperatureC, returnValue.TemperatureC);
            Assert.Equal(weatherForecastDto.Summary, returnValue.Summary);

            _weatherForecastServiceMock.Verify(s => s.CreateWeatherForecastAsync(It.Is<WeatherForecastDto>(dto =>
                dto.Date == weatherForecastDto.Date &&
                dto.TemperatureC == weatherForecastDto.TemperatureC &&
                dto.Summary == weatherForecastDto.Summary)), Times.Once);
        }
        [Fact]
        public async Task CreateWeatherForecast_Failure()
        {
            // Arrange
            var weatherForecastDto = _weatherForecastTestDto;
            _weatherForecastServiceMock.Setup(s => s.CreateWeatherForecastAsync(It.IsAny<WeatherForecastDto>()))
                                       .ReturnsAsync((WeatherForecastDto)null);

            // Act
            var result = await _controller.CreateWeatherForecast(weatherForecastDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<WeatherForecastDto>>(result);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Failed to create weather forecast.", badRequestObjectResult.Value);
        }

        [Fact]
        public async Task DeleteWeatherForecast_Success()
        {
            // Arrange
            int weatherForecastId = 1;
            _weatherForecastServiceMock.Setup(s => s.DeleteWeatherForecastAsync(weatherForecastId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteWeatherForecast(weatherForecastId);

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            _weatherForecastServiceMock.Verify(s => s.DeleteWeatherForecastAsync(weatherForecastId), Times.Once);
        }
        [Fact]
        public async Task DeleteWeatherForecast_NotFound()
        {
            // Arrange
            int weatherForecastId = 1;
            _weatherForecastServiceMock.Setup(s => s.DeleteWeatherForecastAsync(weatherForecastId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteWeatherForecast(weatherForecastId);

            // Assert
            var actionResult = Assert.IsType<NotFoundResult>(result);
            _weatherForecastServiceMock.Verify(s => s.DeleteWeatherForecastAsync(weatherForecastId), Times.Once);
        }

        [Fact]
        public async Task UpdateWeatherForecast_ValidData_ReturnsNoContent()
        {
            // Arrange
            int id = 1;
            var validJson = JsonSerializer.Serialize(new
            {
                date = "2024-05-28",
                temperatureC = 20,
                summary = "Cold"
            });

            _weatherForecastServiceMock.Setup(service => service.UpdateWeatherForecastAsync(id, It.IsAny<JsonElement>())).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateWeatherForecast(id, JsonDocument.Parse(validJson).RootElement);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

    }
}
