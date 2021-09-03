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
    }
}
