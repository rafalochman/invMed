using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;
using invMed.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace invMed.Services
{
    public class ManagerService : IManagerService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<IManagerService> _logger;

        public ManagerService(ApplicationDbContext db, ILogger<IManagerService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<ReportDataView>> GetReports()
        {
            var reports = await _db.Reports.ToListAsync();
            var reportsDataView = new List<ReportDataView>();
            foreach(var report in reports)
            {
                reportsDataView.Add(new ReportDataView(report)); 
            }
            return reportsDataView;
        }

        public async Task<bool> GenerateReport(CreateReportInput reportInput)
        {
            var inventory = await _db.Inventories.Include(x => x.InventoryItems).FirstOrDefaultAsync(x => x.Id == reportInput.InventoryDto.InventoryId);
            if (inventory is null)
            {
                _logger.LogError("Create report error - invenory not found.");
                return false;
            }

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create report error.");
                return false;
            }
        }

        public async Task<ReportDetailsView> GetReportDetailsViewById(int reportId)
        {
            var report = await _db.Reports.Include(x => x.Inventory).FirstOrDefaultAsync(x => x.Id == reportId);
            if (report is null)
            {
                _logger.LogError("Get report details view error - report not found.");
                return new ReportDetailsView();
            }

            var reportdetailsView = new ReportDetailsView(report);

            return reportdetailsView;
        }

        public async Task<ReportView> GetReportView(int reportId)
        {
            var report = await _db.Reports
                .Include(x => x.Inventory).ThenInclude(x => x.Users)
                .Include(x => x.Inventory).ThenInclude(x => x.Places)
                .Include(x => x.ReportItems).ThenInclude(x => x.InventoryItem.Item.Product)
                .Include(x => x.ReportItems).ThenInclude(x => x.Item.Product)
                .Include(x => x.ReportItems).ThenInclude(x => x.Item.Place)
                .FirstOrDefaultAsync(x => x.Id == reportId);

            if (report is null)
            {
                _logger.LogError("Get report view error - report not found.");
                return new ReportView();
            }

            var reportView = new ReportView(report);

            return reportView;
        }

        public async Task<List<NotificationView>> GetNotifications()
        {
            var notifications = await _db.Notifications.Where(x => x.CreationDate > DateTime.Now.AddDays(-30)).OrderBy(x => x.IsNew).OrderByDescending(x => x.CreationDate).ToListAsync();
            var notificationsView = new List<NotificationView>();
            foreach(var notification in notifications)
            {
                notificationsView.Add(new NotificationView(notification));
            }
            return notificationsView;
        }

        public async Task<bool> ReadNotifications()
        {
            var notifications = await _db.Notifications.Where(x => x.IsNew == true).ToListAsync();
            
            foreach (var notification in notifications)
            {
                notification.IsNew = false;
            }
            try
            {
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Read notifications error.");
                return false;
            }
        }
    }
}
