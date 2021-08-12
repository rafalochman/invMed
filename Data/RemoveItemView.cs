using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class RemoveItemView
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string? BarCode { get; set; }
        public string ProductName { get; set; }
    }
}
