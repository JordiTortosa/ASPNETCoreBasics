using ASPNETCoreBasics.Models;
using System.Text.Json;

namespace ASPNETCoreBasics.Services
{
    public interface IWeatherForecastService
    {
        Task<IEnumerable<WeatherForecastDto>> GetWeatherForecastsAsync();
        Task<WeatherForecastDto> CreateWeatherForecastAsync(WeatherForecastDto weatherForecastDto);
        Task<bool> UpdateWeatherForecastAsync(int id, JsonElement json);
        Task<bool> DeleteWeatherForecastAsync(int id);
        Task<IEnumerable<UserDto>> GetUsers();
        Task<UserDto> CreateUser(UserDto userDto);
        Task<IEnumerable<OrderDto>> GetOrders();
        Task<OrderDto> CreateOrder(OrderDto orderDto);
        Task<IEnumerable<UserDto>> GetUsersWithOrders();
    }
}
