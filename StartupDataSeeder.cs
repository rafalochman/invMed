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

        public static async Task FillWithProducts(ApplicationDbContext db)
        {
            var product1 = new Product
            {
                Name = "Chusta trójkątna bawełniana",
                Category = "Opatrunki",
                Amount = 3,
                Price = 4,
                Producer = "Firma x",
                Supplier = "Firma x",
            };

            var product2 = new Product
            {
                Name = "Kompresy gazowe niesterylne 12-warst 500 szt",
                Category = "Opatrunki",
                Amount = 66,
                Price = 22,
                Producer = "Firma x",
                Supplier = "Firma x",
            };

            var product3 = new Product
            {
                Name = "Bandaż elastyczny x22",
                Category = "Opatrunki",
                Amount = 66,
                Price = 22,
                Producer = "Firma x",
                Supplier = "Firma x",
            };

            var product4 = new Product
            {
                Name = "Igły do mezoterapii",
                Category = "Igły",
                Amount = 66,
                Price = 27,
                Producer = "Firma x",
                Supplier = "Firma x",
            };

            var product5 = new Product
            {
                Name = "Igły iniekcyjne jednorazowe MICROLANCE 100 szt",
                Category = "Igły",
                Amount = 66,
                Price = 2.5,
                Producer = "Firma x",
                Supplier = "Firma x",
            };

            var product6 = new Product
            {
                Name = "Plaster DURAPORE",
                Category = "Plastry",
                Amount = 66,
                Price = 22,
                Producer = "Firma x",
                Supplier = "Firma x",
            };

            var product7 = new Product
            {
                Name = "PLASTER DERMAFOIL",
                Category = "Plastry",
                Amount = 66,
                Price = 22,
                Producer = "Firma x",
                Supplier = "Firma x",
            };

            var product8 = new Product
            {
                Name = "PLASTER ELASTOPOR TAŚMA ELASTYCZNA WŁÓKNINOWA 10 MB",
                Category = "Plastry",
                Amount = 66,
                Price = 22,
                Producer = "Firma x",
                Supplier = "Firma x",
            };

            await db.Products.AddAsync(product1);
            await db.Products.AddAsync(product2);
            await db.Products.AddAsync(product3);
            await db.Products.AddAsync(product4);
            await db.Products.AddAsync(product5);
            await db.Products.AddAsync(product6);
            await db.Products.AddAsync(product7);
            await db.Products.AddAsync(product8);
            await db.SaveChangesAsync();
        }
    }
}
