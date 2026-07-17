using IndurFarmsAPI.Models;

namespace IndurFarmsAPI.IServices
{
    public interface IJwtService
    {
        string GenerateToken(Users user);
    }
}
