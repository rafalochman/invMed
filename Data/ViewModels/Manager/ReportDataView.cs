using invMed.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class ReportDataView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GenerationDate { get; set; }
    }
}
