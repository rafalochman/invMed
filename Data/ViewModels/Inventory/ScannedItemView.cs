using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class ScannedItemView
    {
        public int Id { get; set; }
        public string? BarCode { get; set; }
        public string ProductName { get; set; }

        public int ProductId { get; set; }
    }
}
