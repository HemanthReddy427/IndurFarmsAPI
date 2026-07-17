using IndurFarmsAPI.IRepositories;
using IndurFarmsAPI.Middleware;
using IndurFarmsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IndurFarmsAPI.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly IndurFarmsDbContext _indurFarmsDbContext;
        public AddressRepository(IndurFarmsDbContext indurFarmsDbContext)
        {
            _indurFarmsDbContext = indurFarmsDbContext;
        }

        public async Task<Address> CreateAddressAsync(Address address)
        {
            await _indurFarmsDbContext.Addresses.AddAsync(address);
            await _indurFarmsDbContext.SaveChangesAsync();
            return address;
        }

        public async Task DeleteAddressAsync(Address address)
        {
            _indurFarmsDbContext.Remove(address);
            await _indurFarmsDbContext.SaveChangesAsync();
        }

        public async Task<ICollection<Address>> GetAllAddressAsync(int userId)
        {
            ICollection<Address> addresses = await _indurFarmsDbContext.Addresses.Where(add=>add.UserId == userId).ToListAsync();
            return addresses;
        }

        public async Task<Address?> GetAddressByIdAsync(int id)
        {
            Address? address1 = await _indurFarmsDbContext.Addresses.FirstOrDefaultAsync(add => add.Id == id);
            return address1;
        }

        public async Task<Address> UpdateAddressAsync(Address address)
        {
            _indurFarmsDbContext.Addresses.Update(address);
            await _indurFarmsDbContext.SaveChangesAsync();
            return address;
        }
    }
}
