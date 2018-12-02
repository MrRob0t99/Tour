using System.ComponentModel.DataAnnotations;

namespace TourServer.Dto
{
    public class HotelDto
    {
        public string Name { get; set; }

        public double Rating { get; set; }

        [MaxLength(25)]
        public string Address { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }

        public int CityId { get; set; }
    }
}
