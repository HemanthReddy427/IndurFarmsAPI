using IndurFarmsAPI.DTO_s;

namespace IndurFarmsAPI.IServices
{
    public interface ICurrentUserService
    {
        Task<CurrentUserDto> GetCurrentUserAsync();
    }
}
