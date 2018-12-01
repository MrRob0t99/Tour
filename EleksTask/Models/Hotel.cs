namespace TourServer.Models
{
    public class Hotel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Rating { get; set; }

        public string Address { get; set; }

        public City City { get; set; }

        public int CityId { get; set; }
    }
}
