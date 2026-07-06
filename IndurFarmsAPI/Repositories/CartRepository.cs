using IndurFarmsAPI.IRepositories;
using IndurFarmsAPI.Middleware;
using IndurFarmsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IndurFarmsAPI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IndurFarmsDbContext _indurFarmsDbContext;
        public CartRepository(IndurFarmsDbContext indurFarmsDbContext)
        {
            _indurFarmsDbContext = indurFarmsDbContext;
        }
        public async Task<CartItems> AddToCartAsync(CartItems cartItem)
        {
            await _indurFarmsDbContext.AddAsync(cartItem);
            await _indurFarmsDbContext.SaveChangesAsync();
            return cartItem;
        }

        public async Task DeleteCartItemAsync(CartItems cartItem)
        {
            _indurFarmsDbContext.Remove(cartItem);
            await _indurFarmsDbContext.SaveChangesAsync();
        }

        public async Task<CartItems> GetItemByIdAsync(int? cartId, int productId)
        {
            CartItems? cartItem = await _indurFarmsDbContext.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
            return cartItem;
        }

        public async Task<CartItems> GetItemsByCartItemsId(int id)
        {
            CartItems? cartItem = await _indurFarmsDbContext.CartItems.FirstOrDefaultAsync(c=>c.Id == id);
            return cartItem;
        }

        public async Task<CartItems> GetCartItemAsync(int? userId, int productId)
        {
            CartItems? cartItem = await _indurFarmsDbContext.CartItems
                                                .Include(ci=>ci.Cart)
                                                .FirstOrDefaultAsync(ci=>ci.Cart.UserId == userId && ci.ProductId == productId);
            return cartItem;
        }

        public async Task<int?> GetCartId(int userId)
        {
            Cart? cart = await _indurFarmsDbContext.Carts.FirstOrDefaultAsync(ci => ci.UserId == userId);
            return cart?.Id;
        }

        public async Task AddUserToCart(Cart cart)
        {
            await _indurFarmsDbContext.AddAsync(cart);
            await _indurFarmsDbContext.SaveChangesAsync();
        }

        public async Task<ICollection<CartItems>> GetUserCartItemsAsync(int? userId)
        {
            ICollection<CartItems> cartItems = await _indurFarmsDbContext.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.Cart)
                .Where(ci => ci.Cart.UserId == userId)
                .ToListAsync();
            return cartItems;
        }

        public async Task<CartItems> UpdateCartItemAsync(CartItems updateItem)
        {
            _indurFarmsDbContext.CartItems.Update(updateItem);
            await _indurFarmsDbContext.SaveChangesAsync();
            return updateItem;
        }
    }
}
