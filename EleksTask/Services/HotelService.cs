using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EleksTask;
using Microsoft.EntityFrameworkCore;
using TourServer.Dto;
using TourServer.Models;
using TourServer.ServicesInterface;

namespace TourServer.Services
{
    public class HotelService : IHotelService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        public HotelService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<int>> CreateHotel(HotelDto hotelDto)
        {
            var response = new Response<int>();
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == hotelDto.CityId);
            var hotel = new Hotel()
            {
                Name = hotelDto.Name,
                City = city,
                Address = hotelDto.Address,
                Rating = hotelDto.Rating,
                Description = hotelDto.Description,
                CityId = city.Id
            };

            await _context.Hotels.AddAsync(hotel);
            await _context.SaveChangesAsync();

            response.Data = hotel.Id;
            return response;
        }

        public async Task<Response<List<HotelResponseDto>>> GetHotels(int id)
        {
            var response = new Response<List<HotelResponseDto>>();
            var hotels = await _context.Hotels.Where(h => h.City.Id == id).ToListAsync();
            var dto = _mapper.Map<List<HotelResponseDto>>(hotels);
            response.Data = dto;
            return response;
        }
    }
}
