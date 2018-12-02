using System.Collections.Generic;

namespace TourServer.Models
{
    public class Country : BaseEntity
    {

        public string Name { get; set; }

        public List<City> Cities { get; set; }

        public List<Tour> Tours { get; set; }
    }
}
