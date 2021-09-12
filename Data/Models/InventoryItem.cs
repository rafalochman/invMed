using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public DateTime AddDate { get; set; }
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
        public virtual Inventory Inventory { get; set; }
        public virtual AspNetUser AddUser { get; set; }
    }
}
