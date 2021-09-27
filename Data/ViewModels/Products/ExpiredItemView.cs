using invMed.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class ExpiredItemView
    {
        public int Id { get; set; }
        public string BarCode { get; set; }
        public string? ExpirationDate { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public ExpiredComunicateType ComunicateType { get; set; }
    }
}
