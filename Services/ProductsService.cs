using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;
using invMed.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace invMed.Services
{
    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<IProductsService> _logger;

        public ProductsService(ApplicationDbContext db, ILogger<IProductsService> logger)
        {
            _db = db;
            _logger = logger;
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add product error.");
                return false;
            }
        }

        public async Task<List<ProductItemView>> GetItemsByProductId(int productId)
        {
            var items = await _db.Items.Include(x => x.Place).Where(x => x.Product.Id == productId && x.Type != ItemTypeEnum.Over).ToListAsync();
            var itemsView = new List<ProductItemView>();
            foreach (var item in items)
            {
                itemsView.Add(new ProductItemView(item));
            }
            return itemsView;
        }

        public async Task<ProductDetailsView> GetProductDetailsViewById(int id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product is null)
            {
                _logger.LogError($"Get product details error - product {id} not found.");
                return new ProductDetailsView();
            }

            return new ProductDetailsView(product);
        }

        public async Task<ItemDetailsView> GetItemDetailsViewById(int id)
        {
            var item = await _db.Items.Include(x => x.Place).FirstOrDefaultAsync(x => x.Id == id);
            if (item is null)
            {
                _logger.LogError($"Get item details error - item {id} not found.");
                return new ItemDetailsView();
            }
            
            return new ItemDetailsView(item);
        }

        public async Task<List<RunOutProductView>> GetRunOutProducts()
        {
            var products = await _db.Products.Where(x => (x.Amount <= (x.MinAmount * 1.1))).OrderBy(x => x.Category).ToListAsync();
            var runOutProducts = new List<RunOutProductView>();
            foreach (var product in products)
            {
                runOutProducts.Add(new RunOutProductView(product));
            }
            return runOutProducts;
        }

        public async Task<List<ExpiredItemView>> GetExpiredItems()
        {
            var items = await _db.Items.Include(x => x.Product).Where(x => x.ExpirationDate < DateTime.Now.AddDays(30) && x.Type != ItemTypeEnum.Over).OrderBy(x => x.ExpirationDate).ToListAsync();
            var expiredItems = new List<ExpiredItemView>();
            foreach (var item in items)
            {
                expiredItems.Add(new ExpiredItemView(item));
            }
            return expiredItems;
        }
    }
}
