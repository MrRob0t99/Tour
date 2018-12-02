using EleksTask.Models;

namespace TourServer.Models
{
    public class Busket : BaseEntity
    {

        public ApplicationUser ApplicationUser { get; set; }

        public string ApplicationUserId { get; set; }

        public Tour Tour { get; set; }

        public int TourId { get; set; }
    }
}
