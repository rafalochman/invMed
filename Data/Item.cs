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
        public string Place { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public virtual Product Product { get; set; }
        public virtual AspNetUser AddUser { get; set; }
    }
}
