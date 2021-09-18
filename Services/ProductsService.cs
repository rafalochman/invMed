using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

        public async Task<List<ProductItemView>> GetItemsByProductId(int productId)
        {
            var items = await _db.Items.Where(x => x.Product.Id == productId).ToListAsync();
            var itemsView = new List<ProductItemView>();
            foreach(var item in items)
            {
                var itemView = new ProductItemView
                {
                    Id = item.Id,
                    BarCode = item.BarCode,
                    Place = item.Place.Name
                };
                if(item.ExpirationDate is not null)
                {
                    itemView.ExpirationDate = item.ExpirationDate.Value.ToString("dd/MM/yyyy");
                }
                itemsView.Add(itemView);
            }
            return itemsView;
        }

        public async Task<ProductDetailsView> GetProductDetailsViewById(int id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
            return new ProductDetailsView
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                Producer = product.Producer,
                Supplier = product.Supplier,
                Amount = product.Amount,
                Price = product.Price,
                MinAmount = product.MinAmount,
                MaxAmount = product.MaxAmount
            };
        }

        public async Task<ItemDetailsView> GetItemDetailsViewById(int id)
        {
            var item = await _db.Items.Include(x => x.Place).FirstOrDefaultAsync(x => x.Id == id);
            var itemDetailsView = new ItemDetailsView
            {
                Id = item.Id,
                BarCode = item.BarCode,
                BarcodeUrl = item.BarcodeUrl,
                Place = item.Place.Name,
                AddDate = item.AddDate.ToString("dd/MM/yyyy"),
                ProductCategory = item.Product.Category,
                ProductName = item.Product.Name
            };
            if(item.ExpirationDate is not null)
            {
                itemDetailsView.ExpirationDate = item.ExpirationDate.Value.ToString("dd/MM/yyyy");
            }
            return itemDetailsView;
        }
    }
}
