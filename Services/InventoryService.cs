using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;
using invMed.Data.Enums;
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
            var inventory = new Inventory { Type = input.Type, State = InventoryState.Inactive, Description = input.Description, PlannedStartDate = input.PlannedStartDate, PlannedEndDate = input.PlannedEndDate, CreateUser = user };
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
                    inventory.ItemsNumberScan = await _db.Items.CountAsync();
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
                ItemsNumberScan = inventory.ItemsNumberScan,
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
            inventoryView.ScannedNumber = scannedItemsNumber;
            if (scannedItemsNumber != 0)
            {
                inventoryView.Progress = scannedItemsNumber / inventory.ItemsNumberScan * 100;
            }
            else
            {
                inventoryView.Progress = 0;
            }
            return inventoryView;
        }
       
    }
}
