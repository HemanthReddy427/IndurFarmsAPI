using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndurFarmsAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly ICurrentUserService _currentUserService;
        public AddressController(IAddressService addressService, ICurrentUserService currentUserService)
        {
            _addressService = addressService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAdressesAsync()
        {

            CurrentUserDto currentUserDto = await _currentUserService.GetCurrentUserAsync();
            ICollection<AddressResponseDto> addressResponseDto = await _addressService.GetAddressesAsync(currentUserDto.UserId);
            return Ok(addressResponseDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressByIdAsync(int id)
        {
            AddressResponseDto? address = await _addressService.GetAddressByIdAsync(id);
            return Ok(address);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAddressAsync(CreateAddressDto createAddressDto)
        {
            CurrentUserDto currentUser = await _currentUserService.GetCurrentUserAsync();
            AddressResponseDto address = await _addressService.CreateAddressAsync(currentUser.UserId, createAddressDto);
            return Ok(address);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddressAsync(int id,CreateAddressDto createAddressDto)
        {
            CurrentUserDto currentUser = await _currentUserService.GetCurrentUserAsync();
            AddressResponseDto addressResponseDto = await _addressService.UpdateAddressAsync(id, currentUser.UserId, createAddressDto);
            return Ok(addressResponseDto);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteAddressAsync(int id)
        {
            await _addressService.DeleteAddressAsync(id);
            return Ok("Address deleted succefully");
        }
    }
}
