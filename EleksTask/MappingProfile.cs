using AutoMapper;
using TourServer.Dto;
using TourServer.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Country, CountryResponseDto>();

        CreateMap<City, CityResponseDto>();

        CreateMap<CreateOrderRequstDto, Order>();

        CreateMap<Order, GetOrdersResponseDto>();

        CreateMap<Hotel, HotelResponseDto>();
    }
}