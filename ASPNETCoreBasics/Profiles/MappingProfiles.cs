using ASPNETCoreBasics.Models;
using AutoMapper;

namespace ASPNETCoreBasics.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WeatherForecastModel, WeatherForecastDto>();
            CreateMap<WeatherForecastDto, WeatherForecastModel>();

            CreateMap<UserModel, UserDto>();
            CreateMap<UserDto, UserModel>();

            CreateMap<OrderModel, OrderDto>();
            CreateMap<OrderDto, OrderModel>();

        }
    }
}