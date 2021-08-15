using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;
using Microsoft.EntityFrameworkCore;

namespace invMed.Services
{
    public class ProductsService
    {
        private readonly ApplicationDbContext _db;

        public ProductsService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _db.Products.OrderBy(x => x.Category).ToListAsync();
        }

        public async Task<IEnumerable<string>> Search(string searchValue)
        {
            return await _db.Products.Where(x => x.Name.Contains(searchValue)).Select(x => x.Name).ToArrayAsync();
        }

        public async Task<bool> AddProduct(AddProductInput input)
        {
            var product = new Product() { Name = input.Name, Category = input.Category, Amount = 0, Producer = input.Producer, Supplier = input.Supplier, Price = input.Price, MinAmount = input.MinAmount, MaxAmount = input.MaxAmount };
            try
            {
                await _db.Products.AddAsync(product);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
