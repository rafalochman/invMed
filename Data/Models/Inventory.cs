using invMed.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class Inventory
    {
        public int Id { get; set; }
        public InventoryType? Type { get; set; }
        public InventoryState State { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public int InventoryItemsNumber { get; set; }
        public virtual AspNetUser CreateUser { get; set; }
        public virtual ICollection<InventoryItem> InventoryItems { get; set; }
    }
}
