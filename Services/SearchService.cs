using invMed.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data.Enums;
using Microsoft.Extensions.Logging;

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
            var items = await _db.Items.Where(x => x.BarCode.Contains(searchValue) && x.Type != ItemTypeEnum.Over).ToListAsync();
            var searchDtos = new List<SearchDto>();
            foreach (var product in products)
            {
                var searchDto = new SearchDto(product);
                searchDtos.Add(searchDto);
            }
            foreach (var item in items)
            {
                var searchDto = new SearchDto(item);
                searchDtos.Add(searchDto);
            }
            return searchDtos;
        }
    }
}
