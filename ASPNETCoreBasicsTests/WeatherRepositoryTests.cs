using ASPNETCoreBasics.Contexts;
using ASPNETCoreBasics.Contexts.ASPNETCoreBasics.Models;
using ASPNETCoreBasics.Models;
using ASPNETCoreBasics.Repository;
using Moq;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.InMemory.Storage.Internal.InMemoryDatabase;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;
using FluentAssertions;

namespace ASPNETCoreBasicsTests
{

    public class WeatherForecastRepositoryTests
    {
        private readonly WeatherForecastDbContext _context;
        private readonly UserContext _userContext;

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
        }

        [Fact]
        public async Task GetWeatherForecastsAsync_ReturnsForecasts()
        {
            var mockUserContext = new Mock<UserContext>();
            var repository = new WeatherForecastRepository(_context, mockUserContext.Object);

            var result = await repository.GetWeatherForecastsAsync();

            result.Should().NotBeEmpty();
            result.Should().HaveCount(2);
            result.First().Summary.Should().Be("Sunny");
        }
        [Fact]
        public async Task GetWeatherForecastByIdAsync_ReturnsCorrectForecast()
        {
            var mockUserContext = new Mock<UserContext>();
            var repository = new WeatherForecastRepository(_context, mockUserContext.Object);
            var existingForecast = await repository.GetWeatherForecastsAsync();
            var firstForecastId = existingForecast.First().Id;

            var result = await repository.GetWeatherForecastByIdAsync(firstForecastId);

            result.Should().NotBeNull();
            result.Id.Should().Be(firstForecastId);
        }

        [Fact]
        public async Task CreateWeatherForecastAsync_CreatesForecast()
        {
            var mockUserContext = new Mock<UserContext>();
            var repository = new WeatherForecastRepository(_context, mockUserContext.Object);
            var newForecast = new WeatherForecastModel { Date = new DateOnly(2024, 05, 27), TemperatureC = 20, Summary = "Partly cloudy" };

            var result = await repository.CreateWeatherForecastAsync(newForecast);

            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task DeleteWeatherForecastAsync_DeletesForecast()
        {
            var mockUserContext = new Mock<UserContext>();
            var repository = new WeatherForecastRepository(_context, mockUserContext.Object);
            var existingForecast = (await repository.GetWeatherForecastsAsync()).First();

            var result = await repository.DeleteWeatherForecastAsync(existingForecast.Id);

            result.Should().BeTrue();
            var deletedForecast = await repository.GetWeatherForecastByIdAsync(existingForecast.Id);
            deletedForecast.Should().BeNull();
        }
    }
}