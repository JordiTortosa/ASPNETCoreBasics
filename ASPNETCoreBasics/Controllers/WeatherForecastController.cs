using ASPNETCoreBasics.Configurations;
using ASPNETCoreBasics.Filters;
using ASPNETCoreBasics.Models;
using ASPNETCoreBasics.Validator;
using ASPNETCoreBasics.Validators;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Mvc;

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

        public WeatherForecastController(ILogger<WeatherForecastController> logger, MyService myService)
        {
            _myService = myService;
            _logger = logger;
        }
        
        [HttpGet(Name = "GetWeatherForecast")]
        [WeekendFilter]
        public IActionResult Get([FromQuery] WeatherForecastModel request)
        {
            System.Diagnostics.Debug.WriteLine("PRE VALIDACIÓN");
            var validationResult = new WeatherForecastRequestValidator().Validate(request);
            System.Diagnostics.Debug.WriteLine("POST VALIDACIÓN");
            if (!validationResult.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("VALIDACIÓN DENEGADA");
                return BadRequest(validationResult.Errors);
            }

            System.Diagnostics.Debug.WriteLine("VALIDACIÓN ACEPTADA");
            return Ok(GenerateWeatherForecasts());
        }

        private IEnumerable<WeatherForecastModel> GenerateWeatherForecasts()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecastModel
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();
        }
        
        [HttpGet("/Test")]
        [WeekendFilter]

        public IActionResult Get([FromQuery] TestModel request)
        {
            System.Diagnostics.Debug.WriteLine("PRE VALIDACIÓN");
            var validationResult = new TestValidator().Validate(request);
            System.Diagnostics.Debug.WriteLine("POST VALIDACIÓN");
            if (!validationResult.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("VALIDACIÓN DENEGADA");
                return BadRequest(validationResult.Errors);
            }

            System.Diagnostics.Debug.WriteLine("VALIDACIÓN ACEPTADA");
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
