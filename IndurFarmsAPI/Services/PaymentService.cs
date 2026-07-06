using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.IRepositories;
using IndurFarmsAPI.IServices;
using Razorpay.Api;

namespace IndurFarmsAPI.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ICartRepository _cartRepository;
        private readonly ICurrentUserService _currentUserService;

        public PaymentService(
            IConfiguration configuration,
            ICartRepository cartRepository,
            ICurrentUserService currentUserService)
        {
            _configuration = configuration;
            _cartRepository = cartRepository;
            _currentUserService = currentUserService;
        }

        public async Task<RazorpayOrderResponseDto> CreatePaymentOrderAsync(int addressId)
        {
            CurrentUserDto currentUser =
                await _currentUserService.GetCurrentUserAsync();

            var cartItems =
                await _cartRepository.GetUserCartItemsAsync(currentUser.UserId);

            if (!cartItems.Any())
            {
                throw new Exception("Cart is empty.");
            }

            decimal totalAmount = cartItems.Sum(item =>
                item.Product.Price * item.Quantity);

            RazorpayClient client = new RazorpayClient(
                _configuration["Razorpay:Key"],
                _configuration["Razorpay:Secret"]);

            Dictionary<string, object> options = new()
            {
                { "amount", (int)(totalAmount * 100) }, // Amount in paise
                { "currency", "INR" },
                { "receipt", Guid.NewGuid().ToString() }
            };

            Razorpay.Api.Order razorpayOrder =
                client.Order.Create(options);

            return new RazorpayOrderResponseDto
            {
                OrderId = razorpayOrder["id"].ToString()!,
                Amount = totalAmount,
                Currency = razorpayOrder["currency"].ToString()!,
                Key = _configuration["Razorpay:Key"]!
            };
        }

        public async Task<PlaceOrderResponseDto> VerifyPaymentAsync(
            VerifyPaymentDto verifyPaymentDto)
        {
            throw new NotImplementedException();
        }
    }
}