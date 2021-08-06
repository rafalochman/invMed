using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;
using Microsoft.AspNetCore.Identity;

namespace invMed
{
    public static class StartupDataSeeder
    {
        public static async Task FillWithUsers(UserManager<AspNetUser> userManager)
        {
            var admin = new AspNetUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                Name = "admin",
                Surname = "admin",
                IsActive = true
            };
            await userManager.CreateAsync(admin, "1qaz@WSX");
            await userManager.AddToRoleAsync(admin, "ADMIN");

            var warehouseman1 = new AspNetUser
            {
                UserName = "anna@kowalska.com",
                Email = "anna@kowalska.com",
                Name = "Anna",
                Surname = "Kowalska",
                IsActive = true
            };
            await userManager.CreateAsync(warehouseman1, "1qaz@WSX");
            await userManager.AddToRoleAsync(warehouseman1, "MAGAZYNIER");

            var warehouseman2 = new AspNetUser
            {
                UserName = "jan@nowak.com",
                Email = "jan@nowak.com",
                Name = "Jan",
                Surname = "Nowak",
                IsActive = true
            };
            await userManager.CreateAsync(warehouseman2, "1qaz@WSX");
            await userManager.AddToRoleAsync(warehouseman2, "MAGAZYNIER");

            var warehouseman3 = new AspNetUser
            {
                UserName = "aleksandra@kot.com",
                Email = "aleksandra@kot.com",
                Name = "Aleksandra",
                Surname = "Kot",
                IsActive = true
            };
            await userManager.CreateAsync(warehouseman3, "1qaz@WSX");
            await userManager.AddToRoleAsync(warehouseman3, "MAGAZYNIER");

            var manager = new AspNetUser
            {
                UserName = "marek@kowalczewski.com",
                Email = "marek@kowalczewski.com",
                Name = "Marek",
                Surname = "Kowalczewski",
                IsActive = true
            };
            await userManager.CreateAsync(manager, "1qaz@WSX");
            await userManager.AddToRoleAsync(manager, "MANAGER");
        }
    }
}
