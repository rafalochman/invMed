using invMed.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class InventoryDto
    {
        public int Id { get; set; }
        public InventoryTypeEnum InventoryType { get; set; }
        public string InventoryName { get; set; }
        public int InventoryId { get; set; }
    }
}
