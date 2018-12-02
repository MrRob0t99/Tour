namespace TourServer.Models
{
    public class FileModel : BaseEntity
    {

        public string Name { get; set; }

        public string Path { get; set; }

        public Tour Tour { get; set; }

        public int TourId { get; set; }
    }
}
