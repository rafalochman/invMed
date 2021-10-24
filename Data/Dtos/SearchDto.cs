using invMed.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class SearchDto
    {
        public int Id { get; set; }
        public SearchTypeEnum Type { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string? Barcode { get; set; }

        public SearchDto(Product product)
        {
            Id = product.Id;
            Type = SearchTypeEnum.Product;
            Name = product.Name;
            Category = product.Category;
        }

        public SearchDto(Item item)
        {
            Id = item.Id;
            Type = SearchTypeEnum.Item;
            Name = item.Product.Name;
            Category = item.Product.Category;
            Barcode = item.BarCode;
        }
    }
}
