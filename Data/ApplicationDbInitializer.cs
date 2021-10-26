using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace invMed.Data
{
    public static class ApplicationDbInitializer
    {
        public static void SeedUser(UserManager<AspNetUser> userManager)
        {
            if (userManager.FindByEmailAsync("superadmin@invmed.com").Result == null)
            {
                var superadmin = new AspNetUser
                {
                    UserName = "superadmin@invmed.com",
                    Email = "superadmin@invmed.com",
                    Name = "superadmin",
                    Surname = "superadmin",
                    IsActive = true
                };

                var result = userManager.CreateAsync(superadmin, "1qaz@WSX").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(superadmin, RoleNormalizedName.Admin).Wait();
                    userManager.AddToRoleAsync(superadmin, RoleNormalizedName.Manager).Wait();
                    userManager.AddToRoleAsync(superadmin, RoleNormalizedName.Warehouseman).Wait();
                }
            } 
        }
    }
}
