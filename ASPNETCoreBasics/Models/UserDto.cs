﻿namespace ASPNETCoreBasics.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<OrderDto> Orders { get; set; }
    }
}
