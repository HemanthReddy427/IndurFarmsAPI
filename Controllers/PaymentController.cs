using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndurFarmsAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder(
            CreatePaymentOrderRequestDto request)
        {
            var response =
                await _paymentService.CreatePaymentOrderAsync(
                    request.AddressId);

            return Ok(response);
        }

        [HttpPost("Verify")]
        public async Task<IActionResult> Verify(
            VerifyPaymentDto request)
        {
            var response =
                await _paymentService.VerifyPaymentAsync(request);

            return Ok(response);
        }
    }
}