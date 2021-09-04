using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;
using invMed.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            await db.Products.AddAsync(new Product
            {
                Name = "Chusta trójkątna bawełniana",
                Category = "Opatrunki",
                Amount = 0,
                Price = 4,
                MinAmount = 2,
                Producer = "Firma x",
                Supplier = "Firma x",
            });

            await db.Products.AddAsync(new Product
            {
                Name = "Kompresy gazowe niesterylne 12-warst 500 szt",
                Category = "Opatrunki",
                Amount = 0,
                Price = 22,
                MinAmount = 3,
                Producer = "Firma x",
                Supplier = "Firma x",
            });

            await db.Products.AddAsync(new Product
            {
                Name = "Bandaż elastyczny x22",
                Category = "Opatrunki",
                Amount = 0,
                Price = 22,
                MinAmount = 1,
                Producer = "Firma x",
                Supplier = "Firma x",
            });

            await db.Products.AddAsync(new Product
            {
                Name = "Igły do mezoterapii",
                Category = "Igły",
                Amount = 0,
                Price = 27,
                MinAmount = 1,
                Producer = "Firma x",
                Supplier = "Firma x",
            });

            await db.Products.AddAsync(new Product
            {
                Name = "Igły iniekcyjne jednorazowe MICROLANCE 100 szt",
                Category = "Igły",
                Amount = 0,
                Price = 2.5,
                MinAmount = 1,
                Producer = "Firma x",
                Supplier = "Firma x",
            });

            await db.Products.AddAsync(new Product
            {
                Name = "Plaster DURAPORE",
                Category = "Plastry",
                Amount = 0,
                Price = 22,
                MinAmount = 0,
                Producer = "Firma x",
                Supplier = "Firma x",
            });

            await db.Products.AddAsync(new Product
            {
                Name = "PLASTER DERMAFOIL",
                Category = "Plastry",
                Amount = 0,
                Price = 22,
                MinAmount = 0,
                Producer = "Firma x",
                Supplier = "Firma x",
            });

            await db.Products.AddAsync(new Product
            {
                Name = "PLASTER ELASTOPOR TAŚMA ELASTYCZNA WŁÓKNINOWA 10 MB",
                Category = "Plastry",
                Amount = 0,
                Price = 22,
                MinAmount = 0,
                Producer = "Firma x",
                Supplier = "Firma x",
            });
            await db.SaveChangesAsync();
        }

        public static async Task FillWithItems(ApplicationDbContext db)
        {
            var product1 = await db.Products.FirstOrDefaultAsync(x => x.Id == 1);
            var product2 = await db.Products.FirstOrDefaultAsync(x => x.Id == 2);
            var product3 = await db.Products.FirstOrDefaultAsync(x => x.Id == 3);

            await db.Items.AddAsync(new Item
            {
                BarCode = "00000001",
                Place = "A/1",
                AddDate = DateTime.Now,
                Product = product1,
                ExpirationDate = DateTime.Now.AddDays(3)
            });
            product1.Amount++;

            await db.Items.AddAsync(new Item
            {
                BarCode = "00000002",
                Place = "A/1",
                AddDate = DateTime.Now,
                Product = product1,
                ExpirationDate = DateTime.Now.AddDays(4)
            });
            product1.Amount++;

            await db.Items.AddAsync(new Item
            {
                BarCode = "00000003",
                Place = "A/2",
                AddDate = DateTime.Now,
                Product = product1,
                ExpirationDate = DateTime.Now.AddDays(5)
            });
            product1.Amount++;

            await db.Items.AddAsync(new Item
            {
                BarCode = "00000004",
                Place = "B/1",
                AddDate = DateTime.Now,
                Product = product1,
                ExpirationDate = DateTime.Now.AddDays(6)
            });
            product1.Amount++;

            await db.Items.AddAsync(new Item
            {
                BarCode = "00000005",
                Place = "A/1",
                AddDate = DateTime.Now,
                Product = product2,
                ExpirationDate = DateTime.Now.AddDays(3)
            });
            product2.Amount++;

            await db.Items.AddAsync(new Item
            {
                BarCode = "00000006",
                Place = "A/12",
                AddDate = DateTime.Now,
                Product = product2,
                ExpirationDate = DateTime.Now.AddDays(3)
            });
            product2.Amount++;

            await db.Items.AddAsync(new Item
            {
                BarCode = "00000007",
                Place = "B/1",
                AddDate = DateTime.Now,
                Product = product2,
                ExpirationDate = DateTime.Now.AddDays(3)
            });
            product2.Amount++;

            await db.Items.AddAsync(new Item
            {
                BarCode = "00000008",
                Place = "A/1",
                AddDate = DateTime.Now,
                Product = product3,
                ExpirationDate = DateTime.Now.AddDays(3)
            });
            product3.Amount++;

            await db.SaveChangesAsync();
        }

        public static async Task FillWithInventories(ApplicationDbContext db)
        {
            await db.Inventories.AddAsync(new Inventory
            {
                State = InventoryState.Inactive,
                Type = InventoryType.Full,
                Description = "Pełna inwentaryzacja roczna 2021.",
                StartDate = DateTime.Now.AddDays(3),
                PlanedEndDate = DateTime.Now.AddDays(13),
            });

            await db.Inventories.AddAsync(new Inventory
            {
                State = InventoryState.Inactive,
                Type = InventoryType.Partial,
                Description = "Inwentaryzacja częściowa regały B1/2, A12, A2/18.",
                StartDate = DateTime.Now.AddDays(13),
            });

            await db.Inventories.AddAsync(new Inventory
            {
                State = InventoryState.Finished,
                Type = InventoryType.Full,
                Description = "Pełna inwentaryzacja roczna 2020.",
                StartDate = DateTime.Now.AddDays(-13),
                PlanedEndDate = DateTime.Now.AddDays(-3),
            });

            await db.Inventories.AddAsync(new Inventory
            {
                State = InventoryState.Active,
                Type = InventoryType.Partial,
                Description = "Inwentaryzacja częściowa towarów z kategori opatrunki.",
                StartDate = DateTime.Now.AddDays(30),
                PlanedEndDate = DateTime.Now.AddDays(40),
            });

            await db.SaveChangesAsync();
        }

    }
}
