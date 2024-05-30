﻿using ASPNETCoreBasics.Models;

namespace ASPNETCoreBasicsTests.Mocks
{
    public static class DtoMockCreator
    {
        public static WeatherForecastDto CreateWeatherForecastDTO(DateOnly date, int temperatureC, string summary) => new()
        {
            Date = date,
            TemperatureC = temperatureC,
            Summary = summary
        };

        public static UserDto CreateUserDTO(string name, List<OrderDto> orders) => new()
        {
            Name = name,
            Orders = orders,
        };

        public static OrderDto CreateOrderDTO(string description) => new()
        {
            Description = description,
        };
    }
}