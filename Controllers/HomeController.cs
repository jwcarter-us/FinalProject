using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> GetAlbumArt(int id)
        {
            var album = await _context.Album
                    .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {

                return NotFound();
            }
            var imageData = album.AlbumArt;

            return File(imageData, "image/jpg");
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Album.Include(a => a.Artist);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}