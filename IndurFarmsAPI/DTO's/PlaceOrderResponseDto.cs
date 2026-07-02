namespace IndurFarmsAPI.DTO_s
{
    public class PlaceOrderResponseDto
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public string PaymentStatus {  get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
    }
}
