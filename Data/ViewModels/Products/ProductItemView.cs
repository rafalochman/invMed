using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class ProductItemView
    {
        public int Id { get; set; }
        public string BarCode { get; set; }
        public string Place { get; set; }
        public string? ExpirationDate { get; set; }

        public ProductItemView(Item item)
        {
            Id = item.Id;
            BarCode = item.BarCode;
            Place = item.Place.Name;
            ExpirationDate = item.ExpirationDate.HasValue ? item.ExpirationDate.Value.ToString("dd/MM/yyyy") : null;
        }
    }
}
