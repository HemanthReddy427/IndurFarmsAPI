using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.IRepositories;
using IndurFarmsAPI.IServices;
using IndurFarmsAPI.Models;

namespace IndurFarmsAPI.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _productsRepository;
        public ProductsService(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<ICollection<ProductsDto>> GetAllProductsAsync()
        {
            ICollection<Products> products = await _productsRepository.GetAllProductsAsync();
            return products.Select(p => new ProductsDto
            {
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                ImageUrl = p.ImageURL
            }).ToList();
        }

        public async Task AddProductAsync(ProductsDto productDto)
        {
            if (productDto == null)
            {
                throw new Exception("Product cannot be empty");
            }
            await _productsRepository.AddProductAsync(new Products
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock= productDto.Stock,
                ImageURL = productDto.ImageUrl
            });
        }

        public async Task UpdateProductAsync(int id,ProductsDto productDto)
        {
            Products product = await _productsRepository.GetProductsByIdAsync(id);
            if(product== null)
            {
                throw new Exception("Product not found");
            }
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Stock = productDto.Stock;
            product.ImageURL = productDto.ImageUrl;
            await _productsRepository.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            Products product = await _productsRepository.GetProductsByIdAsync(id);
            if(product== null)
            {
                throw new Exception("Product cannot be null");
            }
            await _productsRepository.DeleteProductAsync(product);
        }

        public async Task<ProductsDto> GetProductByIdAsync(int id)
        {
            Products product = await _productsRepository.GetProductsByIdAsync(id);
            ProductsDto productsDto = new ProductsDto
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageURL
            };
            return productsDto;
        }
    }
}
