namespace api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Seller { get; set; }
        public Single Rating { get; set; }
        public string? Image { get; set; }
    }
}