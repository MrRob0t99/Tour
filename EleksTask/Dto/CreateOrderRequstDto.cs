using System;
using TourServer.Models;

namespace TourServer.Dto
{
    public class CreateOrderRequstDto
    {
        public int TourId { get; set; }

        public CityEnum CityEnum { get; set; }

        public TypeOfFood TypeOfFood { get; set; }

        public string Phone { get; set; }

        public DateTime StartDate { get; set; }

        public int CountPeople { get; set; }

        public int Duration { get; set; }


        public decimal TotalPrice { get; set; }
    }
}
