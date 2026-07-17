namespace IndurFarmsAPI.Models
{
    public class Payments
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int TransactionId { get; set; }
        public int Amount { get; set; }
        public int Status { get; set; }
    }
}
