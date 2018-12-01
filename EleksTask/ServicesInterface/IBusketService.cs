using System.Collections.Generic;
using System.Threading.Tasks;
using TourServer.Dto;

namespace TourServer.ServicesInterface
{
    public interface IBusketService
    {
        Task<Response<int>> AddProduct(AddProductToBuseketDto dto, string userId);

        Task<Response<List<GetBusketResponseDto>>> GetBusket(string userId);

        Task<Response<int>> GetCount(string userId);

        Task<Response<bool>> DeleteTour(int tourId, string userId);

    }
}
