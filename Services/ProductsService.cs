using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;
using invMed.Data.Enums;
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

        public async Task<List<RunOutProductsView>> GetRunOutProducts()
        {
            var products = await _db.Products.Where(x => (x.Amount <= (x.MinAmount * 1.1))).OrderBy(x => x.Category).ToListAsync();
            var runOutProducts = new List<RunOutProductsView>();
            foreach (var product in products)
            {
                var runOutProduct = new RunOutProductsView
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = product.Category,
                    Amount = product.Amount
                };
                if (product.Amount < product.MinAmount)
                {
                    runOutProduct.ComunicateType = RunOutComunicateType.Empty;
                }
                else
                {
                    runOutProduct.ComunicateType = RunOutComunicateType.Small;
                }
                runOutProducts.Add(runOutProduct);
            }
            return runOutProducts;
        }

        public async Task<List<ExpiredItemView>> GetExpiredItems()
        {
            var items = await _db.Items.Include(x => x.Product).Where(x => x.ExpirationDate > DateTime.Now.AddDays(-30)).OrderBy(x => x.ExpirationDate).ToListAsync();
            var expiredItems = new List<ExpiredItemView>();
            foreach(var item in items)
            {
                var expiredItemView = new ExpiredItemView()
                {
                    Id = item.Id,
                    ProductName = item.Product.Name,
                    ProductCategory = item.Product.Category,
                    BarCode = item.BarCode
                };
                if (item.ExpirationDate is not null)
                {
                    expiredItemView.ExpirationDate = item.ExpirationDate.Value.ToString("dd/MM/yyyy");
                }

                if (item.ExpirationDate > DateTime.Now)
                {
                    expiredItemView.ComunicateType = ExpiredComunicateType.Expired;
                }
                else
                {
                    expiredItemView.ComunicateType = ExpiredComunicateType.Close;
                }
                expiredItems.Add(expiredItemView);
            }
            return expiredItems;
        }
    }
}
