using ASPNETCoreBasics.Configurations;
using ASPNETCoreBasics.Contexts;
using ASPNETCoreBasics.Contexts.ASPNETCoreBasics.Models;
using ASPNETCoreBasics.Filters;
using ASPNETCoreBasics.Models;
using ASPNETCoreBasics.Validator;
using ASPNETCoreBasics.Validators;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ASPNETCoreBasics.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly MyService _myService;
        private readonly WeatherForecastDbContext _weatherForecastContext;
        private readonly UserContext _userContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, MyService myService, WeatherForecastDbContext weatherForecastContext, UserContext userContext)
        {
            _myService = myService;
            _logger = logger;
            _weatherForecastContext = weatherForecastContext;
            _userContext = userContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecastModel>>> GetWeatherForecasts()
        {
            return await _weatherForecastContext.WeatherForecasts.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<WeatherForecastModel>> CreateWeatherForecast(WeatherForecastModel weatherForecast)
        {
            /*
            {"date": "2024-05-20",
            "temperatureC": 25,
            "summary": "Sunny"}
             */
            if (ModelState.IsValid)
            {
                _weatherForecastContext.WeatherForecasts.Add(weatherForecast);
                await _weatherForecastContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetWeatherForecasts), new { id = weatherForecast.Id }, weatherForecast);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWeatherForecast(int id, [FromBody] JsonElement json)
        {
            /*
            {"date": "2024-05-20",
            "temperatureC": 25,
            "summary": "Sunny"}
             */
            var weatherForecast = await _weatherForecastContext.WeatherForecasts.FindAsync(id);
            if (weatherForecast == null)
            {
                return NotFound();
            }

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
                        return BadRequest("Invalid date format.");
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
            try
            {
                await _weatherForecastContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeatherForecastExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeatherForecast(int id)
        {
            var weatherForecast = await _weatherForecastContext.WeatherForecasts.FindAsync(id);
            if (weatherForecast == null)
            {
                return NotFound();
            }
            _weatherForecastContext.WeatherForecasts.Remove(weatherForecast);
            await _weatherForecastContext.SaveChangesAsync();
            return NoContent();
        }

        private bool WeatherForecastExists(int id)
        {
            return _weatherForecastContext.WeatherForecasts.Any(e => e.Id == id);
        }

        [HttpGet("users")]
        public ActionResult<IEnumerable<UserModel>> GetUsers()
        {
            var users = _userContext.Usuarios.ToList();
            return Ok(users);
        }

        [HttpPost("users")]
        public ActionResult<UserModel> CreateUser(UserModel user)
        {
            _userContext.Usuarios.Add(user);
            _userContext.SaveChanges();
            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
        }

        [HttpGet("orders")]
        public ActionResult<IEnumerable<OrderModel>> GetOrders()
        {
            var orders = _userContext.Pedidos.ToList();
            return Ok(orders);
        }

        [HttpPost("orders")]
        public ActionResult<OrderModel> CreateOrder(OrderModel order)
        {
            _userContext.Pedidos.Add(order);
            _userContext.SaveChanges();
            return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
        }

        [HttpGet("users-with-orders")]
        public ActionResult<IEnumerable<UserModel>> GetUsersWithOrders()
        {
            var usersWithOrders = _userContext.Usuarios
                .Include(user => user.Orders)
                .ToList();

            return Ok(usersWithOrders);
        }

        [HttpGet("/Test")]
        [WeekendFilter]

        public IActionResult Get([FromQuery] TestModel request)
        {
            var validationResult = new TestValidator().Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            return Ok(PrintTest());
        }
        private string PrintTest()
        {
            string title = _myService.GetTitle();
            string response = $"{title}\n¡Hola desde la API!";
            return response;
        }
    }
 }
