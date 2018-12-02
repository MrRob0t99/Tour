using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TourServer.Dto;
using TourServer.Models;
using TourServer.ServicesInterface;

namespace TourServer.Services
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CountryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<Response<int>> CreateCountry(string name)
        {
            var response = new Response<int>();
            if (await _unitOfWork.CountryRepository.Any(c => c.Name == name))
            {
                response.Error = new Error($"Country with name {name} already exist");
                return response;
            }

            var country = new Country();
            country.Name = name;
            await _unitOfWork.CountryRepository.Create(country);
            await _unitOfWork.Commit();
            response.Data = country.Id;
            return response;
        }

        public async Task<Response<List<CountryResponseDto>>> GetCountry()
        {
            var response = new Response<List<CountryResponseDto>>();
            var countries = await _unitOfWork.CountryRepository.Get();
            var dto = _mapper.Map<List<CountryResponseDto>>(countries);
            response.Data = dto;
            return response;
        }
    }
}
