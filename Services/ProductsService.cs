using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;

namespace invMed.Services
{
    public class ProductsService
    {
        private readonly ApplicationDbContext _db;

        public ProductsService(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<List<Product>> GetProducts()
        {
            return Task.FromResult(_db.Products.ToList());
        }

        public void AddProduct(Product product)
        {
            _db.Products.Add(product);
            _db.SaveChanges();
        }
    }
}
