using System.Threading.Tasks;
using EleksTask.Models;
using TourServer.Models;

namespace TourServer
{
    public interface IUnitOfWork
    {
        IGenericRepository<Tour> TourRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }
        IGenericRepository<Country> CountryRepository { get; }
        IGenericRepository<City> CityRepository { get; }
        IGenericRepository<Hotel> HotelRepository { get; }
        IGenericRepository<FileModel> FileRepository { get;  }
        Task Commit();
        void Dispose();
    }
}
