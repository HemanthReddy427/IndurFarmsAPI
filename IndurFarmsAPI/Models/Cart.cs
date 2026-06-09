namespace IndurFarmsAPI.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Users User { get; set; }
        public ICollection<CartItems> Items { get; set; } = new List<CartItems>();
    }
}
