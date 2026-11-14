using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.IServices;
using IndurFarmsAPI.Middleware;
using IndurFarmsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IndurFarmsAPI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IndurFarmsDbContext _dbContext;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            IndurFarmsDbContext dbContext)
        {
            this.httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public async Task<CurrentUserDto> GetCurrentUserAsync()
        {
            var userClaims = httpContextAccessor.HttpContext?.User;
            if (userClaims == null || userClaims.Identity?.IsAuthenticated != true)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            // 1. Extract email from either your local JWT or Microsoft Entra ID token
            string? email = userClaims.FindFirstValue(ClaimTypes.Email)
                         ?? userClaims.FindFirstValue("preferred_username")
                         ?? userClaims.FindFirstValue("email")
                         ?? userClaims.FindFirstValue("upn");

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new UnauthorizedAccessException("Email claim not found in token.");
            }

            // 2. Look up user in Azure SQL by email
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            // 3. Auto-create a SQL user record if this is a first-time Microsoft login
            if (user == null)
            {
                string displayName = userClaims.FindFirstValue("name")
                                  ?? userClaims.FindFirstValue(ClaimTypes.Name)
                                  ?? email.Split('@')[0];

                user = new Users
                {
                    Email = email,
                    Name = displayName, // Change to FullName if your User model uses FullName
                    PasswordHash = "MICROSOFT_SSO_USER", // Satisfies non-nullable password column
                    Role = 2, // Change to RoleId = 2 if your User model uses an integer RoleId
                    Phone = "null", // Optional: Set to null or a default value
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
            }

            // 4. Return UserId and Role just like your original implementation
            string role = userClaims.FindFirstValue(ClaimTypes.Role)
                       ?? user.Role.ToString()
                       ?? "User";

            return new CurrentUserDto
            {
                UserId = user.Id, // Change to user.UserId if your primary key is named UserId
                Role = role
            };
        }
    }
}
