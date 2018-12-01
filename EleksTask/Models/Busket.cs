using EleksTask.Models;

namespace TourServer.Models
{
    public class Busket
    {
        public int Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string ApplicationUserId { get; set; }

        public Tour Tour { get; set; }

        public int TourId { get; set; }
    }
}
