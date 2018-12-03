using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TourServer.Dto;
using TourServer.Models;
using TourServer.ServicesInterface;

namespace TourServer.Services
{
    public class TourServices : ITourService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _environment;

        public TourServices(IUnitOfWork unitOfWork, IHostingEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        public async Task<Response<int>> CreateAsync(CreateTourDto createTourDto)
        {
            var response = new Response<int>();
            var tour = new Tour();
            var hotel = await _unitOfWork.HotelRepository.Find(h => h.Id == createTourDto.HotelId, hot => hot.City);
            tour.Hotel = hotel;
            tour.CitId = hotel.CityId;
            tour.CountrId = hotel.City.CountryId;
            tour.Name = createTourDto.Name;
            tour.Price = createTourDto.Price;
            await _unitOfWork.TourRepository.Create(tour);
            await _unitOfWork.Commit();
            response.Data = tour.Id;
            return response;
        }

        public async Task<Response<bool>> UploadFile(int tourId, IFormFileCollection files)
        {
            var response = new Response<bool>();
            var tour = await _unitOfWork.TourRepository.Find(t => t.Id == tourId);
            var list = new List<FileModel>();
            if (files == null || files.Count == 0)
            {
                var name = Guid.NewGuid().ToString();
                FileModel fileModel = new FileModel()
                {
                    Name = name,
                    Path = "http://asa.az/uploads/default.png",
                    Tour = tour
                };
                await _unitOfWork.FileRepository.Create(fileModel);
            }

            var folder = Path.Combine(_environment.WebRootPath, "files");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            for (int i = 0; i < files.Count; i++)
            {
                if (files[i] != null)
                {
                    var guid = Guid.NewGuid();
                    string path = Path.Combine(folder, guid + files[i].FileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await files[i].CopyToAsync(fileStream);
                    }

                    FileModel fileModel = new FileModel()
                    {
                        Name = files[i].FileName,
                        Path = "https://tourserver20181201023405.azurewebsites.net/files/" + guid + files[i].FileName,
                        Tour = tour
                    };
                    list.Add(fileModel);
                }
                await _unitOfWork.FileRepository.CreateRange(list);
            }

            await _unitOfWork.Commit();
            response.Data = true;
            return response;
        }

        public async Task<Response<GetToursResponseDto>> GetAllTour(GetToursRequestDto requestDto)
        {
            var response = new Response<GetToursResponseDto>(); ;
            if (requestDto.Search == null)
            {
                requestDto.Search = string.Empty;
            }

            Predicate<Tour> filter = tour => true;
            if (requestDto.CountryId != 0 && requestDto.CityId == 0)
            {
                filter = tour => tour.CountrId == requestDto.CountryId;
            }

            if (requestDto.CountryId != 0 && requestDto.CityId != 0)
            {
                filter = tour => tour.CountrId == requestDto.CountryId && tour.CitId == requestDto.CityId;
            }

            Predicate<Tour> predicate = t => filter(t) && !t.isDeleted && t.Price > requestDto.Min && t.Price < requestDto.Max && t.Name.Contains(requestDto.Search);
            var count = await _unitOfWork
                .TourRepository
                .Count(tour => predicate(tour));

            int skip = (requestDto.Page - 1) * requestDto.Size;

            var listTour = await _unitOfWork.TourRepository.Get(tour => predicate(tour), skip, requestDto.Size, tour => tour.FileModels);
            var responseList = new List<ToursRespoonse>();
            foreach (var elem in listTour)
            {
                var item = new ToursRespoonse
                {
                    Id = elem.Id,
                    Name = elem.Name,
                    Price = elem.Price,
                    Path = elem.FileModels.FirstOrDefault()?.Path
                };
                responseList.Add(item);
            }

            response.Data = new GetToursResponseDto()
            {
                Count = count,
                ListTour = responseList
            };
            return response;
        }

        public async Task<Response<GetTourResponseDto>> GetTourById(int id)
        {
            var response = new Response<GetTourResponseDto>();
            var tour = await _unitOfWork.TourRepository.Find(t => t.Id == id, tour1 => tour1.Hotel, tour2 => tour2.Hotel.City,
                tour3 => tour3.Hotel.City.Country, tour4 => tour4.FileModels);

            if (tour == null)
            {
                response.Error = new Error("Tour not found");
            }

            var dto = new GetTourResponseDto()
            {
                Id = tour.Id,
                Name = tour.Name,
                Rating = tour.Hotel.Rating,
                Adress = tour.Hotel.Address,
                CityName = tour.Hotel.City.Name,
                CountyName = tour.Hotel.City.Country.Name,
                Description = tour.Hotel.Description,
                Price = tour.Price,
                HotelName = tour.Hotel.Name,
                FileModels = tour.FileModels.Select(f => f.Path).ToList()
            };
            response.Data = dto;

            return response;
        }

        public async Task<Response<bool>> DeleteTour(int id)
        {
            var response = new Response<bool>();
            var tour = await _unitOfWork.TourRepository.Find(t => t.Id == id);

            if (tour == null)
            {
                response.Error = new Error("Tour not found");
                return response;
            }

            tour.isDeleted = true;
            _unitOfWork.TourRepository.Update(tour);
            await _unitOfWork.Commit();
            response.Data = true;
            return response;
        }
    }
}
