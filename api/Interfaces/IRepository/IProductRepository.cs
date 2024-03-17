using api.Models;

namespace api.Interfaces
{
    public interface IProductRepository
    {
        ICollection<Product> GetProducts();      
    }
}