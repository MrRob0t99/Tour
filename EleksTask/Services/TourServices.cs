using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EleksTask;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TourServer.Dto;
using TourServer.Models;
using TourServer.ServicesInterface;

namespace TourServer.Services
{
    public class TourServices : ITourService
    {
        private readonly ApplicationContext _context;
        private readonly IHostingEnvironment _environment;

        public TourServices(ApplicationContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<Response<int>> CreateAsync(CreateTourDto createTourDto)
        {
            var response = new Response<int>();
            var tour = new Tour();
            var hotel = await _context.Hotels.Include(h => h.City).FirstOrDefaultAsync(c => c.Id == createTourDto.HotelId);
            tour.Hotel = hotel;
            tour.CitId = hotel.CityId;
            tour.CountrId = hotel.City.CountryId;
            tour.Name = createTourDto.Name;
            tour.Price = createTourDto.Price;
            await _context.Tours.AddAsync(tour);
            await _context.SaveChangesAsync();
            response.Data = tour.Id;
            return response;
        }

        public async Task<Response<bool>> UploadFile(int tourId, IFormFileCollection files)
        {
            var response = new Response<bool>();
            var tour = await _context.Tours.FirstOrDefaultAsync(t => t.Id == tourId);
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
                await _context.FileModels.AddAsync(fileModel);
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
                    string path = Path.Combine(folder, guid+files[i].FileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await files[i].CopyToAsync(fileStream);
                    }

                    FileModel fileModel = new FileModel()
                    {
                        Name = files[i].FileName,
                        Path = "https://tourserver20181201023405.azurewebsites.net/files/" + guid+ files[i].FileName,
                        Tour = tour
                    };
                    list.Add(fileModel);
                }
                await _context.FileModels.AddRangeAsync(list);
            }

            await _context.SaveChangesAsync();
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
            var count = await _context.Tours
                .Where(t => t.Price > requestDto.Min && t.Price < requestDto.Max && t.Name.Contains(requestDto.Search) && filter(t))
                .CountAsync();

            int skip = (requestDto.Page - 1) * requestDto.Size;

            var listTour = await _context.Tours
                .Skip(skip)
                .Take(requestDto.Size)
                .Include(t => t.FileModels)
                .Where(t => t.Price > requestDto.Min && t.Price < requestDto.Max && t.Name.Contains(requestDto.Search) && filter(t))
                .Select(t => new ToursRespoonse { Id = t.Id, Name = t.Name, Price = t.Price, Path = t.FileModels.FirstOrDefault().Path })
                .ToListAsync();
            response.Data = new GetToursResponseDto()
            {
                Count = count,
                ListTour = listTour
            };
            return response;
        }

        public async Task<Response<GetTourResponseDto>> GetTourById(int id)
        {
            var response = new Response<GetTourResponseDto>();
            var tour = await _context.Tours
                .Where(t => t.Id == id)
                .Include(t => t.FileModels)
                .Include(t => t.Hotel)
                .ThenInclude(h => h.City)
                .ThenInclude(c => c.Country)
                .FirstOrDefaultAsync();

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
            var tour = await _context.Tours.FirstOrDefaultAsync(t => t.Id == id);

            if (tour == null)
            {
                response.Error = new Error("Tour not found");
                return response;
            }

            _context.Tours.Remove(tour);
            await _context.SaveChangesAsync();
            response.Data = true;
            return response;
        }
    }
}
