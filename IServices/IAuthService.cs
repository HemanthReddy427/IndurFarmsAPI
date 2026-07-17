using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace IndurFarmsAPI.IServices
{
    public interface IAuthService
    {
        Task<ICollection<RegisterResponseDto>> GetAllUsersAsync();
        Task<RegisterResponseDto> RegisterUserAsync(RegisterDto registerDto);
        Task<RegisterResponseDto> UpdateUserAsync(RegisterDto registerDto);
        Task<bool> DeleteUserAsync(string email);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    }
}
