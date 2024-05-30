using ASPNETCoreBasics.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCoreBasics.Contexts
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public UserContext()
        { }

        public DbSet<UserModel> Usuarios { get; set; }
        public DbSet<OrderModel> Pedidos { get; set; }
    }
}