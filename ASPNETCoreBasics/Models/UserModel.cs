namespace ASPNETCoreBasics.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<OrderModel> Orders { get; set; }
    }
    public class OrderModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
