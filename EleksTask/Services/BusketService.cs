using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EleksTask;
using Microsoft.EntityFrameworkCore;
using TourServer.Dto;
using TourServer.Models;
using TourServer.ServicesInterface;

namespace TourServer.Services
{
    public class BusketService : IBusketService
    {
        private readonly ApplicationContext _context;

        public BusketService(ApplicationContext context)
        {
            _context = context; 
        }

        public async Task<Response<int>> AddProduct(AddProductToBuseketDto dto, string userId)
        {
            var response = new Response<int>();
            if (_context.Buskets.Any(b => b.ApplicationUserId == userId && b.TourId == dto.TourId))
            {
                response.Error = new Error("Tour already added to busket");
                return response;
            }

            var busket = new Busket()
            {
                TourId = dto.TourId,
                ApplicationUserId = userId
            };
            await _context.Buskets.AddAsync(busket);
            await _context.SaveChangesAsync();
            response.Data = busket.Id;
            return response;
        }


        public async Task<Response<List<GetBusketResponseDto>>> GetBusket(string userId)
        {
            var response = new Response<List<GetBusketResponseDto>>();
            var busketDto = await _context
                .Buskets
                .Where(c => c.ApplicationUserId == userId)
                .Include(c => c.Tour)
                .ThenInclude(c => c.Hotel)
                .ThenInclude(c => c.City)
                .ThenInclude(c => c.Country)
                .Select(t => new GetBusketResponseDto()
                {
                    TourId = t.TourId,
                    Name = t.Tour.Name,
                    Price = t.Tour.Price,
                    Path = t.Tour.FileModels.FirstOrDefault().Path,
                    Address = t.Tour.Hotel.Address,
                    CityName = t.Tour.Hotel.City.Name,
                    CountryName = t.Tour.Hotel.City.Country.Name,
                    HotelName = t.Tour.Hotel.Name
                })
                .ToListAsync();

            response.Data = busketDto;

            return response;
        }

        public async Task<Response<int>> GetCount(string userId)
        {
            var response = new Response<int>();
            var count = await _context
                .Buskets
                .Where(c => c.ApplicationUserId == userId).CountAsync();
            response.Data = count;
            return response;
        }

        public async Task<Response<bool>> DeleteTour(int tourId,string userId)
        {
            var response = new Response<bool>();
            var busket = await _context.Buskets.FirstOrDefaultAsync(b => b.ApplicationUserId == userId && b.TourId == tourId);

            if (busket == null)
            {
                response.Error = new Error("Not found busket item");
                return response;
            }

            _context.Buskets.Remove(busket);
            await _context.SaveChangesAsync();
            response.Data = true;
            return response;
        }
    }
}
