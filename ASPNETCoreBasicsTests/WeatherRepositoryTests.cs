using ASPNETCoreBasics.Contexts;
using ASPNETCoreBasics.Contexts.ASPNETCoreBasics.Models;
using ASPNETCoreBasics.Models;
using ASPNETCoreBasics.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace ASPNETCoreBasicsTests
{
    public class WeatherForecastRepositoryTests
    {
        private readonly WeatherForecastDbContext _context;
        private readonly UserContext _userContext;
        private readonly UserContext _orderContext;

        public WeatherForecastRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<WeatherForecastDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new WeatherForecastDbContext(options);

            _context.WeatherForecasts.AddRange(new List<WeatherForecastModel>
        {
            new WeatherForecastModel { Date = new DateOnly(2024,05,22), TemperatureC = 25, Summary = "Sunny" },
            new WeatherForecastModel { Date = new DateOnly(2024,05,24), TemperatureC = 18, Summary = "Cloudy" }
        });
            _context.SaveChanges();

            var userOptions = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase2")
                .Options;

            _userContext = new UserContext(userOptions);

            _userContext.Usuarios.AddRange(new List<UserModel>
        {
            new UserModel { Name="Test1", Orders = [new OrderModel {Description= "Test1" }] },
            new UserModel { Name="Test2", Orders = [new OrderModel {Description= "Test2" }] },
        });
            _context.SaveChanges();

            var orderOptions = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase2")
                .Options;

            _orderContext = new UserContext(orderOptions);

            _orderContext.Pedidos.AddRange(new List<OrderModel>
        {
            new OrderModel { Description = "Test1" },
            new OrderModel { Description = "Test2" },
        });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetWeatherForecastsAsync_ReturnsForecasts()
        {
            var mockUserContext = new Mock<UserContext>(new DbContextOptionsBuilder<UserContext>()
            .UseInMemoryDatabase(databaseName: "TestUserDatabase")
            .Options);

            var repository = new WeatherForecastRepository(_context, mockUserContext.Object);

            var result = await repository.GetWeatherForecastsAsync();

            result.Should().NotBeEmpty();
            result.First().Summary.Should().Be("Sunny");
        }

        [Fact]
        public async Task GetWeatherForecastByIdAsync_ReturnsCorrectForecast()
        {
            var repository = new WeatherForecastRepository(_context, _userContext);
            var existingForecast = await repository.GetWeatherForecastsAsync();
            var firstForecastId = existingForecast.First().Id;

            var result = await repository.GetWeatherForecastByIdAsync(firstForecastId);

            result.Should().NotBeNull();
            result.Id.Should().Be(firstForecastId);
        }

        [Fact]
        public async Task CreateWeatherForecastAsync_CreatesForecast()
        {
            var repository = new WeatherForecastRepository(_context, _userContext);
            var newForecast = new WeatherForecastModel { Date = new DateOnly(2024, 05, 27), TemperatureC = 20, Summary = "Partly cloudy" };

            var result = await repository.CreateWeatherForecastAsync(newForecast);

            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.TemperatureC.Should().Be(20);
        }

        [Fact]
        public async Task DeleteWeatherForecastAsync_DeletesForecast()
        {
            var repository = new WeatherForecastRepository(_context, _userContext);
            var existingForecast = (await repository.GetWeatherForecastsAsync()).First();

            var result = await repository.DeleteWeatherForecastAsync(existingForecast.Id);

            result.Should().BeTrue();
            var deletedForecast = await repository.GetWeatherForecastByIdAsync(existingForecast.Id);
            deletedForecast.Should().BeNull();
        }

        [Fact]
        public async Task GetUsersAsync_ReturnsUsers()
        {
            var repository = new WeatherForecastRepository(_context, _userContext);

            var result = await repository.GetUsersAsync();

            result.Should().NotBeNull();
            result.First().Name.Should().Be("Test1");
        }

        [Fact]
        public async Task CreateUserAsync_CreatesUser()
        {
            var repository = new WeatherForecastRepository(_context, _userContext);
            var newUser = new UserModel { Name = "Test3", Orders = [new OrderModel { Description = "Test3" }] };

            var result = await repository.CreateUserAsync(newUser);

            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetOrdersAsync_ReturnsOrders()
        {
            var repository = new WeatherForecastRepository(_context, _orderContext);

            var result = await repository.GetOrdersAsync();

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateOrderAsync_CreatesOrder()
        {
            var repository = new WeatherForecastRepository(_context, _orderContext);
            var newOrder = new OrderModel { Description = "Test" };

            var result = await repository.CreateOrderAsync(newOrder);

            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetUsersWithOrdersAsync_ReturnsUsersWithOrders()
        {
            var repository = new WeatherForecastRepository(_context, _userContext);

            var result = await repository.GetUsersWithOrdersAsync();

            result.Should().NotBeNull();
        }
    }
}