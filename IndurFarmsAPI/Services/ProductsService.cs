using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.IRepositories;
using IndurFarmsAPI.IServices;
using IndurFarmsAPI.Models;

namespace IndurFarmsAPI.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        public ProductsService(IProductsRepository productsRepository, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _productsRepository = productsRepository;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public async Task<ICollection<ProductsResponseDto>> GetAllProductsAsync()
        {
            ICollection<Products> products = await _productsRepository.GetAllProductsAsync();
            return products.Select(p => new ProductsResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                ImageUrl = $"{_configuration["Api:BaseUrl"]}{p.ImageURL}"
            }).ToList();
        }

        public async Task AddProductAsync(ProductsDto productDto)
        {
            if (productDto == null)
            {
                throw new Exception("Product cannot be empty");
            }
            string imagePath = await SaveImageAsync(productDto.Image);
            await _productsRepository.AddProductAsync(new Products
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock= productDto.Stock,
                ImageURL = imagePath
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
            //product.ImageURL = productDto.ImageUrl;
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

        public async Task<ProductsResponseDto> GetProductByIdAsync(int id)
        {
            Products product = await _productsRepository.GetProductsByIdAsync(id);
            ProductsResponseDto productsDto = new ProductsResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = $"{_configuration["Api:BaseUrl"]}{product.ImageURL}"
            };
            return productsDto;
        }

        private async Task<string> SaveImageAsync(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                throw new Exception("Please select an image.");
            }

            string extension = Path.GetExtension(image.FileName);

            string[] allowedExtensions =
            {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

            if (!allowedExtensions.Contains(extension.ToLower()))
            {
                throw new Exception("Only JPG, JPEG, PNG and WEBP images are allowed.");
            }

            string fileName = $"{Guid.NewGuid()}{extension}";

            string folderPath = Path.Combine(
                _webHostEnvironment.WebRootPath,
                "images",
                "products"
            );

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, fileName);

            using FileStream stream = new FileStream(filePath, FileMode.Create);

            await image.CopyToAsync(stream);

            return $"/images/products/{fileName}";
        }
    }
}
