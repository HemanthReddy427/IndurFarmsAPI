using IndurFarmsAPI.DTO_s;

namespace IndurFarmsAPI.IServices
{
    public interface ICartService
    {
        Task<ICollection<GetCartItemsDto>> GetUserCartItems();
        Task<GetCartItemsDto> AddToCartAsync(AddToCartDto addToCartDto);
        Task<GetCartItemsDto> UpdateCartProductAsync(GetCartItemsDto getCartItemsDto);
        Task DeleteProductAsync(int id);
    }
}
