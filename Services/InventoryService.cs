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
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AspNetUser> _userManager;

        public InventoryService(ApplicationDbContext db, UserManager<AspNetUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<bool> CreateInventory(CreateInventoryInput input)
        {
            var places = new List<Place>();
            if (input.Places is not null)
            {
                foreach (var placeName in input.Places)
                {
                    places.Add(await _db.Places.FirstOrDefaultAsync(x => x.Name == placeName));
                }
            }
            var warehousemen = new List<AspNetUser>();
            if (input.Warehousemen is not null)
            {
                foreach (var warehouseman in input.Warehousemen)
                {
                    warehousemen.Add(await _userManager.FindByNameAsync(warehouseman));
                }
            }
            var inventory = new Inventory { Name = input.Name, Type = input.Type, State = InventoryStateEnum.Inactive, Description = input.Description, PlannedStartDate = input.PlannedStartDate, PlannedEndDate = input.PlannedEndDate, Places = places, Users = warehousemen };
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

        public async Task<List<InventoryView>> GetInventories()
        {
            var inventories = await _db.Inventories.Include(x => x.Users).ToListAsync();
            var inventoriesView = new List<InventoryView>();
            foreach (var inventory in inventories)
            {
                var inventoryView = new InventoryView()
                {
                    Id = inventory.Id,
                    Name = inventory.Name,
                    State = inventory.State,
                    Type = inventory.Type,
                    Description = inventory.Description,
                };
                if (inventory.PlannedStartDate is not null)
                {
                    inventoryView.PlannedStartDate = inventory.PlannedStartDate.Value.ToString("dd/MM/yyyy");
                }
                if (inventory.PlannedEndDate is not null)
                {
                    inventoryView.PlannedEndDate = inventory.PlannedEndDate.Value.ToString("dd/MM/yyyy");
                }
                if (inventory.Type == InventoryTypeEnum.Full)
                {
                    inventory.InventoryItemsNumber = await _db.Items.Where(x => x.Type != ItemTypeEnum.Over).CountAsync();
                }
                var userNames = new List<string>();

                if (inventory.Users is not null)
                {
                    foreach (var user in inventory.Users)
                    {
                        userNames.Add(user.UserName);
                    }
                }
                inventoryView.UserNames = userNames;
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
                Name = inventory.Name,
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
                inventoryView.Progres = (int)((double)scannedItemsNumber / (double)inventory.InventoryItemsNumber * 100);
            }
            else
            {
                inventoryView.Progres = 0;
            }
            return inventoryView;
        }

        public async Task<bool> StartInventory(int id)
        {
            var inventory = await _db.Inventories.FirstOrDefaultAsync(x => x.Id == id);
            if (inventory.State == InventoryStateEnum.Inactive)
            {
                inventory.State = InventoryStateEnum.Active;
                inventory.StartDate = DateTime.Now;
                if (inventory.Type == InventoryTypeEnum.Full)
                {
                    inventory.InventoryItemsNumber = await _db.Items.Where(x => x.Type != ItemTypeEnum.Over).CountAsync();
                }
                else if (inventory.Type == InventoryTypeEnum.Partial)
                {
                    inventory.InventoryItemsNumber = await _db.Items.Where(x => inventory.Places.Contains(x.Place)).Where(x => x.Type != ItemTypeEnum.Over).CountAsync();
                }
            }
            else
            {
                return false;
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

        public async Task<bool> RemoveScannedItem(int inventoryItemId)
        {
            var inventoryItem = await _db.InventoryItems.FirstOrDefaultAsync(x => x.Id == inventoryItemId);
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
            foreach (var item in scannedItems)
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

        public async Task<string[]> GetWarehousemenLogins()
        {
            var warehousemen = await _userManager.GetUsersInRoleAsync(RoleNormalizedName.Warehouseman);
            return warehousemen.Select(x => x.UserName).ToArray();
        }

        public async Task<bool> FinishInventory(int id)
        {
            var inventory = await _db.Inventories.FirstOrDefaultAsync(x => x.Id == id);
            if (inventory.State == InventoryStateEnum.Active)
            {
                inventory.State = InventoryStateEnum.Finished;
                inventory.EndDate = DateTime.Now;
            }
            else
            {
                return false;
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

        public async Task<bool> IsManager(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var isManager = await _userManager.IsInRoleAsync(user, RoleName.Manager);
            if (isManager)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<InventoryDto>> SearchFinishedInventories(string searchValue = "")
        {
            var inventories = await _db.Inventories.Where(x => x.State == InventoryStateEnum.Finished).ToListAsync();
            if (searchValue != "")
            {
                inventories = inventories.Where(x => x.Name.Contains(searchValue)).ToList();
            }
            var inventoriesDto = new List<InventoryDto>();
            foreach (var inventory in inventories)
            {
                var inventoryDto = new InventoryDto()
                {
                    InventoryName = inventory.Name,
                    InventoryId = inventory.Id,
                    InventoryType = inventory.Type.Value
                };
                inventoriesDto.Add(inventoryDto);
            }
            return inventoriesDto;
        }
    }
}
