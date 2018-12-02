
using System;
using EleksTask.Models;

namespace TourServer.Models
{
    public class Order : BaseEntity
    {

        public Tour Tour { get; set; }

        public int TourId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string ApplicationUserId { get; set; }
        
        public CityEnum CityEnum { get; set; }

        public TypeOfFood TypeOfFood { get; set; }

        public DateTime StartDate { get; set; }

        public int CountPeople { get; set; }

        public int Duration { get; set; }

        public decimal TotalPrice { get; set; }

        public string Phone { get; set; }

        public bool IsConfirmed { get; set; }

        public string Path { get; set; }
    }
}
