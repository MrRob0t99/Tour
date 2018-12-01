namespace TourServer.Dto
{
    public class GetCountTourDto
    {
        public string Search { get; set; } = string.Empty;
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
