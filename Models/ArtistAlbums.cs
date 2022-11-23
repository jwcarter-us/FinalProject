using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class ArtistAlbums
    {
        public int Id { get; set; }
        [ForeignKey("Artist")]
        public int? ArtistID { get; set; }
        public Artist? Artist { get; set; }
        [ForeignKey("Album")]
        public int? AlbumID { get; set; }
        public Album? Album { get; set; }

    }
}
