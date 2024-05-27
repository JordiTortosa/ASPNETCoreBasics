
using System.Data.Entity;
using ASPNETCoreBasics.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCoreBasics.Contexts
{
    
    namespace ASPNETCoreBasics.Models
    {
        public class WeatherForecastDbContext : Microsoft.EntityFrameworkCore.DbContext
        {
            public WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options)
                : base(options)
            {
            }
            public WeatherForecastDbContext() { }
            public Microsoft.EntityFrameworkCore.DbSet<WeatherForecastModel> WeatherForecasts { get; set; }
        }
    }
}
