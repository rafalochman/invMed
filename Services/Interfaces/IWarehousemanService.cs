using invMed.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace invMed.Services
{
    public interface IWarehousemanService
    {
        Task<(string barcode, string barcodeUrl)> AddItemAndGetBarcode(AddItemInput input, string userName);
        Task<IEnumerable<string>> GetAllPlacesNames();
        Task<IEnumerable<string>> GetAllProductsNames();
        Task<List<NewItemsView>> GetNewItems();
        Task<RemoveItemView> GetRemoveItemViewByItemBarCode(string barCode, int number);
        Task<bool> RemoveItems(List<RemoveItemView> items);
        Task<IEnumerable<string>> SearchPlacesNames(string searchValue);
        Task<IEnumerable<string>> SearchProductsNames(string searchValue);
    }
}