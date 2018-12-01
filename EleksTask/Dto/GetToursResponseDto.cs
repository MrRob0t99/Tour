using System.Collections.Generic;

namespace TourServer.Dto
{
    public class GetToursResponseDto
    {
        public List<ToursRespoonse> ListTour { get; set; }

        public int Count { get; set; }
    }
}
