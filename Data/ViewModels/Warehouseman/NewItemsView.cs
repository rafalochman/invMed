using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class NewItemsView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string BarCode { get; set; }
        public DateTime AddDate { get; set; }
    }
}
