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
    public class CountryService : ICountryService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        public CountryService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<Response<int>> CreateCountry(string name)
        {
            var response = new Response<int>();
            if (_context.Countries.Any(c => c.Name == name))
            {
                response.Error = new Error($"Country with name {name} already exist");
                return response;
            }

            var country = new Country();
            country.Name = name;
            await _context.Countries.AddAsync(country);
            await _context.SaveChangesAsync();
            response.Data = country.Id;
            return response;
        }

        public async Task<Response<List<CountryResponseDto>>> GetCountry()
        {
            var response = new Response<List<CountryResponseDto>>();
            var countries = await _context.Countries.ToListAsync();
            var dto = _mapper.Map<List<CountryResponseDto>>(countries);
            response.Data = dto;
            return response;
        }
    }
}
