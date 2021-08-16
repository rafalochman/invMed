using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<bool> AddItem(AddItemInput input, string userName)
        {
            var product = await _db.Products.FirstOrDefaultAsync(x => x.Name == input.ProductName);
            var user = await _userManager.FindByNameAsync(userName);
            var item = new Item { BarCode = input.BarCode, Place = input.Place, AddDate = DateTime.Now, ExpirationDate = input.ExpirationDate, Product = product, AddUser = user};
            try
            {
                _db.Items.Add(item);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
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
            var products = await _db.Products.Where(x => (x.Amount <= (x.MinAmount * 1.1))).ToListAsync();
            var runOutProducts = new List<RunOutProductsView>();
            foreach(var product in products)
            {
                var runOutProduct = new RunOutProductsView();
                runOutProduct.Id = product.Id;
                runOutProduct.Name = product.Name;
                runOutProduct.Category = product.Category;
                runOutProduct.Amount = product.Amount;
                if(product.Amount < product.MinAmount)
                {
                    runOutProduct.CommunicateType = true;
                }
                else
                {
                    runOutProduct.CommunicateType = false;
                }
                runOutProducts.Add(runOutProduct);
            }

            return runOutProducts.OrderBy(x => x.Category).ToList();
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
    }
}
