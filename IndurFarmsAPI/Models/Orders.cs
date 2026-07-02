namespace IndurFarmsAPI.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public decimal TotalAmount { get; set; }
        public int PaymentStatus { get; set; }
        public int OrderStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public Users User { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
    }
}
