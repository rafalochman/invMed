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

        public async Task<ReportDetailsView> GetReportDetailsViewById(int reportId)
        {
            var report = await _db.Reports.Include(x => x.Inventory).FirstOrDefaultAsync(x => x.Id == reportId);
            var reportdetailsView = new ReportDetailsView()
            {
                Name = report.Name,
                Description = report.Description,
                GenerationDate = report.GenerationDate.Value.ToString("dd/MM/yyyy"),
                InventoryName = report.Inventory.Name,
                InventoryDescription = report.Inventory.Description
            };
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

            var reportView = new ReportView()
            {
                Name = report.Name,
                Description = report.Description,
                GenerationDate = report.GenerationDate.Value.ToString("dd/MM/yyyy"),
                InventoryName = report.Inventory.Name,
                InventoryDescription = report.Inventory.Description,
                InventoryType = report.Inventory.Type.GetDisplayName(),
                InventoryStartDate = report.Inventory.StartDate.Value.ToString("dd/MM/yyyy"),
                InventoryFinishDate = report.Inventory.EndDate.Value.ToString("dd/MM/yyyy")
            };

            var warehousemenNames = String.Empty;
            foreach(var userName in report.Inventory.Users.Select(x => x.UserName))
            {
                warehousemenNames += userName + " ";
            }

            reportView.WarehousemenNames = warehousemenNames;

            if(report.Inventory.Type == InventoryTypeEnum.Partial)
            {
                var placesNames = String.Empty;
                foreach (var place in report.Inventory.Places.Select(x => x.Name))
                {
                    placesNames += place + " ";
                }
                reportView.PlacesNames = placesNames;
            }

            var shortageItems = new List<ReportItemView>();
            var overItems = new List<ReportItemView>();

            foreach(var reportItem in report.ReportItems)
            {
                if(reportItem.ReportItemType == ReportItemTypeEnum.Shortage)
                {
                    var shortageItemView = new ReportItemView
                    {
                        BarCode = reportItem.Item.BarCode,
                        Place = reportItem.Item.Place.Name,
                        ProductCategory = reportItem.Item.Product.Category,
                        ProductName = reportItem.Item.Product.Name,
                        AddDate = reportItem.Item.AddDate.ToString("dd/MM/yyyy"),
                        Type = ReportItemTypeEnum.Shortage
                    };

                    shortageItems.Add(shortageItemView);
                }
                else if (reportItem.ReportItemType == ReportItemTypeEnum.Over)
                {
                    var overItemView = new ReportItemView()
                    {
                        BarCode = reportItem.InventoryItem.Item.BarCode,
                        ProductCategory = reportItem.InventoryItem.Item.Product.Category,
                        ProductName = reportItem.InventoryItem.Item.Product.Name,
                        Type = ReportItemTypeEnum.Over
                    };
                    overItems.Add(overItemView);
                }
            }

            reportView.OverItems = overItems;
            reportView.ShortageItems = shortageItems;
            reportView.OverNumber = overItems.Count;
            reportView.ShortageNumber = shortageItems.Count;

            return reportView;
        }
    }
}
