using invMed.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace invMed.Services
{
    public interface IProductsService
    {
        Task<bool> AddProduct(AddProductInput input);
        Task<List<ExpiredItemView>> GetExpiredItems();
        Task<ItemDetailsView> GetItemDetailsViewById(int id);
        Task<List<ProductItemView>> GetItemsByProductId(int productId);
        Task<ProductDetailsView> GetProductDetailsViewById(int id);
        Task<List<Product>> GetProducts();
        Task<List<RunOutProductView>> GetRunOutProducts();
    }
}