using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dto;
using api.Interfaces;
using api.Models;

namespace api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext _context)
        {
            this._context = _context;
        }

        public bool CreateFavorite(FavoriteDto favorite)
        {
            var user = _context.Users.Where(u => u.Id == favorite.UserId).FirstOrDefault();
            var product = _context.Products.Where(p => p.Id == favorite.ProductId).FirstOrDefault();

            if (user == null || product == null)
                return false;

            _context.Add(new FavoriteProduct()
            {
                UserId = favorite.UserId,
                ProductId = favorite.ProductId,
                Product = product,
                User = user
            });
            return Save();
        }

        public bool DeleteFavorite(int userId, int productId)
        {
            var favorite = _context.FavoriteProducts.Where(
                f => f.ProductId == productId && f.UserId == userId).FirstOrDefault();

            if(favorite == null)
                return false;
            
            _context.Remove(favorite);
            return Save();
        }

        public ICollection<Product> GetFavoriteProducts(int userId)
        {
            return _context.FavoriteProducts.Where(f => f.UserId == userId)
                .Select(p => p.Product).ToList();
        }

        public ICollection<Product> GetProducts()
        {
            return _context.Products.OrderBy(p => p.Id).ToList();
        }

        public bool Save()
        {
            var _saved = _context.SaveChanges();
            return _saved > 0 ? true : false;
        }
    }
}