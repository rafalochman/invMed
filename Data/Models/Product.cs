using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class Product
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
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
