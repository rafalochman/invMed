using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;
using invMed.Data.Domain;
using invMed.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace invMed.Services
{
    public class WarehousemanService : IWarehousemanService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly ILogger<IWarehousemanService> _logger;

        public WarehousemanService(ApplicationDbContext db, UserManager<AspNetUser> userManager, ILogger<IWarehousemanService> logger)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> GetAllProductsNames()
        {
            return await _db.Products.Select(x => x.Name).ToArrayAsync();
        }

        public async Task<IEnumerable<string>> SearchProductsNames(string searchValue)
        {
            return await _db.Products.Where(x => x.Name.Contains(searchValue)).Select(x => x.Name).ToArrayAsync();
        }

        public async Task<IEnumerable<string>> GetAllPlacesNames()
        {
            return await _db.Places.Select(x => x.Name).ToArrayAsync();
        }

        public async Task<IEnumerable<string>> SearchPlacesNames(string searchValue)
        {
            return await _db.Places.Where(x => x.Name.Contains(searchValue)).Select(x => x.Name).ToArrayAsync();
        }

        public async Task<(string barcode, string barcodeUrl)> AddItemAndGetBarcode(AddItemInput input, string userName)
        {
            var product = await _db.Products.FirstOrDefaultAsync(x => x.Name == input.ProductName);
            var user = await _userManager.FindByNameAsync(userName);
            var place = await _db.Places.FirstOrDefaultAsync(x => x.Name == input.Place);
            var item = new Item { Place = place, AddDate = DateTime.Now, ExpirationDate = input.ExpirationDate, Product = product, AddUser = user, Type = ItemTypeEnum.Regular };
            try
            {
                _db.Items.Add(item);
                product.Amount += 1;
                await _db.SaveChangesAsync();
                item.BarCode = item.Id.ToString("D8");
                var barcode = new Barcode(item.BarCode);
                var barcodeUrl = barcode.UrlData;
                item.BarcodeUrl = barcodeUrl;
                await _db.SaveChangesAsync();
                return (barcode: item.BarCode, barcodeUrl: barcodeUrl);
            }
            catch
            {
                return ("", "");
            }
        }

        public async Task<RemoveItemView> GetRemoveItemViewByItemBarCode(string barCode, int number)
        {
            var item = await _db.Items.FirstOrDefaultAsync(x => x.BarCode == barCode);
            if (item is not null)
            {
                return new RemoveItemView()
                {
                    Id = item.Id,
                    Number = number,
                    BarCode = item.BarCode,
                    ProductName = item.Product.Name,
                    ProductId = item.Product.Id
                };
            }
            return new RemoveItemView();
        }

        public async Task<bool> RemoveItems(List<RemoveItemView> items)
        {
            var products = new List<Product>();
            try
            {
                foreach (var itemView in items)
                {
                    var item = await _db.Items.Include(x => x.InventoryItems).FirstOrDefaultAsync(x => x.BarCode == itemView.BarCode);
                    var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == itemView.ProductId);
                    var notifications = await _db.Notifications.Where(x => x.Item.Id == item.Id).ToListAsync();
                    product.Amount -= 1;
                    _db.Remove(item);
                    _db.RemoveRange(notifications);
                    products.Add(product);
                }
                await _db.SaveChangesAsync();

                await TryCreateSmallAmountNotifications(products);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<NewItemsView>> GetNewItems()
        {
            var items = await _db.Items.Include(x => x.Product).Where(x => x.Type != ItemTypeEnum.Over).OrderByDescending(x => x.AddDate).Take(20).ToListAsync();
            var newItems = new List<NewItemsView>();
            foreach (var item in items)
            {
                var newItem = new NewItemsView
                {
                    Id = item.Id,
                    Name = item.Product.Name,
                    Category = item.Product.Category,
                    BarCode = item.BarCode,
                    AddDate = item.AddDate
                };
                newItems.Add(newItem);
            }
            return newItems;
        }

        private async Task TryCreateSmallAmountNotifications(List<Product> products)
        {
            products = products.Where(x => x.Amount <= (x.MinAmount * 1.1)).ToList();

            foreach (var product in products)
            {
                var notification = new Notification()
                {
                    CreationDate = DateTime.Now,
                    IsNew = true,
                    Product = product,
                    Type = NotificationTypeEnum.SmallAmount
                };
                _db.Notifications.Add(notification);
            }
            try
            {
                await _db.SaveChangesAsync();
                if(products.Count > 0)
                {
                    RefreshService.CallRequestRefresh();
                }
            }
            catch
            {

            }
        }
    }
}
