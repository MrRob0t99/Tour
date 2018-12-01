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
    public class CityService :ICityService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public CityService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<Response<int>> CreateCity(CreateCityRequestDto cityRequestDto)
        {
            var response = new Response<int>();

            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == cityRequestDto.CountryId);
            if (country == null || _context.Cities.Any(c => c.Name == cityRequestDto.CityName))
            {
                response.Error = new Error($"City with name {cityRequestDto.CityName}");
                return response;
            }

            var city = new City();
            city.Name = cityRequestDto.CityName;
            city.Country = country;
            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();
            response.Data = city.Id;
            return response;
        }

        public async Task<Response<List<CityResponseDto>>> GetCity(int id)
        {
            var response = new Response<List<CityResponseDto>>();
            var cities = await _context.Cities.Where(c => c.CountryId == id).ToListAsync();
            var citiesDto = _mapper.Map<List<CityResponseDto>>(cities);
            response.Data = citiesDto;
            return response;
        }
    }
}
