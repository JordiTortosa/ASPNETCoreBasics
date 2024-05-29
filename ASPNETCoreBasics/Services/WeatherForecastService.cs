namespace ASPNETCoreBasics.Services
{
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading.Tasks;
    using ASPNETCoreBasics.Configurations;
    using ASPNETCoreBasics.Models;
    using ASPNETCoreBasics.Repository;
    using AutoMapper;
    using Microsoft.Extensions.Logging;

    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IWeatherForecastRepository _weatherForecastRepository;
        private readonly ILogger<WeatherForecastService> _logger;
        private readonly MyService _myService;
        private readonly IMapper _mapper;

        public WeatherForecastService(IWeatherForecastRepository weatherForecastRepository, ILogger<WeatherForecastService> logger, MyService myService, IMapper mapper)
        {
            _weatherForecastRepository = weatherForecastRepository;
            _logger = logger;
            _myService = myService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WeatherForecastDto>> GetWeatherForecastsAsync()
        {
            _logger.LogInformation("Getting weather forecasts");
            var weatherforecast = await _weatherForecastRepository.GetWeatherForecastsAsync();
            var weatherforecastDto = _mapper.Map<IEnumerable<WeatherForecastDto>>(weatherforecast);
            return weatherforecastDto;
        }

        public async Task<WeatherForecastDto> CreateWeatherForecastAsync(WeatherForecastDto weatherForecastDto)
        {
            _logger.LogInformation("Create weather forecast");
            var weatherForecast = _mapper.Map<WeatherForecastModel>(weatherForecastDto);
            var createdForecast = await _weatherForecastRepository.CreateWeatherForecastAsync(weatherForecast);
            var createdForecastDto = _mapper.Map<WeatherForecastDto>(createdForecast);
            return createdForecastDto;
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

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            _logger.LogInformation("Get users");
            var users = await _weatherForecastRepository.GetUsersAsync();
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
            return usersDto;
        }

        public async Task<UserDto> CreateUser(UserDto userDto)
        {
            _logger.LogInformation("Create users");
            var user = _mapper.Map<UserModel>(userDto);
            var createdUser = await _weatherForecastRepository.CreateUserAsync(user);
            var createdUserDto = _mapper.Map<UserDto>(createdUser);
            return createdUserDto;
        }

        public async Task<IEnumerable<OrderDto>> GetOrders()
        {
            _logger.LogInformation("Get orders");
            var orders = await _weatherForecastRepository.GetOrdersAsync();
            var ordersDto = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return ordersDto;
        }

        public async Task<OrderDto> CreateOrder(OrderDto orderDto)
        {
            _logger.LogInformation("Get order");
            var order = _mapper.Map<OrderModel>(orderDto);
            var createdOrder = await _weatherForecastRepository.CreateOrderAsync(order);
            var createdOrderDto = _mapper.Map<OrderDto>(createdOrder);
            return createdOrderDto;
        }

        public async Task<IEnumerable<UserDto>> GetUsersWithOrders()
        {
            _logger.LogInformation("Get users and their corresponding orders");
            var usersWithOrders = await _weatherForecastRepository.GetUsersWithOrdersAsync();
            var usersWithOrdersDto = _mapper.Map<IEnumerable<UserDto>>(usersWithOrders);
            return usersWithOrdersDto;
        }
    }
}
