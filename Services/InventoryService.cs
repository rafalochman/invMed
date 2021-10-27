using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;
using invMed.Data.Domain;
using invMed.Data.Enums;
using invMed.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace invMed.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly ILogger<IInventoryService> _logger;

        public InventoryService(ApplicationDbContext db, UserManager<AspNetUser> userManager, ILogger<IInventoryService> logger)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create inventory error.");
                return false;
            }
        }

        public async Task<List<InventoryView>> GetInventories()
        {
            var inventories = await _db.Inventories.Include(x => x.Users).OrderByDescending(x => x.PlannedStartDate).ToListAsync();
            var inventoriesView = new List<InventoryView>();
            foreach (var inventory in inventories)
            {
                var inventoryView = new InventoryView(inventory);
                inventoriesView.Add(inventoryView);
            }
            return inventoriesView;
        }

        public async Task<InventoryDetailsView> GetInventoryDetailsViewById(int id)
        {
            var inventory = await _db.Inventories.Include(x => x.InventoryItems).ThenInclude(x => x.Item).Include(x => x.Places).FirstOrDefaultAsync(x => x.Id == id);
            if(inventory is null)
            {
                _logger.LogError("Get inventory details error - inventory not found.");
                return new InventoryDetailsView();
            }

            var inventoryView = new InventoryDetailsView(inventory);
           
            return inventoryView;
        }

        public async Task<bool> StartInventory(int id)
        {
            var inventory = await _db.Inventories.Include(x => x.Places).FirstOrDefaultAsync(x => x.Id == id);
            if (inventory is null)
            {
                _logger.LogError("Start inventory error - inventory not found.");
                return false;
            }

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
                _logger.LogError("Start inventory error - inventory is active or finished.");
                return false;
            }
            try
            {
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Start inventory error.");
                return false;
            }
        }

        public async Task<ScannedItemView> GetScannedItemViewAndAddItemToInventory(string barCode, int inventoryId, string userName)
        {
            var item = await _db.Items.Include(x => x.Product).FirstOrDefaultAsync(x => x.BarCode == barCode);
            var inventory = await _db.Inventories.FirstOrDefaultAsync(x => x.Id == inventoryId);

            if(item is null || inventory is null)
            {
                _logger.LogError("Add item to inventory error - item or inventory not found.");
                return new ScannedItemView();
            }

            if(inventory.Type == InventoryTypeEnum.Partial)
            {
                if (!inventory.Places.Contains(item.Place))
                {
                    return new ScannedItemView();
                }
            }

            if (item is not null)
            {
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
                    return new ScannedItemView(inventoryItem);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Add item to inventory error.");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Remove scanned item error.");
                return false;
            }
        }

        public async Task<List<ScannedItemView>> GetScannedItems(int inventoryId)
        {
            var scannedItems = await _db.InventoryItems.Include(x => x.Item.Product).Where(x => x.Inventory.Id == inventoryId).ToListAsync();
            var scannedItemsView = new List<ScannedItemView>();
            foreach (var item in scannedItems)
            {
                scannedItemsView.Add(new ScannedItemView(item));
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
            if (inventory is null)
            {
                _logger.LogError("Finish inventory error - inventory not found.");
                return false;
            }

            if (inventory.State == InventoryStateEnum.Active)
            {
                inventory.State = InventoryStateEnum.Finished;
                inventory.EndDate = DateTime.Now;
            }
            else
            {
                _logger.LogError("Finish inventory error - inventory is inactive or finished.");
                return false;
            }
            try
            {
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Finish inventory error.");
                return false;
            }
        }

        public async Task<bool> IsManager(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
            {
                _logger.LogError("Check if user has role manager error - user not found.");
                return false;
            }

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
                inventoriesDto.Add(new InventoryDto(inventory));
            }
            return inventoriesDto;
        }

        public async Task<bool> AddNewItem(AddNewItemInput input, string userName)
        {
            var isBarcode = _db.Items.Any(x => x.BarCode == input.Barcode);
            if (isBarcode)
            {
                _logger.LogWarning("Add new item warning - item alredy exists.");
                return false;
            }
            var product = await _db.Products.FirstOrDefaultAsync(x => x.Name == input.ProductName);
            var user = await _userManager.FindByNameAsync(userName);
            var item = new Item { AddDate = DateTime.Now, Product = product, AddUser = user, Type = ItemTypeEnum.Over, BarCode = input.Barcode };
            var barcode = new Barcode(item.BarCode);
            item.BarcodeUrl = barcode.UrlData;
            try
            {
                _db.Items.Add(item);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add new item error.");
                return false;
            }
        }
    }
}
