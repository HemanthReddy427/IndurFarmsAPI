using Hangfire;
using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.Enums;
using IndurFarmsAPI.IRepositories;
using IndurFarmsAPI.IServices;
using IndurFarmsAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace IndurFarmsAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public AuthService(IAuthRepository authRepository, IJwtService jwtService, IEmailService emailService, IBackgroundJobClient backgroundJobClient)
        {
            _authRepository = authRepository;
            _jwtService = jwtService;
            _emailService = emailService;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<ICollection<RegisterResponseDto>> GetAllUsersAsync()
        {
            ICollection<Users> users = await _authRepository.GetAllUsersAsync();
            ICollection<RegisterResponseDto> registerResponseDto = users.Select(user => new RegisterResponseDto
            {
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone
            }).ToList();
            return registerResponseDto;
        }
        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            Users currentUser = await _authRepository.GetByEmailAsync(loginDto.Email);
            if (currentUser == null)
            {
                throw new Exception("User does not exists");
            }
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(
                                    loginDto.Password,
                                    currentUser.PasswordHash);
            if (!isValidPassword)
            {
                throw new Exception("Invalid Credentials");
            }
            string token = _jwtService.GenerateToken(currentUser);
            return new LoginResponseDto
            {
                Token = token,
                Name = currentUser.Name,
                Email = currentUser.Email,
                Role = ((RolesEnum)currentUser.Role).ToString(),
                UserId = currentUser.Id
            };
        }
        public async Task<RegisterResponseDto> RegisterUserAsync(RegisterDto registerDto)
        {
            Users existingUser = await _authRepository.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                throw new Exception("User Already Exists");
            }
            await _authRepository.RegisterUserAsync(new Users
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Role = (int)RolesEnum.Customer,
                Phone = registerDto.Phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                IsEmailVerified = false,
                IsPhoneVerified = false
            });
            _backgroundJobClient.Enqueue<IEmailService>(x => x.SendWelcomeEmailAsync(registerDto.Email, registerDto.Name));
            return new RegisterResponseDto
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Phone = registerDto.Phone
            };
        }

        public async Task<RegisterResponseDto> UpdateUserAsync(RegisterDto registerDto)
        {
            Users users = await _authRepository.GetByEmailAsync(registerDto.Email);
            if (users == null)
            {
                throw new Exception("User does not exists");
            }
            users.Name = registerDto.Name;
            users.Email = registerDto.Email;
            users.Phone = registerDto.Phone;
            await _authRepository.UpdateUserAsync(users);
            return new RegisterResponseDto
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Phone = registerDto.Phone
            };
        }

        public async Task<bool> DeleteUserAsync(string email)
        {
            Users currentUser = await _authRepository.GetByEmailAsync(email);
            if (currentUser == null)
            {
                throw new Exception("User does not exists");
            }
            await _authRepository.DeleteUserAsync(currentUser);
            return true;
        }
    }
}
