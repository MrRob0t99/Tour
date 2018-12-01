namespace TourServer.Dto
{
    public class GetBusketResponseDto
    {
        public int TourId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Path { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string HotelName { get; set; }
    }
}
