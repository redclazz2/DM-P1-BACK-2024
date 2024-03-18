namespace api.Models
{
    public class FavoriteProduct
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public required Product Product { get; set; }
        public required User User { get; set; }
    }
}