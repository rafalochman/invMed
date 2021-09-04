using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace invMed.Data
{
    public class AspNetUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
