using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;

namespace invMed.Services
{
    public class ItemsService
    {
        private readonly ApplicationDbContext _db;

        public ItemsService(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<List<Item>> GetItemsByProductId(int id)
        {
            List<Item> items = _db.Items.Where(i => i.Product.Id == id).ToList();
            return Task.FromResult(items);
        }
    }
}
