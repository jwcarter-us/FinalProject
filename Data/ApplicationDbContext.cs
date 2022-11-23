using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FinalProject.Models;

namespace FinalProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FinalProject.Models.Artist> Artist { get; set; }
        public DbSet<FinalProject.Models.Album> Album { get; set; }
        public DbSet<FinalProject.Models.ArtistAlbums> ArtistAlbums { get; set; }
    }
}