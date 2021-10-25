using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class NewItemsView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string BarCode { get; set; }
        public DateTime AddDate { get; set; }

        public NewItemsView(Item item)
        {
            Id = item.Id;
            Name = item.Product.Name;
            Category = item.Product.Category;
            BarCode = item.BarCode;
            AddDate = item.AddDate;
        }
    }
}
