using invMed.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class ReportDetailsView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GenerationDate { get; set; }
        public string InventoryName { get; set; }
        public string InventoryDescription { get; set; }

        public ReportDetailsView() { }

        public ReportDetailsView(Report report)
        {
            Name = report.Name;
            Description = report.Description;
            GenerationDate = report.GenerationDate.Value.ToString("dd/MM/yyyy");
            InventoryName = report.Inventory.Name;
            InventoryDescription = report.Inventory.Description;
        }
    }
}
