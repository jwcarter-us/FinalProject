using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class ArtistAlbumsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArtistAlbumsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ArtistAlbums
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ArtistAlbums.Include(a => a.Album).Include(a => a.Artist);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ArtistAlbums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ArtistAlbums == null)
            {
                return NotFound();
            }

            var artistAlbums = await _context.ArtistAlbums
                .Include(a => a.Album)
                .Include(a => a.Artist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artistAlbums == null)
            {
                return NotFound();
            }

            return View(artistAlbums);
        }

        // GET: ArtistAlbums/Create
        public IActionResult Create()
        {
            ViewData["AlbumID"] = new SelectList(_context.Album, "Id", "AlbumName");
            ViewData["ArtistID"] = new SelectList(_context.Artist, "Id", "Name");
            return View();
        }

        // POST: ArtistAlbums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArtistID,AlbumID")] ArtistAlbums artistAlbums)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artistAlbums);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlbumID"] = new SelectList(_context.Album, "Id", "AlbumName", artistAlbums.AlbumID);
            ViewData["ArtistID"] = new SelectList(_context.Artist, "Id", "Name", artistAlbums.ArtistID);
            return View(artistAlbums);
        }

        // GET: ArtistAlbums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ArtistAlbums == null)
            {
                return NotFound();
            }

            var artistAlbums = await _context.ArtistAlbums.FindAsync(id);
            if (artistAlbums == null)
            {
                return NotFound();
            }
            ViewData["AlbumID"] = new SelectList(_context.Album, "Id", "AlbumName", artistAlbums.AlbumID);
            ViewData["ArtistID"] = new SelectList(_context.Artist, "Id", "Name", artistAlbums.ArtistID);
            return View(artistAlbums);
        }

        // POST: ArtistAlbums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArtistID,AlbumID")] ArtistAlbums artistAlbums)
        {
            if (id != artistAlbums.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artistAlbums);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistAlbumsExists(artistAlbums.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlbumID"] = new SelectList(_context.Album, "Id", "AlbumName", artistAlbums.AlbumID);
            ViewData["ArtistID"] = new SelectList(_context.Artist, "Id", "Name", artistAlbums.ArtistID);
            return View(artistAlbums);
        }

        // GET: ArtistAlbums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ArtistAlbums == null)
            {
                return NotFound();
            }

            var artistAlbums = await _context.ArtistAlbums
                .Include(a => a.Album)
                .Include(a => a.Artist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artistAlbums == null)
            {
                return NotFound();
            }

            return View(artistAlbums);
        }

        // POST: ArtistAlbums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ArtistAlbums == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ArtistAlbums'  is null.");
            }
            var artistAlbums = await _context.ArtistAlbums.FindAsync(id);
            if (artistAlbums != null)
            {
                _context.ArtistAlbums.Remove(artistAlbums);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistAlbumsExists(int id)
        {
            return _context.ArtistAlbums.Any(e => e.Id == id);
        }
    }
}
