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
        public InventoryTypeEnum? Type { get; set; }
        public InventoryStateEnum State { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string PlannedStartDate { get; set; }
        public string PlannedEndDate { get; set; }
        public List<string> UserNames { get; set; }

        public InventoryView(Inventory inventory)
        {
            Id = inventory.Id;
            Name = inventory.Name;
            State = inventory.State;
            Type = inventory.Type;
            Description = inventory.Description;
            PlannedStartDate = inventory.PlannedStartDate.HasValue ? inventory.PlannedStartDate.Value.ToString("dd/MM/yyyy") : null;
            PlannedEndDate = inventory.PlannedEndDate.HasValue ? inventory.PlannedEndDate.Value.ToString("dd/MM/yyyy") : null;
            UserNames = inventory.Users.Select(x => x.UserName).ToList();
        }
    }
}
