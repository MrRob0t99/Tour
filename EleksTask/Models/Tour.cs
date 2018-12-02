using System.Collections.Generic;

namespace TourServer.Models
{
    public class Tour: BaseEntity
    {

        public string Name { get; set; }

        public double Price { get; set; }

        public List<FileModel> FileModels { get; set; }

        public Hotel Hotel { get; set; }

        public int HotelId { get; set; }

        public bool isDeleted { get; set; }

        public int CountryId { get; set; }

        public int CitId { get; set; }
    }
}
