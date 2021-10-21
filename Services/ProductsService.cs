﻿using System;
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
            var items = await _db.Items.Where(x => x.Product.Id == productId).Where(x => x.Type != ItemTypeEnum.Over).ToListAsync();
            var itemsView = new List<ProductItemView>();
            foreach (var item in items)
            {
                var itemView = new ProductItemView
                {
                    Id = item.Id,
                    BarCode = item.BarCode,
                    Place = item.Place.Name
                };
                if (item.ExpirationDate is not null)
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
            if (product is null)
            {
                _logger.LogError("Get product details error - product not found.");
                return new ProductDetailsView();
            }

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
            if (item is null)
            {
                _logger.LogError("Get item details error - item not found.");
                return new ItemDetailsView();
            }

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
            if (item.ExpirationDate is not null)
            {
                itemDetailsView.ExpirationDate = item.ExpirationDate.Value.ToString("dd/MM/yyyy");
            }
            return itemDetailsView;
        }

        public async Task<List<RunOutProductView>> GetRunOutProducts()
        {
            var products = await _db.Products.Where(x => (x.Amount <= (x.MinAmount * 1.1))).OrderBy(x => x.Category).ToListAsync();
            var runOutProducts = new List<RunOutProductView>();
            foreach (var product in products)
            {
                var runOutProduct = new RunOutProductView
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = product.Category,
                    Amount = product.Amount
                };
                if (product.Amount < product.MinAmount)
                {
                    runOutProduct.ComunicateType = RunOutComunicateTypeEnum.Empty;
                }
                else
                {
                    runOutProduct.ComunicateType = RunOutComunicateTypeEnum.Small;
                }
                runOutProducts.Add(runOutProduct);
            }
            return runOutProducts;
        }

        public async Task<List<ExpiredItemView>> GetExpiredItems()
        {
            var items = await _db.Items.Include(x => x.Product).Where(x => x.ExpirationDate > DateTime.Now.AddDays(-30)).Where(x => x.Type != ItemTypeEnum.Over).OrderBy(x => x.ExpirationDate).ToListAsync();
            var expiredItems = new List<ExpiredItemView>();
            foreach (var item in items)
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
                    expiredItemView.ComunicateType = ExpiredComunicateTypeEnum.Expired;
                }
                else
                {
                    expiredItemView.ComunicateType = ExpiredComunicateTypeEnum.Close;
                }
                expiredItems.Add(expiredItemView);
            }
            return expiredItems;
        }
    }
}
