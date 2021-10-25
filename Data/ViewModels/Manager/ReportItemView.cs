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

        public ReportItemView(ReportItem reportItem, ReportItemTypeEnum type)
        {
            if (type == ReportItemTypeEnum.Shortage)
            {
                BarCode = reportItem.Item.BarCode;
                Place = reportItem.Item.Place.Name;
                ProductCategory = reportItem.Item.Product.Category;
                ProductName = reportItem.Item.Product.Name;
                AddDate = reportItem.Item.AddDate.ToString("dd/MM/yyyy");
                Type = ReportItemTypeEnum.Shortage;
            }
            else if (type == ReportItemTypeEnum.Over)
            {
                BarCode = reportItem.InventoryItem.Item.BarCode;
                ProductCategory = reportItem.InventoryItem.Item.Product.Category;
                ProductName = reportItem.InventoryItem.Item.Product.Name;
                Type = ReportItemTypeEnum.Over;
            }
        }
    }
}
