using System.Collections.Generic;

namespace TourServer.Models
{
    public class Tour
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public List<FileModel> FileModels { get; set; }

        public Hotel Hotel { get; set; }

        public int HotelId { get; set; }

        public int CountrId { get; set; }

        public int CitId { get; set; }
    }
}
