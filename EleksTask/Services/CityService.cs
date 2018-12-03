using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TourServer.Dto;
using TourServer.Models;
using TourServer.ServicesInterface;

namespace TourServer.Services
{
    public class CityService :ICityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<Response<int>> CreateCity(CreateCityRequestDto cityRequestDto)
        {
            var response = new Response<int>();

            var country = await _unitOfWork.CountryRepository.Find(c => c.Id == cityRequestDto.CountryId);
            if (country == null || await _unitOfWork.CityRepository.Any(c => c.Name == cityRequestDto.CityName))
            {
                response.Error = new Error($"City with name {cityRequestDto.CityName}");
                return response;
            }

            var city = new City
            {
                Name = cityRequestDto.CityName,
                Country = country
            };

            await _unitOfWork.CityRepository.Create(city);
            await _unitOfWork.Commit();
            response.Data = city.Id;
            return response;
        }

        public async Task<Response<List<CityResponseDto>>> GetCity(int id)
        {
            var response = new Response<List<CityResponseDto>>();
            var cities = await _unitOfWork.CityRepository.Get(c => c.CountryId == id);
            var citiesDto = _mapper.Map<List<CityResponseDto>>(cities);
            response.Data = citiesDto;
            return response;
        }
    }
}
