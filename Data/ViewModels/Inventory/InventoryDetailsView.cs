using invMed.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class InventoryDetailsView
    {
        public int Id { get; set; }
        public InventoryTypeEnum? Type { get; set; }
        public InventoryStateEnum State { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string PlannedEndDate { get; set; }
        public string PlannedStartDate { get; set; }
        public string EndDate { get; set; }
        public int InventoryItemsNumber { get; set; }
        public int OverItemsNumber { get; set; }
        public int ScannedItemsNumber { get; set; }
        public int Progres { get; set; }
        public string PlacesNames { get; set; }
    }
}
