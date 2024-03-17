using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;

namespace api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext _context){
            this._context = _context;
        }

        public ICollection<Product> GetProducts()
        {
            return _context.Products.OrderBy(p => p.Id).ToList();
        }
    }
}