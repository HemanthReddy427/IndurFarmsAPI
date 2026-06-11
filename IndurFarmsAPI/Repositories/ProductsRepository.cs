using IndurFarmsAPI.IRepositories;
using IndurFarmsAPI.Middleware;
using IndurFarmsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IndurFarmsAPI.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly IndurFarmsDbContext _indurFarmsDbContext;
        public ProductsRepository(IndurFarmsDbContext indurFarmsDbContext)
        {
            _indurFarmsDbContext = indurFarmsDbContext;
        }

        public async Task<ICollection<Products>> GetAllProductsAsync()
        {
            return await _indurFarmsDbContext.Products.ToListAsync();
        }

        public async Task<Products> GetProductsByIdAsync(int id)
        {
            return await _indurFarmsDbContext.Products.SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddProductAsync(Products product)
        {
            await _indurFarmsDbContext.Products.AddAsync(product);
            await _indurFarmsDbContext.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Products product)
        {
            _indurFarmsDbContext.Update(product);
            await _indurFarmsDbContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Products product)
        {
            _indurFarmsDbContext.Remove(product);
            await _indurFarmsDbContext.SaveChangesAsync();
        }
    }
}
