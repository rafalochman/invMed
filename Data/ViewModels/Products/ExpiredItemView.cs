using invMed.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class ExpiredItemView
    {
        public int Id { get; set; }
        public string BarCode { get; set; }
        public string? ExpirationDate { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public ExpiredComunicateTypeEnum ComunicateType { get; set; }

        public ExpiredItemView(Item item)
        {
            Id = item.Id;
            ProductName = item.Product.Name;
            ProductCategory = item.Product.Category;
            BarCode = item.BarCode;

            if (item.ExpirationDate is not null)
            {
                ExpirationDate = item.ExpirationDate.Value.ToString("dd/MM/yyyy");
            }

            if (item.ExpirationDate > DateTime.Now)
            {
                ComunicateType = ExpiredComunicateTypeEnum.Expired;
            }
            else
            {
                ComunicateType = ExpiredComunicateTypeEnum.Close;
            }
        }
    }
}
