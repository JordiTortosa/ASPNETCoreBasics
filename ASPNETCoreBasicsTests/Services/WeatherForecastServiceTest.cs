using ASPNETCoreBasics.Configurations;
using ASPNETCoreBasics.Models;
using ASPNETCoreBasics.Repository;
using ASPNETCoreBasics.Services;
using ASPNETCoreBasicsTests.Mocks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using Xunit;

namespace ASPNETCoreBasicsTests.Services
{
    public class WeatherForecastServiceTest
    {
        private readonly Mock<IWeatherForecastRepository> _weatherForecastRepositoryMock;
        private readonly Mock<ILogger<WeatherForecastService>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly WeatherForecastService _weatherForecastService;
        private readonly WeatherForecastDto _weatherForecastTestDto;

        public WeatherForecastServiceTest()
        {
            _weatherForecastRepositoryMock = new Mock<IWeatherForecastRepository>();
            _loggerMock = new Mock<ILogger<WeatherForecastService>>();
            _mapperMock = new Mock<IMapper>();

            _weatherForecastService = new WeatherForecastService(_weatherForecastRepositoryMock.Object, _loggerMock.Object, _mapperMock.Object);

            _weatherForecastTestDto = DtoMockCreator.CreateWeatherForecastDTO(new DateOnly(2024, 05, 28), 20, "Cold");
        }

        [Fact]
        public async Task GetWeatherForecastsAsync_Success()
        {
            // Arrange
            var weatherForecasts = new List<WeatherForecastModel>
            {
                new WeatherForecastModel { Date = new DateOnly(2024, 05, 28), TemperatureC = 15, Summary = "SuperCold" },
                new WeatherForecastModel { Date = new DateOnly(2024, 05, 29), TemperatureC = 30, Summary = "SuperHot" }
            };

            var weatherForecastList = new List<WeatherForecastDto>
            {
                DtoMockCreator.CreateWeatherForecastDTO(new DateOnly(2024, 05, 28), 15, "SuperCold"),
                DtoMockCreator.CreateWeatherForecastDTO(new DateOnly(2024, 05, 29), 30, "SuperHot"),
            };

            _weatherForecastRepositoryMock.Setup(repo => repo.GetWeatherForecastsAsync()).ReturnsAsync(weatherForecasts);
            _mapperMock.Setup(m => m.Map<IEnumerable<WeatherForecastDto>>(It.IsAny<IEnumerable<WeatherForecastModel>>())).Returns(weatherForecastList);

            // Act
            var result = await _weatherForecastService.GetWeatherForecastsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(weatherForecastList.Count, result.Count());
            Assert.Equal(weatherForecastList, result); // Verifica que los objetos sean iguales

            _weatherForecastRepositoryMock.Verify(repo => repo.GetWeatherForecastsAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<WeatherForecastDto>>(weatherForecasts), Times.Once);
        }

        [Fact]
        public async Task CreateWeatherForecastAsync_Success()
        {
            // Arrange
            var weatherForecastModel = new WeatherForecastModel { Date = _weatherForecastTestDto.Date, TemperatureC = _weatherForecastTestDto.TemperatureC, Summary = _weatherForecastTestDto.Summary };

            _mapperMock.Setup(m => m.Map<WeatherForecastModel>(It.IsAny<WeatherForecastDto>())).Returns(weatherForecastModel);
            _weatherForecastRepositoryMock.Setup(repo => repo.CreateWeatherForecastAsync(It.IsAny<WeatherForecastModel>())).ReturnsAsync(weatherForecastModel);
            _mapperMock.Setup(m => m.Map<WeatherForecastDto>(It.IsAny<WeatherForecastModel>())).Returns(_weatherForecastTestDto);

            // Act
            var result = await _weatherForecastService.CreateWeatherForecastAsync(_weatherForecastTestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_weatherForecastTestDto.Date, result.Date);
            Assert.Equal(_weatherForecastTestDto.TemperatureC, result.TemperatureC);
            Assert.Equal(_weatherForecastTestDto.Summary, result.Summary);

            _weatherForecastRepositoryMock.Verify(repo => repo.CreateWeatherForecastAsync(weatherForecastModel), Times.Once);
            _mapperMock.Verify(m => m.Map<WeatherForecastModel>(_weatherForecastTestDto), Times.Once);
            _mapperMock.Verify(m => m.Map<WeatherForecastDto>(weatherForecastModel), Times.Once);
        }

