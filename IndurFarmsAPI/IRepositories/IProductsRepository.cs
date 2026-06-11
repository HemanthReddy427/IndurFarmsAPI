using IndurFarmsAPI.Models;

namespace IndurFarmsAPI.IRepositories
{
    public interface IProductsRepository
    {
        Task<ICollection<Products>> GetAllProductsAsync();
        Task<Products> GetProductsByIdAsync(int id);
        Task AddProductAsync(Products product);
        Task UpdateProductAsync(Products product);
        Task DeleteProductAsync(Products product);
    }
}
