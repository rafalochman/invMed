using invMed.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class ReportItemView
    {
        public int Id { get; set; }
        public string BarCode { get; set; }
        public string Place { get; set; }
        public string AddDate { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public ReportItemTypeEnum Type { get; set; }
    }
}
