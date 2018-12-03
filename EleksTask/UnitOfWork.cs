using System.Threading.Tasks;
using EleksTask;
using TourServer.Models;

namespace TourServer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _dbContext;
        private GenericRepository<Tour> _tourRepository;
        private GenericRepository<Order> _orderRepository;
        private GenericRepository<Country> _countryRepository;
        private GenericRepository<City> _cityRepository;
        private GenericRepository<Hotel> _hotelRepository;
        private GenericRepository<FileModel> _fileRepository;

        IGenericRepository<Hotel> ApplicationUser { get; }
        public IGenericRepository<Tour> TourRepository
        {
            get
            {

                if (this._tourRepository == null)
                {
                    this._tourRepository = new GenericRepository<Tour>(_dbContext);
                }
                return _tourRepository;
            }
        }
        public IGenericRepository<FileModel> FileRepository
        {
            get
            {

                if (this._fileRepository == null)
                {
                    this._fileRepository = new GenericRepository<FileModel>(_dbContext);
                }
                return _fileRepository;
            }
        }
        public IGenericRepository<Hotel> HotelRepository
        {
            get
            {

                if (this._hotelRepository == null)
                {
                    this._hotelRepository = new GenericRepository<Hotel>(_dbContext);
                }
                return _hotelRepository;
            }
        }
        public IGenericRepository<City> CityRepository
        {
            get
            {

                if (this._cityRepository == null)
                {
                    this._cityRepository = new GenericRepository<City>(_dbContext);
                }
                return _cityRepository;
            }
        }
        public IGenericRepository<Order> OrderRepository
        {
            get
            {

                if (this._orderRepository == null)
                {
                    this._orderRepository = new GenericRepository<Order>(_dbContext);
                }
                return _orderRepository;
            }
        }

        public IGenericRepository<Country> CountryRepository
        {
            get
            {

                if (this._countryRepository == null)
                {
                    this._countryRepository = new GenericRepository<Country>(_dbContext);
                }
                return _countryRepository;
            }
        }

        public UnitOfWork(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
        {
           await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }

}
