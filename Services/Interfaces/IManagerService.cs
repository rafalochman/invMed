using invMed.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace invMed.Services
{
    public interface IManagerService
    {
        Task<List<ReportDataView>> GetReports();
        Task<bool> GenerateReport(CreateReportInput reportInput);
        Task<ReportDetailsView> GetReportDetailsViewById(int reportId);
        Task<ReportView> GetReportView(int reportId);
    }
}