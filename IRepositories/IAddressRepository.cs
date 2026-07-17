using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.Models;

namespace IndurFarmsAPI.IRepositories
{
    public interface IAddressRepository
    {
        Task<ICollection<Address>> GetAllAddressAsync(int userId);
        Task<Address?> GetAddressByIdAsync(int id);
        Task<Address> CreateAddressAsync(Address address);
        Task<Address> UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(Address address);
    }
}
