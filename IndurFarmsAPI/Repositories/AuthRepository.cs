using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.IRepositories;
using IndurFarmsAPI.Middleware;
using IndurFarmsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IndurFarmsAPI.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IndurFarmsDbContext _indurFarmsDbContext;
        public AuthRepository(IndurFarmsDbContext indurFarmsDbContext)
        {
            _indurFarmsDbContext = indurFarmsDbContext;
        }

        public async Task<ICollection<Users>> GetAllUsersAsync()
        {
            return await _indurFarmsDbContext.Users.ToListAsync();
        }
        public async Task<Users?> GetByEmailAsync(string email)
        {
            Users? user = await _indurFarmsDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user!;
            
        }

        public async Task<Users> RegisterUserAsync(Users user)
        {
            await _indurFarmsDbContext.Users.AddAsync(user);
            await _indurFarmsDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<Users> UpdateUserAsync(Users user)
        {
            _indurFarmsDbContext.Update(user);
            await _indurFarmsDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(Users user)
        {
            _indurFarmsDbContext.Users.Remove(user);
            await _indurFarmsDbContext.SaveChangesAsync();
            return true;
        }
    }
}
