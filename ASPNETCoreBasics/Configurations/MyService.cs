using Moq;

namespace ASPNETCoreBasics.Configurations
{
    public class MyService
    {
        private readonly IConfiguration _configuration;

        public MyService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetTitle()
        {
            return _configuration["AppSettings:Title"];
        }

        public static implicit operator Mock<object>(MyService v)
        {
            throw new NotImplementedException();
        }
    }
}