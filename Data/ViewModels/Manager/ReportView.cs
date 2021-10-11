using invMed.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class ReportView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GenerationDate { get; set; }
        public string InventoryName { get; set; }
        public string InventoryDescription { get; set; }
        public string InventoryType { get; set; }
        public string InventoryStartDate { get; set; }
        public string InventoryFinishDate { get; set; }
        public string WarehousemenNames { get; set; }
        public string PlacesNames { get; set; }
        public int OverNumber { get; set; }
        public int ShortageNumber { get; set; }
        public List<ReportItemView> OverItems { get; set; }
        public List<ReportItemView> ShortageItems { get; set; }
    }
}
