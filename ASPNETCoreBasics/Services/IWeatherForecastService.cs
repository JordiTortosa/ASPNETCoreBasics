using ASPNETCoreBasics.Models;
using System.Text.Json;

namespace ASPNETCoreBasics.Services
{
    public interface IWeatherForecastService
    {
        Task<IEnumerable<WeatherForecastModel>> GetWeatherForecastsAsync();
        Task<WeatherForecastModel> CreateWeatherForecastAsync(WeatherForecastModel weatherForecast);
        Task<bool> UpdateWeatherForecastAsync(int id, JsonElement json);
        Task<bool> DeleteWeatherForecastAsync(int id);
        Task<IEnumerable<UserModel>> GetUsers();
        Task<UserModel> CreateUser(UserModel user);
        Task<IEnumerable<OrderModel>> GetOrders();
        Task<OrderModel> CreateOrder(OrderModel order);
        Task<IEnumerable<UserModel>> GetUsersWithOrders();
    }
}
