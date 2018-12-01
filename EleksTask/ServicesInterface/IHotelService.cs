using System.Collections.Generic;
using System.Threading.Tasks;
using TourServer.Dto;

namespace TourServer.ServicesInterface
{
    public interface IHotelService
    {
        Task<Response<int>> CreateHotel(HotelDto hotelDto);

        Task<Response<List<HotelResponseDto>>> GetHotels(int id);
    }
}
