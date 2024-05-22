using ASPNETCoreBasics.Services;

namespace ASPNETCoreBasics.Middleware
{
    public class TimeMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<WeatherForecastService> _logger;

        public TimeMiddleware(RequestDelegate next, ILogger<WeatherForecastService> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestTime = DateTime.Now;
            System.Diagnostics.Debug.WriteLine($"Request recibida a las {requestTime}");
            _logger.LogInformation("Request recibida a las {RequestTime}", requestTime);
            //Console.WriteLine($"Request recibida a las {requestTime}");
            await _next(context);
        }
    }
}
