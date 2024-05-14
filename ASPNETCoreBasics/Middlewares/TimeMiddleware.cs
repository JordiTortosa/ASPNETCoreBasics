namespace ASPNETCoreBasics.Middleware
{
    public class TimeMiddleware
    {

        private readonly RequestDelegate _next;

        public TimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestTime = DateTime.Now;
            System.Diagnostics.Debug.WriteLine($"Request recibida a las {requestTime}");
            //Console.WriteLine($"Request recibida a las {requestTime}");
            await _next(context);
        }
    }
}
