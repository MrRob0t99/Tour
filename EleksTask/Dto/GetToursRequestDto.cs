namespace TourServer.Dto
{
    public class GetToursRequestDto
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 9;
        public string Search { get; set; } = string.Empty;
        public int Min { get; set; } = 0;
        public int Max { get; set; } = int.MaxValue;
        public int CityId { get; set; }
        public int CountryId { get; set; }
    }
}
