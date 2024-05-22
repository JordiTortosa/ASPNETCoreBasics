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
        private readonly IMapper _mapper;

        public WeatherForecastController(IWeatherForecastService weatherForecastService, ILogger<WeatherForecastController> logger, MyService myService, IMapper mapper)
        {
            _weatherForecastService = weatherForecastService;
            _logger = logger;
            _myService = myService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecastDto>>> GetWeatherForecasts()
        {
            var forecasts = await _weatherForecastService.GetWeatherForecastsAsync();
            //System.Diagnostics.Debug.WriteLine("ANTES DEL MAPPING");
            var forecastsDto = _mapper.Map<IEnumerable<WeatherForecastDto>>(forecasts);
            return Ok(forecastsDto);
        }

        [HttpPost]
        public async Task<ActionResult<WeatherForecastDto>> CreateWeatherForecast(WeatherForecastDto weatherForecastDto)
        {
            /*
            {
                "date": "2023-05-22",
                "temperatureC": 25,
                "summary": "Sunny"
            }
            */
            if (ModelState.IsValid)
            {
                var forecast = _mapper.Map<WeatherForecastModel>(weatherForecastDto);
                var createdForecast = await _weatherForecastService.CreateWeatherForecastAsync(forecast);
                var createdForecastDto = _mapper.Map<WeatherForecastDto>(createdForecast);
                return CreatedAtAction(nameof(GetWeatherForecasts), new { id = createdForecastDto.Id }, createdForecastDto);
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
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(usersDto);
        }

        [HttpPost("users")]
        public async Task<ActionResult<UserDto>> CreateUser(UserDto userDto)
        {
            var user = _mapper.Map<UserModel>(userDto);
            var createdUser = await _weatherForecastService.CreateUser(user);
            var createdUserDto = _mapper.Map<UserDto>(createdUser);
            return CreatedAtAction(nameof(GetUsers), new { id = createdUserDto.Id }, createdUserDto);
        }

        [HttpGet("orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _weatherForecastService.GetOrders();
            var ordersDto = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(ordersDto);
        }

        [HttpPost("orders")]
        public async Task<ActionResult<OrderDto>> CreateOrder(OrderDto orderDto)
        {
            var order = _mapper.Map<OrderModel>(orderDto);
            var createdOrder = await _weatherForecastService.CreateOrder(order);
            var createdOrderDto = _mapper.Map<OrderDto>(createdOrder);
            return CreatedAtAction(nameof(GetOrders), new { id = createdOrderDto.Id }, createdOrderDto);
        }

        [HttpGet("users-with-orders")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersWithOrders()
        {
            var usersWithOrders = await _weatherForecastService.GetUsersWithOrders();
            var usersWithOrdersDto = _mapper.Map<IEnumerable<UserDto>>(usersWithOrders);
            return Ok(usersWithOrdersDto);
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
