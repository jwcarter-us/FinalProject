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
        public IActionResult Remove(int serviceId)
        {
            List<int> sessionList = new List<int>();
            sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
            sessionList.Remove(serviceId);
            HttpContext.Session.SetObject(SD.SessionCart, sessionList);

            return RedirectToAction(nameof(checkout));

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

        public async Task<IActionResult> RingUp()
        {
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Order")]
        public async Task<IActionResult> OrderPOST()
        {
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
            if(!ModelState.IsValid)
            {
                return View(CartVM);
            }
            else
            {
                CartVM.OrderHeader.OrderDate = DateTime.Now;
                CartVM.OrderHeader.Status = SD.StatusSubmitted;
                CartVM.OrderHeader.AlbumCount = CartVM.AlbumList.Count;
                _context.Add(CartVM.OrderHeader);
                await _context.SaveChangesAsync();

                foreach(var item in CartVM.AlbumList)
                {
                    OrderDetails orderDetails = new OrderDetails
                    {
                        AlbumId = item.Id,
                        OrderHeaderId = CartVM.OrderHeader.Id,
                        ServiceName = item.AlbumName,
                        Price = (int)item.Price
                    };
                    _context.Add(orderDetails);
                    
                }
                await _context.SaveChangesAsync();
                HttpContext.Session.SetObject(SD.SessionCart, new List<int>());
                return RedirectToAction("OrderConfirmation", new { id = CartVM.OrderHeader.Id});

            }
            
        }

        public IActionResult OrderConfirmation(int id)
        {
            HttpContext.Session.SetObject(SD.SessionCart, new List<int>());
            return View(id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}