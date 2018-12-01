namespace TourServer.Models
{
    public class FileModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public Tour Tour { get; set; }

        public int TourId { get; set; }
    }
}
