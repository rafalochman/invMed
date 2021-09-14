using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;
using invMed.Data.Enums;
using invMed.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace invMed.Services
{
    public class InventoryService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AspNetUser> _userManager;

        public InventoryService(ApplicationDbContext db, UserManager<AspNetUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<bool> CreateInventory(CreateInventoryInput input, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var places = new List<Place>();
            foreach(var placeName in input.Places)
            {
                places.Add(await _db.Places.FirstOrDefaultAsync(x => x.Name == placeName));
            }
            var inventory = new Inventory { Type = input.Type, State = InventoryState.Inactive, Description = input.Description, PlannedStartDate = input.PlannedStartDate, PlannedEndDate = input.PlannedEndDate, CreateUser = user, Places = places };
            try
            {
                _db.Inventories.Add(inventory);
                await _db.SaveChangesAsync();
                return (true);
            }
            catch
            {
                return (false);
            }
        }

        public async Task<List<InventoryView>> GetInactiveAndActiveInventories()
        {
            var inventories = await _db.Inventories.Where(x => x.State == InventoryState.Inactive || x.State == InventoryState.Active).ToListAsync();
            var inventoriesView = new List<InventoryView>();
            foreach(var inventory in inventories)
            {
                var inventoryView = new InventoryView()
                {
                    Id = inventory.Id,
                    State = inventory.State,
                    Type = inventory.Type,
                    Description = inventory.Description,
                };
                if(inventory.PlannedStartDate is not null)
                {
                    inventoryView.PlannedStartDate = inventory.PlannedStartDate.Value.ToString("dd/MM/yyyy");
                }
                if(inventory.PlannedEndDate is not null)
                {
                    inventoryView.PlannedEndDate = inventory.PlannedEndDate.Value.ToString("dd/MM/yyyy");
                }
                if(inventory.Type == InventoryType.Full)
                {
                    inventory.InventoryItemsNumber = await _db.Items.CountAsync();
                }
                inventoriesView.Add(inventoryView);
            }
            return inventoriesView;
        }

        public async Task<InventoryDetailsView> GetInventoryDetailsViewById(int id)
        {
            var inventory = await _db.Inventories.Include(x => x.InventoryItems).FirstOrDefaultAsync(x => x.Id == id);
            var inventoryView = new InventoryDetailsView()
            {
                Id = inventory.Id,
                State = inventory.State,
                Type = inventory.Type,
                Description = inventory.Description,
                InventoryItemsNumber = inventory.InventoryItemsNumber,
            };
            if (inventory.StartDate is not null)
            {
                inventoryView.StartDate = inventory.StartDate.Value.ToString("dd/MM/yyyy");
            }
            if (inventory.PlannedStartDate is not null)
            {
                inventoryView.PlannedStartDate = inventory.PlannedStartDate.Value.ToString("dd/MM/yyyy");
            }
            if (inventory.PlannedEndDate is not null)
            {
                inventoryView.PlannedEndDate = inventory.PlannedEndDate.Value.ToString("dd/MM/yyyy");
            }
            if (inventory.EndDate is not null)
            {
                inventoryView.EndDate = inventory.EndDate.Value.ToString("dd/MM/yyyy");
            }
            var scannedItemsNumber = inventory.InventoryItems.Count;
            inventoryView.ScannedItemsNumber = scannedItemsNumber;
            if (inventory.InventoryItemsNumber != 0)
            {
                inventoryView.Progress = scannedItemsNumber / inventory.InventoryItemsNumber * 100;
            }
            else
            {
                inventoryView.Progress = 0;
            }
            return inventoryView;
        }
       
        public async Task<bool> StartInventory(int id)
        {
            var inventory = await _db.Inventories.FirstOrDefaultAsync(x => x.Id == id);
            inventory.State = InventoryState.Active;
            inventory.StartDate = DateTime.Now;
            if (inventory.Type == InventoryType.Full)
            {
                inventory.InventoryItemsNumber = await _db.Items.CountAsync();
            }
            else if (inventory.Type == InventoryType.Partial)
            {
                inventory.InventoryItemsNumber = await _db.Items.Where(x => inventory.Places.Contains(x.Place)).CountAsync();
            }
            try
            {
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<ScannedItemView> GetScannedItemViewAndAddItemToInventory(string barCode, int inventoryId, string userName)
        {
            var item = await _db.Items.Include(x => x.Product).FirstOrDefaultAsync(x => x.BarCode == barCode);
            if (item is not null)
            {
                var inventory = await _db.Inventories.FirstOrDefaultAsync(x => x.Id == inventoryId);
                var user = await _userManager.FindByNameAsync(userName);
                var inventoryItem = new InventoryItem()
                {
                    Item = item,
                    AddDate = DateTime.Now,
                    AddUser = user
                };
                inventory.InventoryItems.Add(inventoryItem);
                try
                {
                    await _db.SaveChangesAsync();
                    return new ScannedItemView()
                    {
                        Id = inventoryItem.Id,
                        BarCode = inventoryItem.Item.BarCode,
                        ProductName = inventoryItem.Item.Product.Name,
                        ProductId = inventoryItem.Item.Product.Id
                    };
                }
                catch
                {
                    return new ScannedItemView();
                }
            }
            return new ScannedItemView();
        }

        public async Task<bool> RemoveScannedItem(int itemId)
        {
            var inventoryItem = await _db.InventoryItems.FirstOrDefaultAsync(x => x.Id == itemId);
            try
            {
                _db.Remove(inventoryItem);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<ScannedItemView>> GetScannedItems(int inventoryId)
        {
            var scannedItems = await _db.InventoryItems.Include(x => x.Item.Product).Where(x => x.Inventory.Id == inventoryId).ToListAsync();
            var scannedItemsView = new List<ScannedItemView>();
            foreach(var item in scannedItems)
            {
                scannedItemsView.Add(new ScannedItemView()
                {
                    Id = item.Id,
                    BarCode = item.Item.BarCode,
                    ProductName = item.Item.Product.Name,
                    ProductId = item.Item.Product.Id
                });
            }
            return scannedItemsView;
        }

        public async Task<string[]> GetPlacesNames()
        {
            return await _db.Places.Select(x => x.Name).ToArrayAsync();
        }
    }
}
