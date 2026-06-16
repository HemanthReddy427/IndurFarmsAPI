using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.Models;

namespace IndurFarmsAPI.IRepositories
{
    public interface ICartRepository
    {
        Task<ICollection<CartItems>> GetItemsFromCartAsync();
        Task<CartItems> GetItemByIdAsync(int cartId, int productId);
        Task<int> GetCartId(int userId);
        Task<CartItems> AddToCartAsync(CartItems cartItem);
        Task<CartItems> UpdateCartItemAsync(CartItems updateItem);
        Task DeleteCartItemAsync(CartItems cartItem);
    }
}
