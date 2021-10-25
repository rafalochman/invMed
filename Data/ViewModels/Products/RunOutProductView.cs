using invMed.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class RunOutProductView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Amount { get; set; }
        public RunOutComunicateTypeEnum ComunicateType { get; set; }

        public RunOutProductView(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Category = product.Category;
            Amount = product.Amount;

            if (product.Amount < product.MinAmount)
            {
                ComunicateType = RunOutComunicateTypeEnum.Empty;
            }
            else
            {
                ComunicateType = RunOutComunicateTypeEnum.Small;
            }
        }
    }
}
