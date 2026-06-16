using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.IServices;
using IndurFarmsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace IndurFarmsAPI.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) {
            _authService = authService;
        }

        [HttpGet]
        [Route("Users")]
        public async Task<IActionResult> GetAllUsers()
        {
            ICollection<RegisterResponseDto> users = await _authService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginUser(LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("User name or Password cannot be null");
            }
            string token = await _authService.LoginAsync(loginDto);
            return Ok(token);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser(RegisterDto registerDto)
        {
            await _authService.RegisterUserAsync(registerDto);
            return Ok("User Added Successfully");
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateUserAsync(RegisterDto registerDto)
        {
            await _authService.UpdateUserAsync(registerDto);
            return Ok("User Updated Successfully");
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUserAsync(string email)
        {
            await _authService.DeleteUserAsync(email);
            return Ok("User deleted successfully");
        }
    }
}
