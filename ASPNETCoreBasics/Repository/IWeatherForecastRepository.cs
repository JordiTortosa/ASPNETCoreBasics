using ASPNETCoreBasics.Models;
using System.Text.Json;

namespace ASPNETCoreBasics.Repository
{
    public interface IWeatherForecastRepository
    {
        Task<IEnumerable<WeatherForecastModel>> GetWeatherForecastsAsync();

        Task<WeatherForecastModel> GetWeatherForecastByIdAsync(int id);

        Task<WeatherForecastModel> CreateWeatherForecastAsync(WeatherForecastModel weatherForecast);

        Task<bool> UpdateWeatherForecastAsync(WeatherForecastModel weatherForecast, JsonElement json);

        Task<bool> DeleteWeatherForecastAsync(int id);

        Task<bool> WeatherForecastExistsAsync(int id);

        Task<IEnumerable<UserModel>> GetUsersAsync();

        Task<UserModel> CreateUserAsync(UserModel user);

        Task<IEnumerable<OrderModel>> GetOrdersAsync();

        Task<OrderModel> CreateOrderAsync(OrderModel order);

        Task<IEnumerable<UserModel>> GetUsersWithOrdersAsync();
    }
}