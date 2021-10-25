using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class ScannedItemView
    {
        public int Id { get; set; }
        public string? BarCode { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }

        public ScannedItemView() { }

        public ScannedItemView(InventoryItem inventoryItem)
        {
            Id = inventoryItem.Id;
            BarCode = inventoryItem.Item.BarCode;
            ProductName = inventoryItem.Item.Product.Name;
            ProductId = inventoryItem.Item.Product.Id;
        }
    }
}
