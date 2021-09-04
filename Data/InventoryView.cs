using invMed.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class InventoryView
    {
        public int Id { get; set; }
        public InventoryType? Type { get; set; }
        public InventoryState State { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string PlanedEndDate { get; set; }
    }
}
