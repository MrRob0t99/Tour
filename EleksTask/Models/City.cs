using System.Collections.Generic;

namespace TourServer.Models
{
    public class City : BaseEntity
    {

        public string Name { get; set; }

        public Country Country { get; set; }

        public int CountryId { get; set; }

        public List<Tour> Tours { get; set; }

        public List<Hotel> Hotels { get; set; }
    }
}
