using invMed.Data.Enums;
using invMed.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class Inventory
    {
        public int Id { get; set; }
        public InventoryTypeEnum? Type { get; set; }
        public InventoryStateEnum State { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public int InventoryItemsNumber { get; set; }
        public virtual ICollection<InventoryItem> InventoryItems { get; set; }
        public virtual ICollection<Place> Places { get; set; }
        public virtual ICollection<AspNetUser> Users { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
