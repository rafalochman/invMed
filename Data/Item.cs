using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class Item
    {
        public int Id { get; set; }
        public virtual Product Product { get; set; }
        public string Code { get; set; }
    }
}
