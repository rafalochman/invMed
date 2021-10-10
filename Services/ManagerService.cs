using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;
using invMed.Data.Enums;
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
            var inventory = await _db.Inventories.Include(x => x.InventoryItems).FirstOrDefaultAsync(x => x.Id == reportInput.InventoryDto.InventoryId);
            var reportItems = new List<ReportItem>();
            var regularItems = await _db.Items.Where(x => x.Type == ItemTypeEnum.Regular).ToListAsync();
            if (inventory.Type == InventoryTypeEnum.Partial)
            {
                regularItems = regularItems.Where(x => inventory.Places.Contains(x.Place)).ToList();
            }
            
            foreach (var item in regularItems)
            {
                if (!inventory.InventoryItems.Select(x => x.ItemId).Contains(item.Id))
                {
                    var reportItem = new ReportItem()
                    {
                        Item = item,
                        ReportItemType = ReportItemTypeEnum.Shortage
                    };
                    reportItems.Add(reportItem);
                }
            }
            var overItems = inventory.InventoryItems.Where(x => x.Item.Type == ItemTypeEnum.Over);
            foreach (var item in overItems)
            {
                var reportItem = new ReportItem()
                {
                    InventoryItem = item,
                    ReportItemType = ReportItemTypeEnum.Over
                };
                reportItems.Add(reportItem);
            }

            var report = new Report()
            {
                Name = reportInput.Name,
                Description = reportInput.Description,
                GenerationDate = DateTime.Now,
                Inventory = inventory,
                AllItems = inventory.InventoryItemsNumber,
                ScannedItems = inventory.InventoryItems.Count,
                ReportItems = reportItems
            };
            try
            {
                _db.Reports.Add(report);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
