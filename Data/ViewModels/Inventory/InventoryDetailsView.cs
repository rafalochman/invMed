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

        public InventoryDetailsView() { }

        public InventoryDetailsView(Inventory inventory)
        {
            Id = inventory.Id;
            Name = inventory.Name;
            State = inventory.State;
            Type = inventory.Type;
            Description = inventory.Description;
            InventoryItemsNumber = inventory.InventoryItemsNumber;
            StartDate = inventory.StartDate.HasValue ? inventory.StartDate.Value.ToString("dd/MM/yyyy") : null;
            PlannedStartDate = inventory.PlannedStartDate.HasValue ? inventory.PlannedStartDate.Value.ToString("dd/MM/yyyy") : null;
            PlannedEndDate = inventory.PlannedEndDate.HasValue ? inventory.PlannedEndDate.Value.ToString("dd/MM/yyyy") : null;
            EndDate = inventory.EndDate.HasValue ? inventory.EndDate.Value.ToString("dd/MM/yyyy") : null;

            var scannedItemsNumber = inventory.InventoryItems.Where(x => x.Item.Type != ItemTypeEnum.Over).Count();
            ScannedItemsNumber = scannedItemsNumber;
            OverItemsNumber = inventory.InventoryItems.Where(x => x.Item.Type == ItemTypeEnum.Over).Count();
            if (inventory.InventoryItemsNumber != 0)
            {
                Progres = (int)((double)scannedItemsNumber / (double)inventory.InventoryItemsNumber * 100);
            }
            else
            {
                Progres = 0;
            }

            if (inventory.Type == InventoryTypeEnum.Partial)
            {
                var placesNames = String.Empty;
                foreach (var place in inventory.Places.Select(x => x.Name))
                {
                    placesNames += place + " ";
                }
                PlacesNames = placesNames;
            }
        }
    }
}
