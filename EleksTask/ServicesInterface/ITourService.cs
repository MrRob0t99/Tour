using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TourServer.Dto;

namespace TourServer.ServicesInterface
{
    public interface ITourService
    {
        Task<Response<int>> CreateAsync(CreateTourDto createTourDto);

        Task<Response<bool>> UploadFile(int tourId, IFormFileCollection files);

        Task<Response<GetToursResponseDto>> GetAllTour(GetToursRequestDto requestDto);

        Task<Response<GetTourResponseDto>> GetTourById(int id);

        Task<Response<bool>> DeleteTour(int id);
    }
}
