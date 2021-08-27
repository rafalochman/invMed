using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class ProductItemView
    {
        public int Id { get; set; }
        public string BarCode { get; set; }
        public string Place { get; set; }
        public string? ExpirationDate { get; set; }
    }
}
