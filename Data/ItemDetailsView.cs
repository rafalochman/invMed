using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class ItemDetailsView
    {
        public int Id { get; set; }
        public string BarCode { get; set; }

        public string BarcodeUrl { get; set; }
        public string Place { get; set; }
        public string AddDate { get; set; }
        public string? ExpirationDate { get; set; }

        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
    }
}
