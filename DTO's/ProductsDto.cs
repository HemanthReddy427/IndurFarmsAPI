namespace IndurFarmsAPI.DTO_s
{
    public class ProductsDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public IFormFile? Image { get; set; }
    }
}
