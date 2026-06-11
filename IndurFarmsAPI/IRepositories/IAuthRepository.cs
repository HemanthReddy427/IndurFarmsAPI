using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.Models;

namespace IndurFarmsAPI.IRepositories
{
    public interface IAuthRepository
    {
        Task<ICollection<Users>> GetAllUsersAsync();
        Task<Users> GetByEmailAsync(string email);
        Task<Users> RegisterUserAsync(Users user);
        Task<Users> UpdateUserAsync(Users user);
        Task<bool> DeleteUserAsync(Users user);
    }
}
