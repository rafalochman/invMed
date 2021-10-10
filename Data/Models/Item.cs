using invMed.Data.Enums;
using invMed.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class Item
    {
        public int Id { get; set; }
        public string BarCode { get; set; }
        public string BarcodeUrl { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public ItemTypeEnum Type { get; set; }
        public virtual Place Place { get; set; }
        public virtual Product Product { get; set; }
        public virtual AspNetUser AddUser { get; set; }
        public virtual ICollection<InventoryItem> InventoryItems { get; set; }
        public virtual ICollection<ReportItem> ReportItems { get; set; }
    }
}
