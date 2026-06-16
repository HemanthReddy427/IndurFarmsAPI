using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.IRepositories;
using IndurFarmsAPI.IServices;
using IndurFarmsAPI.Models;

namespace IndurFarmsAPI.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<AddressResponseDto> CreateAddressAsync(int userId, CreateAddressDto createAddressDto)
        {
            Address address = new Address
            {
                UserId = userId,
                BasicAddress = createAddressDto.BasicAddress,
                Landmark = createAddressDto.Landmark,
                City = createAddressDto.City,
                District = createAddressDto.District,
                State = createAddressDto.State,
                ZipCode = createAddressDto.ZipCode
            };
            Address addre = await _addressRepository.CreateAddressAsync(address);
            return new AddressResponseDto{
                BasicAddress = addre.BasicAddress,
                Landmark = addre.Landmark,
                City =addre.City,
                District = addre.District,
                State = addre.State,
                ZipCode =addre.ZipCode
            };
        }

        public async Task DeleteAddressAsync(int id)
        {
            Address? address = await _addressRepository.GetAddressByIdAsync(id);
            if(address == null)
            {
                throw new Exception("Address cannot be null");
            }
            await _addressRepository.DeleteAddressAsync(address!);
        }

        public async Task<AddressResponseDto?> GetAddressByIdAsync(int id)
        {
            Address? address = await _addressRepository.GetAddressByIdAsync(id);
            if (address == null)
            {
                throw new Exception("Address not found");
            }
            return new AddressResponseDto
            {
                BasicAddress = address.BasicAddress,
                Landmark = address.Landmark,
                City = address.City,
                District = address.District,
                State = address.State,
                ZipCode =address.ZipCode
            };
        }

        public async Task<ICollection<AddressResponseDto>> GetAddressesAsync(int userId)
        {
            ICollection<Address> addresses = await _addressRepository.GetAllAddressAsync(userId);
            return addresses.Select(address => new AddressResponseDto
            {
                Id = address.Id,
                BasicAddress = address.BasicAddress,
                Landmark = address.Landmark,
                City = address.City,
                District = address.District,
                State = address.State,
                ZipCode = address.ZipCode
            }).ToList();
        }

        public async Task<AddressResponseDto> UpdateAddressAsync(int id,int userId, CreateAddressDto createAddressDto)
        {
            Address? address = await _addressRepository.GetAddressByIdAsync(id);
            if (address == null)
            {
                throw new Exception("Address not found");
            }
            address.UserId = userId;
            address.BasicAddress = createAddressDto.BasicAddress;
            address.Landmark = createAddressDto.Landmark;
            address.City = createAddressDto.City;
            address.District = createAddressDto.District;
            address.State = createAddressDto.State;
            address.ZipCode = createAddressDto.ZipCode;
            Address newAddress = await _addressRepository.UpdateAddressAsync(address);
            return new AddressResponseDto
            {
                BasicAddress = newAddress.BasicAddress,
                Landmark = newAddress.Landmark,
                City = newAddress.City,
                District=newAddress.District,
                State =newAddress.State,
                ZipCode =newAddress.ZipCode
            };
        }
    }
}
