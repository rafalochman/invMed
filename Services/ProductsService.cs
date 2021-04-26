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
            List<Product> products = _db.Products.ToList();
            return Task.FromResult(products);
        }

        public Task<Product> GetProductById(int id)
        {
            Product product = _db.Products.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(product);
        }

        public void AddProduct(Product product)
        {
            _db.Products.Add(product);
            _db.SaveChanges();
        }

        public void EditProduct(Product product)
        {
            _db.Products.Update(product);
            _db.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            _db.Products.Remove(product);
            _db.SaveChanges();
        }
    }
}
