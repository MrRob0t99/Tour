using System.Collections.Generic;
using System.Threading.Tasks;
using TourServer.Dto;

namespace TourServer.ServicesInterface
{
    public interface ICountryService
    {
        Task<Response<int>> CreateCountry(string name);

        Task<Response<List<CountryResponseDto>>> GetCountry();
    }
}
