namespace ASPNETCoreBasics.Repository
{
    using ASPNETCoreBasics.Contexts;
    using ASPNETCoreBasics.Contexts.ASPNETCoreBasics.Models;
    using ASPNETCoreBasics.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class WeatherForecastRepository : IWeatherForecastRepository
    {
        private readonly WeatherForecastDbContext _weatherForecastContext;
        private readonly UserContext _userContext;

        public WeatherForecastRepository(WeatherForecastDbContext weatherForecastContext, UserContext userContext)
        {
            _weatherForecastContext = weatherForecastContext;
            _userContext = userContext;
        }

        public async Task<IEnumerable<WeatherForecastModel>> GetWeatherForecastsAsync()
        {
            return await _weatherForecastContext.WeatherForecasts.ToListAsync();
        }

        public async Task<WeatherForecastModel> GetWeatherForecastByIdAsync(int id)
        {
            return await _weatherForecastContext.WeatherForecasts.FindAsync(id);
        }

        public async Task<WeatherForecastModel> CreateWeatherForecastAsync(WeatherForecastModel weatherForecast)
        {
            _weatherForecastContext.WeatherForecasts.Add(weatherForecast);
            await _weatherForecastContext.SaveChangesAsync();
            return weatherForecast;
        }

        public async Task<bool> UpdateWeatherForecastAsync(WeatherForecastModel weatherForecast, JsonElement json)
        {
            using (JsonDocument doc = JsonDocument.Parse(json.GetRawText()))
            {
                var root = doc.RootElement;
                if (root.TryGetProperty("date", out var dateElement))
                {
                    if (DateTime.TryParse(dateElement.GetString(), out DateTime dateValue))
                    {
                        weatherForecast.Date = new DateOnly(dateValue.Year, dateValue.Month, dateValue.Day);
                    }
                    else
                    {
                        return false;
                    }
                }
                if (root.TryGetProperty("temperatureC", out var temperatureElement))
                {
                    weatherForecast.TemperatureC = temperatureElement.GetInt32();
                }
                if (root.TryGetProperty("summary", out var summaryElement))
                {
                    weatherForecast.Summary = summaryElement.GetString();
                }
            }

            _weatherForecastContext.WeatherForecasts.Update(weatherForecast);
            try
            {
                await _weatherForecastContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await WeatherForecastExistsAsync(weatherForecast.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteWeatherForecastAsync(int id)
        {
            var weatherForecast = await _weatherForecastContext.WeatherForecasts.FindAsync(id);
            if (weatherForecast == null)
            {
                return false;
            }

            _weatherForecastContext.WeatherForecasts.Remove(weatherForecast);
            await _weatherForecastContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> WeatherForecastExistsAsync(int id)
        {
            return await _weatherForecastContext.WeatherForecasts.AnyAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            return await _userContext.Usuarios.ToListAsync();
        }

        public async Task<UserModel> CreateUserAsync(UserModel user)
        {
            _userContext.Usuarios.Add(user);
            await _userContext.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersAsync()
        {
            return await _userContext.Pedidos.ToListAsync();
        }

        public async Task<OrderModel> CreateOrderAsync(OrderModel order)
        {
            _userContext.Pedidos.Add(order);
            await _userContext.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<UserModel>> GetUsersWithOrdersAsync()
        {
            return await _userContext.Usuarios.Include(user => user.Orders).ToListAsync();
        }
    }
}