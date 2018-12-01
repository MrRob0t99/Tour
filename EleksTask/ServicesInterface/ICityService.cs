using System.Collections.Generic;
using System.Threading.Tasks;
using TourServer.Dto;

namespace TourServer.ServicesInterface
{
    public interface ICityService
    {
        Task<Response<int>> CreateCity(CreateCityRequestDto cityRequestDto);

        Task<Response<List<CityResponseDto>>> GetCity(int id);
    }
}
