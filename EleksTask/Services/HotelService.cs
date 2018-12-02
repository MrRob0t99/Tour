using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TourServer.Dto;
using TourServer.Models;
using TourServer.ServicesInterface;

namespace TourServer.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HotelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<int>> CreateHotel(HotelDto hotelDto)
        {
            var response = new Response<int>();
            var city = await _unitOfWork.CityRepository.Find(c => c.Id == hotelDto.CityId);
            var hotel = new Hotel()
            {
                Name = hotelDto.Name,
                City = city,
                Address = hotelDto.Address,
                Rating = hotelDto.Rating,
                Description = hotelDto.Description,
                CityId = city.Id
            };

            await _unitOfWork.HotelRepository.Create(hotel);
            await _unitOfWork.Commit();

            response.Data = hotel.Id;
            return response;
        }

        public async Task<Response<List<HotelResponseDto>>> GetHotels(int id)
        {
            var response = new Response<List<HotelResponseDto>>();
            var hotels = await _unitOfWork.HotelRepository.Get(h => h.City.Id == id);
            var dto = _mapper.Map<List<HotelResponseDto>>(hotels);
            response.Data = dto;
            return response;
        }
    }
}
