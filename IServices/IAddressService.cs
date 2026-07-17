using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.Models;

namespace IndurFarmsAPI.IServices
{
    public interface IAddressService
    {
        Task<ICollection<AddressResponseDto>> GetAddressesAsync(int userId);
        Task<AddressResponseDto?> GetAddressByIdAsync(int id);
        Task<AddressResponseDto> CreateAddressAsync(int userId, CreateAddressDto createAddressDto);
        Task<AddressResponseDto> UpdateAddressAsync(int id,int userId, CreateAddressDto createAddressDto);
        Task DeleteAddressAsync(int id);
    }
}
