namespace FinalProject.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[]? ArtistPicture { get; set; }
        public DateTime Birthday { get; set; }
        public string BirthPlace { get; set; }
        public string Label { get; set; }
    }
}
