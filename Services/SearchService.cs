﻿using invMed.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data.Enums;

namespace invMed.Services
{
    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext _db;

        public SearchService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<SearchDto>> Search(string searchValue)
        {
            var products = await _db.Products.Where(x => x.Name.Contains(searchValue)).ToListAsync();
            var items = await _db.Items.Where(x => x.BarCode.Contains(searchValue)).Where(x => x.Type != ItemTypeEnum.Over).ToListAsync();
            var searchDtos = new List<SearchDto>();
            foreach (var product in products)
            {
                searchDtos.Add(new SearchDto()
                {
                    Id = product.Id,
                    Type = SearchTypeEnum.Product,
                    Name = product.Name,
                    Category = product.Category
                });
            }
            foreach (var item in items)
            {
                searchDtos.Add(new SearchDto()
                {
                    Id = item.Id,
                    Type = SearchTypeEnum.Item,
                    Name = item.Product.Name,
                    Category = item.Product.Category,
                    Barcode = item.BarCode
                });
            }
            return searchDtos;
        }
    }
}
