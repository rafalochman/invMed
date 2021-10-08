using invMed.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace invMed.Services
{
    public interface IInventoryService
    {
        Task<bool> CreateInventory(CreateInventoryInput input);
        Task<List<InventoryView>> GetInventories();
        Task<InventoryDetailsView> GetInventoryDetailsViewById(int id);
        Task<string[]> GetPlacesNames();
        Task<List<ScannedItemView>> GetScannedItems(int inventoryId);
        Task<ScannedItemView> GetScannedItemViewAndAddItemToInventory(string barCode, int inventoryId, string userName);
        Task<bool> RemoveScannedItem(int itemId);
        Task<bool> StartInventory(int id);
        Task<string[]> GetWarehousemenLogins();
        Task<bool> FinishInventory(int id);
        Task<bool> IsManager(string userName);
        Task<IEnumerable<InventoryDto>> GetAllFinishedInventories();
        Task<IEnumerable<InventoryDto>> SearchFinishedInventories(string searchValue);

    }
}