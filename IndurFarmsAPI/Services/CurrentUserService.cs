using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.IServices;
using System.Security.Claims;

namespace IndurFarmsAPI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<CurrentUserDto> GetCurrentUserAsync()
        {
            int UserId = int.Parse(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            string RoleId = httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Role)!.Value;
            CurrentUserDto currentUserDto = new CurrentUserDto{
                UserId = UserId,
                Role = RoleId
            };
            return currentUserDto;
        }
    }
}
