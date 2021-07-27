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
            };
            await userManager.CreateAsync(admin, "1qaz@WSX");
            await userManager.AddToRoleAsync(admin, "ADMIN");

            var user1 = new AspNetUser
            {
                UserName = "anna@kowalska.com",
                Email = "anna@kowalska.com",
                Name = "Anna",
                Surname = "Kowalska",
            };
            await userManager.CreateAsync(user1, "1qaz@WSX");
            await userManager.AddToRoleAsync(user1, "USER");

            var user2 = new AspNetUser
            {
                UserName = "jan@nowak.com",
                Email = "jan@nowak.com",
                Name = "Jan",
                Surname = "Nowak",
            };
            await userManager.CreateAsync(user2, "1qaz@WSX");
            await userManager.AddToRoleAsync(user2, "USER");

            var user3 = new AspNetUser
            {
                UserName = "aleksandra@kot.com",
                Email = "aleksandra@kot.com",
                Name = "Aleksandra",
                Surname = "Kot",
            };
            await userManager.CreateAsync(user3, "1qaz@WSX");
            await userManager.AddToRoleAsync(user3, "USER");

            var manager = new AspNetUser
            {
                UserName = "marek@kowalczewski.com",
                Email = "marek@kowalczewski.com",
                Name = "Marek",
                Surname = "Kowalczewski",
            };
            await userManager.CreateAsync(manager, "1qaz@WSX");
            await userManager.AddToRoleAsync(manager, "MANAGER");
        }
    }
}
