using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class Inventory
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? PlanedEndDate { get; set; }
        public virtual AspNetUser CreateUser { get; set; }
        public virtual ICollection<InventoryItem> InventoryItems { get; set; }
    }
}
