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

        public ReportView() { }

        public ReportView(Report report)
        {
            Name = report.Name;
            Description = report.Description;
            GenerationDate = report.GenerationDate.Value.ToString("dd/MM/yyyy");
            InventoryName = report.Inventory.Name;
            InventoryDescription = report.Inventory.Description;
            InventoryType = report.Inventory.Type.GetDisplayName();
            InventoryStartDate = report.Inventory.StartDate.Value.ToString("dd/MM/yyyy");
            InventoryFinishDate = report.Inventory.EndDate.Value.ToString("dd/MM/yyyy");

            var warehousemenNames = String.Empty;
            foreach (var userName in report.Inventory.Users.Select(x => x.UserName))
            {
                warehousemenNames += userName + " ";
            }
            WarehousemenNames = warehousemenNames;

            if (report.Inventory.Type == InventoryTypeEnum.Partial)
            {
                var placesNames = String.Empty;
                foreach (var place in report.Inventory.Places.Select(x => x.Name))
                {
                    placesNames += place + " ";
                }
                PlacesNames = placesNames;
            }

            var shortageItems = new List<ReportItemView>();
            var overItems = new List<ReportItemView>();
            foreach (var reportItem in report.ReportItems)
            {
                if (reportItem.ReportItemType == ReportItemTypeEnum.Shortage)
                {
                    shortageItems.Add(new ReportItemView(reportItem, ReportItemTypeEnum.Shortage));
                }
                else if (reportItem.ReportItemType == ReportItemTypeEnum.Over)
                {
                    overItems.Add(new ReportItemView(reportItem, ReportItemTypeEnum.Over));
                }
            }
            OverItems = overItems;
            ShortageItems = shortageItems;
            OverNumber = overItems.Count;
            ShortageNumber = shortageItems.Count;
        }
    }
}
