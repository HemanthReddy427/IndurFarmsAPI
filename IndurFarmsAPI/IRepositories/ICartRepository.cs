using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.Models;

namespace IndurFarmsAPI.IRepositories
{
    public interface ICartRepository
    {
        Task<ICollection<CartItems>> GetUserCartItemsAsync(int? cartId);
        Task<CartItems> GetItemByIdAsync(int? cartId, int productId);
        Task<CartItems> GetCartItemAsync(int? cartId, int productId);
        Task<int?> GetCartId(int userId);
        Task AddUserToCart(Cart cart);
        Task<CartItems> AddToCartAsync(CartItems cartItem);
        Task<CartItems> UpdateCartItemAsync(CartItems updateItem);
        Task DeleteCartItemAsync(CartItems cartItem);
        Task<CartItems> GetItemsByCartItemsId(int id);
    }
}
