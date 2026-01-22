using System.Collections.Generic;
using System.Threading.Tasks;
using IsTakipWpf.Models;

namespace IsTakipWpf.Services
{
    public interface ILocationService
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<IEnumerable<string>> GetDistrictsAsync(string cityName);
    }
}
