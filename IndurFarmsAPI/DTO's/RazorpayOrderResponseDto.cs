namespace IndurFarmsAPI.DTO_s
{
    public class RazorpayOrderResponseDto
    {
        public string OrderId { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = string.Empty;

        public string Key { get; set; } = string.Empty;
    }
}