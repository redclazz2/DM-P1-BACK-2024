using api.Dto;
using api.Models;

namespace api.Interfaces
{
    public interface IProductRepository
    {
        ICollection<Product> GetProducts();   
        ICollection<Product> GetFavoriteProducts(int userId);
        bool CreateFavorite(FavoriteDto favorite);
        bool DeleteFavorite(int userId, int productId);
        bool Save();   
    }
}