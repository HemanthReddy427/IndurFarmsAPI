using IndurFarmsAPI.DTO_s;

namespace IndurFarmsAPI.IServices
{
    public interface IPaymentService
    {
        Task<RazorpayOrderResponseDto> CreatePaymentOrderAsync(int addressId);

        Task<PlaceOrderResponseDto> VerifyPaymentAsync(
            VerifyPaymentDto verifyPaymentDto);
    }
}