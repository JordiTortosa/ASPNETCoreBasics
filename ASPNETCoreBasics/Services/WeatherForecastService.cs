namespace ASPNETCoreBasics.Services
{
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading.Tasks;
    using ASPNETCoreBasics.Configurations;
    using ASPNETCoreBasics.Models;
    using ASPNETCoreBasics.Repository;
    using Microsoft.Extensions.Logging;

    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IWeatherForecastRepository _weatherForecastRepository;
        private readonly ILogger<WeatherForecastService> _logger;
        private readonly MyService _myService;

        public WeatherForecastService(IWeatherForecastRepository weatherForecastRepository, ILogger<WeatherForecastService> logger, MyService myService)
        {
            _weatherForecastRepository = weatherForecastRepository;
            _logger = logger;
            _myService = myService;
        }

        public async Task<IEnumerable<WeatherForecastModel>> GetWeatherForecastsAsync()
        {
            _logger.LogInformation("Getting weather forecasts");
            return await _weatherForecastRepository.GetWeatherForecastsAsync();
        }

        public async Task<WeatherForecastModel> CreateWeatherForecastAsync(WeatherForecastModel weatherForecast)
        {
            _logger.LogInformation("Create weather forecast");
            return await _weatherForecastRepository.CreateWeatherForecastAsync(weatherForecast);
        }

        public async Task<bool> UpdateWeatherForecastAsync(int id, JsonElement json)
        {
            var weatherForecast = await _weatherForecastRepository.GetWeatherForecastByIdAsync(id);
            if (weatherForecast == null)
            {
                return false;
            }
            _logger.LogInformation("Update weather forecasts with id: {id}", id);
            return await _weatherForecastRepository.UpdateWeatherForecastAsync(weatherForecast, json);
        }

        public async Task<bool> DeleteWeatherForecastAsync(int id)
        {
            _logger.LogInformation("Delete weather forecasts with id: {id}", id);
            return await _weatherForecastRepository.DeleteWeatherForecastAsync(id);
        }

        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            _logger.LogInformation("Get users");
            return await _weatherForecastRepository.GetUsersAsync();
        }

        public async Task<UserModel> CreateUser(UserModel user)
        {
            _logger.LogInformation("Create users");
            return await _weatherForecastRepository.CreateUserAsync(user);
        }

        public async Task<IEnumerable<OrderModel>> GetOrders()
        {
            _logger.LogInformation("Get orders");
            return await _weatherForecastRepository.GetOrdersAsync();
        }

        public async Task<OrderModel> CreateOrder(OrderModel order)
        {
            _logger.LogInformation("Get order");
            return await _weatherForecastRepository.CreateOrderAsync(order);
        }

        public async Task<IEnumerable<UserModel>> GetUsersWithOrders()
        {
            _logger.LogInformation("Get users and their corresponding orders");
            return await _weatherForecastRepository.GetUsersWithOrdersAsync();
        }
    }
}
