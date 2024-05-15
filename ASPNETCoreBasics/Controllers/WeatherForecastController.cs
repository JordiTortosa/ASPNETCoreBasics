using ASPNETCoreBasics.Configurations;
using ASPNETCoreBasics.Contexts.ASPNETCoreBasics.Models;
using ASPNETCoreBasics.Filters;
using ASPNETCoreBasics.Models;
using ASPNETCoreBasics.Validator;
using ASPNETCoreBasics.Validators;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private readonly ApplicationDbContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, MyService myService, ApplicationDbContext context)
        {
            _myService = myService;
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecastModel>>> GetWeatherForecasts()
        {
            return await _context.WeatherForecasts.ToListAsync();
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
                _context.WeatherForecasts.Add(weatherForecast);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetWeatherForecasts), new { id = weatherForecast.Id }, weatherForecast);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /*
        [HttpGet(Name = "GetWeatherForecast")]
        [WeekendFilter]

        public IEnumerable<WeatherForecastModel> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecastModel
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();
        }

        
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
        */
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
