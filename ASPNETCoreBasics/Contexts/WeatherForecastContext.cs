
using System.Data.Entity;
using ASPNETCoreBasics.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCoreBasics.Contexts
{
    
    namespace ASPNETCoreBasics.Models
    {
        public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            public Microsoft.EntityFrameworkCore.DbSet<WeatherForecastModel> WeatherForecasts { get; set; }
        }
    }
}
