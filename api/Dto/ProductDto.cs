namespace api.Dto
{
    public class ProductDto
    {
        public required string Name { get; set; }
        public required string Seller { get; set; }
        public Single Rating { get; set; }
        public string? Image { get; set; }
    }
}