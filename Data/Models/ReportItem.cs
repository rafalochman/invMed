using invMed.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class ReportItem
    {
        public int Id { get; set; }
        public ReportItemTypeEnum ReportItemType { get; set; }
        public virtual Item Item { get; set; }
        public virtual InventoryItem InventoryItem { get; set; }
        public virtual Report Report { get; set; }
    }
}