        [Fact]
        public async Task CreateWeatherForecastAsync_Failure()
        {
            // Arrange
            var weatherForecastModel = new WeatherForecastModel { Date = _weatherForecastTestDto.Date, TemperatureC = _weatherForecastTestDto.TemperatureC, Summary = _weatherForecastTestDto.Summary };

            _mapperMock.Setup(m => m.Map<WeatherForecastModel>(It.IsAny<WeatherForecastDto>())).Returns(weatherForecastModel);
            _weatherForecastRepositoryMock.Setup(repo => repo.CreateWeatherForecastAsync(It.IsAny<WeatherForecastModel>())).ReturnsAsync((WeatherForecastModel)null);

            // Act
            var result = await _weatherForecastService.CreateWeatherForecastAsync(_weatherForecastTestDto);

            // Assert
            Assert.Null(result);
            _weatherForecastRepositoryMock.Verify(repo => repo.CreateWeatherForecastAsync(weatherForecastModel), Times.Once);
            _mapperMock.Verify(m => m.Map<WeatherForecastModel>(_weatherForecastTestDto), Times.Once);
            // No necesitamos verificar el mapeo de un nulo, porque no se debería llamar en ese caso
        }

        [Fact]
        public async Task DeleteWeatherForecastAsync_Success()
        {
            // Arrange
            int weatherForecastId = 1;
            _weatherForecastRepositoryMock.Setup(repo => repo.DeleteWeatherForecastAsync(weatherForecastId)).ReturnsAsync(true);

            // Act
            var result = await _weatherForecastService.DeleteWeatherForecastAsync(weatherForecastId);

            // Assert
            Assert.True(result);
            _weatherForecastRepositoryMock.Verify(repo => repo.DeleteWeatherForecastAsync(weatherForecastId), Times.Once);
        }

        [Fact]
        public async Task DeleteWeatherForecastAsync_NotFound()
        {
            // Arrange
            int weatherForecastId = 1;
            _weatherForecastRepositoryMock.Setup(repo => repo.DeleteWeatherForecastAsync(weatherForecastId)).ReturnsAsync(false);

            // Act
            var result = await _weatherForecastService.DeleteWeatherForecastAsync(weatherForecastId);

            // Assert
            Assert.False(result);
            _weatherForecastRepositoryMock.Verify(repo => repo.DeleteWeatherForecastAsync(weatherForecastId), Times.Once);
        }

        [Fact]
        public async Task UpdateWeatherForecastAsync_ValidData_ReturnsTrue()
        {
            // Arrange
            int id = 1;
            var validJson = JsonSerializer.Serialize(new
            {
                date = "2024-05-28",
                temperatureC = 20,
                summary = "Cold"
            });

            var weatherForecastModel = new WeatherForecastModel { Date = new DateOnly(2024, 05, 28), TemperatureC = 20, Summary = "Cold" };
            _weatherForecastRepositoryMock.Setup(repo => repo.GetWeatherForecastByIdAsync(id)).ReturnsAsync(weatherForecastModel);
            _weatherForecastRepositoryMock.Setup(repo => repo.UpdateWeatherForecastAsync(weatherForecastModel, It.IsAny<JsonElement>())).ReturnsAsync(true);

            // Act
            var result = await _weatherForecastService.UpdateWeatherForecastAsync(id, JsonDocument.Parse(validJson).RootElement);

            // Assert
            Assert.True(result);
            _weatherForecastRepositoryMock.Verify(repo => repo.GetWeatherForecastByIdAsync(id), Times.Once);
            _weatherForecastRepositoryMock.Verify(repo => repo.UpdateWeatherForecastAsync(weatherForecastModel, It.IsAny<JsonElement>()), Times.Once);
        }

        [Fact]
        public async Task UpdateWeatherForecastAsync_InvalidId_ReturnsFalse()
        {
            // Arrange
            int id = 1;
            var validJson = JsonSerializer.Serialize(new
            {
                date = "2024-05-28",
                temperatureC = 20,
                summary = "Cold"
            });

            _weatherForecastRepositoryMock.Setup(repo => repo.GetWeatherForecastByIdAsync(id)).ReturnsAsync((WeatherForecastModel)null);

            // Act
            var result = await _weatherForecastService.UpdateWeatherForecastAsync(id, JsonDocument.Parse(validJson).RootElement);

            // Assert
            Assert.False(result);
            _weatherForecastRepositoryMock.Verify(repo => repo.GetWeatherForecastByIdAsync(id), Times.Once);
            _weatherForecastRepositoryMock.Verify(repo => repo.UpdateWeatherForecastAsync(It.IsAny<WeatherForecastModel>(), It.IsAny<JsonElement>()), Times.Never);
        }

        [Fact]
        public async Task GetUsers_ReturnsListOfUserDtos()
        {
            // Arrange
            var users = new List<UserModel>
            {
                new UserModel { Id = 1, Name = "Carlos" },
                new UserModel { Id = 2, Name = "Ana" }
            };
            var userDtos = new List<UserDto>
            {
                new UserDto { Id = 1, Name = "Carlos" },
                new UserDto { Id = 2, Name = "Ana" }
            };

            _weatherForecastRepositoryMock.Setup(repo => repo.GetUsersAsync()).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IEnumerable<UserDto>>(users)).Returns(userDtos);

