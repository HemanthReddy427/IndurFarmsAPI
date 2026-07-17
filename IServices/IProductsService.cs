using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.Models;

namespace IndurFarmsAPI.IServices
{
    public interface IProductsService
    {
        Task<ICollection<ProductsResponseDto>> GetAllProductsAsync();
        Task<ProductsResponseDto> GetProductByIdAsync(int id);
        Task AddProductAsync(ProductsDto productsDto);
        Task UpdateProductAsync(int id,ProductsDto productDto);
        Task DeleteProductAsync(int id);
    }
}
