using invMed.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace invMed.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchDto>> Search(string searchValue);
    }
}