            // Act
            var result = await _weatherForecastService.GetUsers();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDtos.Count, result.Count());
            _weatherForecastRepositoryMock.Verify(repo => repo.GetUsersAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<UserDto>>(users), Times.Once);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreatedUserDto()
        {
            // Arrange
            var userDto = new UserDto { Name = "Carlos" };
            var user = new UserModel { Name = "Carlos" };
            var createdUser = new UserModel { Id = 1, Name = "Carlos" };
            var createdUserDto = new UserDto { Id = 1, Name = "Carlos" };

            _mapperMock.Setup(m => m.Map<UserModel>(userDto)).Returns(user);
            _weatherForecastRepositoryMock.Setup(repo => repo.CreateUserAsync(user)).ReturnsAsync(createdUser);
            _mapperMock.Setup(m => m.Map<UserDto>(createdUser)).Returns(createdUserDto);

            // Act
            var result = await _weatherForecastService.CreateUser(userDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createdUserDto.Id, result.Id);
            Assert.Equal(createdUserDto.Name, result.Name);

            _weatherForecastRepositoryMock.Verify(repo => repo.CreateUserAsync(user), Times.Once);
            _mapperMock.Verify(m => m.Map<UserModel>(userDto), Times.Once);
            _mapperMock.Verify(m => m.Map<UserDto>(createdUser), Times.Once);
        }

        [Fact]
        public async Task GetOrders_ReturnsListOfOrderDtos()
        {
            // Arrange
            var orders = new List<OrderModel>
            {
                new OrderModel { Id = 1, Description = "Order 1" },
                new OrderModel { Id = 2, Description = "Order 2" }
            };
            var orderDtos = new List<OrderDto>
            {
                new OrderDto { Id = 1, Description = "Order 1" },
                new OrderDto { Id = 2, Description = "Order 2" }
            };

            _weatherForecastRepositoryMock.Setup(repo => repo.GetOrdersAsync()).ReturnsAsync(orders);
            _mapperMock.Setup(m => m.Map<IEnumerable<OrderDto>>(orders)).Returns(orderDtos);

            // Act
            var result = await _weatherForecastService.GetOrders();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderDtos.Count, result.Count());
            _weatherForecastRepositoryMock.Verify(repo => repo.GetOrdersAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<OrderDto>>(orders), Times.Once);
        }

        [Fact]
        public async Task CreateOrder_ReturnsCreatedOrderDto()
        {
            // Arrange
            var orderDto = new OrderDto { Description = "Order 1" };
            var order = new OrderModel { Description = "Order 1" };
            var createdOrder = new OrderModel { Id = 1, Description = "Order 1" };
            var createdOrderDto = new OrderDto { Id = 1, Description = "Order 1" };

            _mapperMock.Setup(m => m.Map<OrderModel>(orderDto)).Returns(order);
            _weatherForecastRepositoryMock.Setup(repo => repo.CreateOrderAsync(order)).ReturnsAsync(createdOrder);
            _mapperMock.Setup(m => m.Map<OrderDto>(createdOrder)).Returns(createdOrderDto);

            // Act
            var result = await _weatherForecastService.CreateOrder(orderDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createdOrderDto.Id, result.Id);
            Assert.Equal(createdOrderDto.Description, result.Description);

            _weatherForecastRepositoryMock.Verify(repo => repo.CreateOrderAsync(order), Times.Once);
            _mapperMock.Verify(m => m.Map<OrderModel>(orderDto), Times.Once);
            _mapperMock.Verify(m => m.Map<OrderDto>(createdOrder), Times.Once);
        }

        [Fact]
        public async Task GetUsersWithOrders_ReturnsListOfUserDtosWithOrders()
        {
            // Arrange
            var usersWithOrders = new List<UserModel>
            {
                new UserModel
                {
                    Id = 1,
                    Name = "Carlos",
                    Orders = new List<OrderModel>
                    {
                        new OrderModel { Id = 1, Description = "Order 1" }
                    }
                },
                new UserModel
                {
                    Id = 2,
                    Name = "Ana",
                    Orders = new List<OrderModel>
                    {
                        new OrderModel { Id = 2, Description = "Order 2" }
                    }
                }
            };
            var usersWithOrdersDto = new List<UserDto>
            {
                new UserDto
                {
                    Id = 1,
                    Name = "Carlos",
                    Orders = new List<OrderDto>
                    {
                        new OrderDto { Id = 1, Description = "Order 1" }
                    }
                },
                new UserDto
                {
                    Id = 2,
                    Name = "Ana",
                    Orders = new List<OrderDto>
                    {
                        new OrderDto { Id = 2, Description = "Order 2" }
                    }
                }
            };

            _weatherForecastRepositoryMock.Setup(repo => repo.GetUsersWithOrdersAsync()).ReturnsAsync(usersWithOrders);
            _mapperMock.Setup(m => m.Map<IEnumerable<UserDto>>(usersWithOrders)).Returns(usersWithOrdersDto);

            // Act
            var result = await _weatherForecastService.GetUsersWithOrders();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(usersWithOrdersDto.Count, result.Count());
            _weatherForecastRepositoryMock.Verify(repo => repo.GetUsersWithOrdersAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<UserDto>>(usersWithOrders), Times.Once);
        }
    }
}