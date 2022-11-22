using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string AlbumName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public byte[]? AlbumArt { get; set; }
        public float Price { get; set; }
        [ForeignKey("Artist")]
        public int? ArtistID { get; set; }
        public Artist? Artist { get; set; }
    }
}
