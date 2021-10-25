using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class RemoveItemView
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string? BarCode { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }

        public RemoveItemView() { }

        public RemoveItemView(Item item, int number)
        {
            Id = item.Id;
            Number = number;
            BarCode = item.BarCode;
            ProductName = item.Product.Name;
            ProductId = item.Product.Id;
        }
    }
}
