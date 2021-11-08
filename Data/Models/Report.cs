using invMed.Data.Enums;
using invMed.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class Report
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime GenerationDate { get; set; }
        public int AllItems { get; set; }
        public int ScannedItems { get; set; }
        public virtual Inventory Inventory { get; set; }
        public virtual ICollection<ReportItem> ReportItems { get; set; }
    }
}
