

using System.Collections.Generic;

namespace TourServer.Dto
{
    public class GetTourResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public List<string> FileModels { get; set; }

        public string HotelName { get; set; }

        public string CountyName { get; set; }

        public string CityName { get; set; }

        public double Rating { get; set; }

        public string Adress { get; set; }

        public string Description { get; set; }
    }
}
