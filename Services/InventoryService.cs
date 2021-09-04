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
            var inventory = new Inventory { Type = input.Type, State = InventoryState.Inactive, Description = input.Description, StartDate = input.StartDate, PlanedEndDate = input.PlanedEndDate, CreateUser = user };
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
                if(inventory.StartDate is not null)
                {
                    inventoryView.StartDate = inventory.StartDate.Value.ToString("dd/MM/yyyy");
                }
                if(inventory.PlanedEndDate is not null)
                {
                    inventoryView.PlanedEndDate = inventory.PlanedEndDate.Value.ToString("dd/MM/yyyy");
                }
                inventoriesView.Add(inventoryView);
            }
            return inventoriesView;
        }
    }
}
