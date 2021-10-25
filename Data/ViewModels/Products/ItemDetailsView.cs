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

        public ItemDetailsView() { }

        public ItemDetailsView(Item item)
        {
            Id = item.Id;
            BarCode = item.BarCode;
            BarcodeUrl = item.BarcodeUrl;
            Place = item.Place.Name;
            AddDate = item.AddDate.ToString("dd/MM/yyyy");
            ProductCategory = item.Product.Category;
            ProductName = item.Product.Name;
            ExpirationDate = item.ExpirationDate.HasValue ? item.ExpirationDate.Value.ToString("dd/MM/yyyy") : null;
        }
    }
}
