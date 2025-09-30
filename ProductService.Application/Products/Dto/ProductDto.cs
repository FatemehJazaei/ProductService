namespace ProductService.Application.Products.Dto
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public bool IsAvailable { get; set; }
        public string ManufactureEmail { get; set; } = default!;
        public string ManufacturePhone { get; set; } = default!;
        public DateOnly ProduceDate { get; set; }
    }
}