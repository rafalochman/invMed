using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class ProductDetailsView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Producer { get; set; }
        public string Supplier { get; set; }

        public int Amount { get; set; }
        public double Price { get; set; }
        public int MinAmount { get; set; }
        public int MaxAmount { get; set; }

        public ProductDetailsView() { }

        public ProductDetailsView(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Category = product.Category;
            Producer = product.Producer;
            Supplier = product.Supplier;
            Amount = product.Amount;
            Price = product.Price;
            MinAmount = product.MinAmount;
            MaxAmount = product.MaxAmount;
        }
    }
}
