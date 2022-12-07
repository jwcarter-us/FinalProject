using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using FinalProject.Extentions;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
            CartVM = new CartVM()
            {
                OrderHeader = new Models.OrderHeader(),
                AlbumList = new List<Album>()

            };
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

        public IActionResult AddToCart(int serviceId)
        {
            List<int> sessionList = new List<int>();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SD.SessionCart)))
            {
                sessionList.Add(serviceId);
                HttpContext.Session.SetObject(SD.SessionCart, sessionList);

            }
            else
            {
                sessionList = HttpContext.Session.GetObject<List<int>> (SD.SessionCart);
                if(!sessionList.Contains(serviceId))
                {
                    sessionList.Add(serviceId);
                    HttpContext.Session.SetObject(SD.SessionCart, sessionList);
                }
            }
            return RedirectToAction(nameof(Index));
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
        [BindProperty]
        public CartVM CartVM { get; set; }

        public async Task<IActionResult> checkout()
        {
            var applicationDbContext = _context.Album.Include(a => a.Artist);
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                List<int> albumList = new List<int>();
                albumList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                foreach (int albumId in albumList)
                {
                    var album = await _context.Album
                    .Include(a => a.Artist)
                    .FirstOrDefaultAsync(m => m.Id == albumId);
                    CartVM.AlbumList.Add(album);
                }
            }
            return View(CartVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}