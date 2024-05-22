using ASPNETCoreBasics.Configurations;
using ASPNETCoreBasics.Contexts;
using ASPNETCoreBasics.Contexts.ASPNETCoreBasics.Models;
using ASPNETCoreBasics.Filters;
using ASPNETCoreBasics.Models;
using ASPNETCoreBasics.Services;
using ASPNETCoreBasics.Validator;
using ASPNETCoreBasics.Validators;
using AutoMapper;
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
        private readonly IWeatherForecastService _weatherForecastService;
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly MyService _myService;

        public WeatherForecastController(IWeatherForecastService weatherForecastService, ILogger<WeatherForecastController> logger, MyService myService)
        {
            _weatherForecastService = weatherForecastService;
            _logger = logger;
            _myService = myService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecastDto>>> GetWeatherForecasts()
        {
            var forecasts = await _weatherForecastService.GetWeatherForecastsAsync();
            return Ok(forecasts);
        }

        [HttpPost]
        public async Task<ActionResult<WeatherForecastDto>> CreateWeatherForecast(WeatherForecastDto weatherForecastDto)
        {
            if (ModelState.IsValid)
            {
                var createdForecast = await _weatherForecastService.CreateWeatherForecastAsync(weatherForecastDto);
                return CreatedAtAction(nameof(GetWeatherForecasts), new { id = createdForecast.Id }, createdForecast);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWeatherForecast(int id, [FromBody] JsonElement json)
        {
            var result = await _weatherForecastService.UpdateWeatherForecastAsync(id, json);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeatherForecast(int id)
        {
            var result = await _weatherForecastService.DeleteWeatherForecastAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _weatherForecastService.GetUsers();
            return Ok(users);
        }

        [HttpPost("users")]
        public async Task<ActionResult<UserDto>> CreateUser(UserDto userDto)
        {
            var createdUser = await _weatherForecastService.CreateUser(userDto);
            return CreatedAtAction(nameof(GetUsers), new { id = createdUser.Id }, createdUser);
        }

        [HttpGet("orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _weatherForecastService.GetOrders();
            return Ok(orders);
        }

        [HttpPost("orders")]
        public async Task<ActionResult<OrderDto>> CreateOrder(OrderDto orderDto)
        {
            var createdOrder = await _weatherForecastService.CreateOrder(orderDto);
            return CreatedAtAction(nameof(GetOrders), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpGet("users-with-orders")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersWithOrders()
        {
            var usersWithOrders = await _weatherForecastService.GetUsersWithOrders();
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
