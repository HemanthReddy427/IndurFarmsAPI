using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndurFarmsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;
        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            ICollection<ProductsResponseDto> products = await _productsService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            ProductsResponseDto product = await _productsService.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        [Route("Add")]
        public async Task<IActionResult> AddProductAsync([FromForm]ProductsDto product)
        {
            await _productsService.AddProductAsync(product);
            return Ok("Product Added Successfully");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, ProductsDto productDto)
        {
            await _productsService.UpdateProductAsync(id, productDto);
            return Ok("Product Updated Successfully");
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            await _productsService.DeleteProductAsync(id);
            return Ok("Product Deleted Successfully");
        }
    }
}
