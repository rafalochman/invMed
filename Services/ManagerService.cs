using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace invMed.Services
{
    public class ManagerService : IManagerService
    {
        private readonly ApplicationDbContext _db;

        public ManagerService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<ReportDataView>> GetReports()
        {
            var reports = await _db.Reports.ToListAsync();
            var reportsDataView = new List<ReportDataView>();
            foreach(var report in reports)
            {
                var reportDataView = new ReportDataView
                {
                    Id = report.Id,
                    Name = report.Name,
                    Description = report.Description,
                    GenerationDate = report.GenerationDate.Value.ToString("dd/MM/yyyy")
                };
                reportsDataView.Add(reportDataView); 
            }
            return reportsDataView;
        }

        public async Task<bool> GenerateReport(CreateReportInput reportInput)
        {
            return true;
        }
    }
}
