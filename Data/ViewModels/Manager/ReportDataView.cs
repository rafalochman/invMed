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

        public ReportDataView(Report report)
        {
            Id = report.Id;
            Name = report.Name;
            Description = report.Description;
            GenerationDate = report.GenerationDate.ToString("dd/MM/yyyy");
        }
    }
}
