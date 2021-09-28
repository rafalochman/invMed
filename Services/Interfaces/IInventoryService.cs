﻿using invMed.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace invMed.Services
{
    public interface IInventoryService
    {
        Task<bool> CreateInventory(CreateInventoryInput input, string userName);
        Task<List<InventoryView>> GetInactiveAndActiveInventories();
        Task<InventoryDetailsView> GetInventoryDetailsViewById(int id);
        Task<string[]> GetPlacesNames();
        Task<List<ScannedItemView>> GetScannedItems(int inventoryId);
        Task<ScannedItemView> GetScannedItemViewAndAddItemToInventory(string barCode, int inventoryId, string userName);
        Task<bool> RemoveScannedItem(int itemId);
        Task<bool> StartInventory(int id);
    }
}