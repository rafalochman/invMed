using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BarcodeLib;
using invMed.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace invMed.Services
{
    public class WarehousemanService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AspNetUser> _userManager;

        public WarehousemanService(ApplicationDbContext db, UserManager<AspNetUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IEnumerable<string>> Search(string searchValue)
        {
            return await _db.Products.Where(x => x.Name.Contains(searchValue)).Select(x => x.Name).ToArrayAsync();
        }

        public async Task<IEnumerable<string>> GetAllProductsNames()
        {
            return await _db.Products.Select(x => x.Name).ToArrayAsync();
        }

        public async Task<IEnumerable<string>> SearchProductsNames(string searchValue)
        {
            return await _db.Products.Where(x => x.Name.Contains(searchValue)).Select(x => x.Name).ToArrayAsync();
        }

        public async Task<(string barcode, string barcodeUrl)> AddItemAndGetBarcode(AddItemInput input, string userName)
        {
            var product = await _db.Products.FirstOrDefaultAsync(x => x.Name == input.ProductName);
            var user = await _userManager.FindByNameAsync(userName);
            var item = new Item { Place = input.Place, AddDate = DateTime.Now, ExpirationDate = input.ExpirationDate, Product = product, AddUser = user};
            try
            {
                _db.Items.Add(item);
                product.Amount += 1;
                await _db.SaveChangesAsync();
                item.BarCode = item.Id.ToString("D8");
                var barcodeUrl = GenerateBarCode(item.BarCode);
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
            if(item is not null)
            {
                return new RemoveItemView()
                {
                    Id = item.Id,
                    Number = number,
                    BarCode = item.BarCode,
                    ProductName = item.Product.Name
                };
            }
            return new RemoveItemView();
            
        }

        public async Task<bool> RemoveItems(List<RemoveItemView> items)
        {
            try
            {
                foreach(var itemView in items)
                {
                    var item = await _db.Items.FirstOrDefaultAsync(x => x.BarCode == itemView.BarCode);
                    _db.Remove(item);
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<RunOutProductsView>> GetRunOutProducts()
        {
            var products = await _db.Products.Where(x => (x.Amount <= (x.MinAmount * 1.1))).OrderBy(x => x.Category).ToListAsync();
            var runOutProducts = new List<RunOutProductsView>();
            foreach(var product in products)
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
                    runOutProduct.CommunicateType = true;
                }
                else
                {
                    runOutProduct.CommunicateType = false;
                }
                runOutProducts.Add(runOutProduct);
            }
            return runOutProducts;
        }

        public async Task<List<NewItemsView>> GetNewItems()
        {
            var items = await _db.Items.Include(x => x.Product).OrderByDescending(x => x.AddDate).Take(20).ToListAsync();
            var newItems = new List<NewItemsView>();
            foreach(var item in items)
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

        public string GenerateBarCode(string barcodevalue)
        {
            const int Width = 250;
            const int Height = 100;
            var barcode = new Barcode();
            var barcodeImage = barcode.Encode(TYPE.CODE128, barcodevalue, Width, Height);

            var thumbnail = barcodeImage.GetThumbnailImage(Width, Height, () => false, IntPtr.Zero);
            var imageConverter = new ImageConverter();
            var thumbnailBytes = (byte[])imageConverter.ConvertTo(thumbnail, typeof(byte[]));

            var barcodeImageBase64 = Convert.ToBase64String(thumbnailBytes);
            var urlData = string.Format("data:image/jpg;base64, {0}", barcodeImageBase64);
            return urlData;
        }
    }
}
