using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PatoRestaurant.Data;
using PatoRestaurant.Models;

namespace PatoRestaurant.Controllers
{
    public class SocialEventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SocialEventController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SocialEvent
        public async Task<IActionResult> Index()
        {
              return View(await _context.SocialEvents.ToListAsync());
        }

        // GET: SocialEvent/Details/5
        public async Task<IActionResult> Details(ushort? id)
        {
            if (id == null || _context.SocialEvents == null)
            {
                return NotFound();
            }

            var socialEvent = await _context.SocialEvents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (socialEvent == null)
            {
                return NotFound();
            }

            return View(socialEvent);
        }

        // GET: SocialEvent/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SocialEvent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,EventDate,Image")] SocialEvent socialEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(socialEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(socialEvent);
        }

        // GET: SocialEvent/Edit/5
        public async Task<IActionResult> Edit(ushort? id)
        {
            if (id == null || _context.SocialEvents == null)
            {
                return NotFound();
            }

            var socialEvent = await _context.SocialEvents.FindAsync(id);
            if (socialEvent == null)
            {
                return NotFound();
            }
            return View(socialEvent);
        }

        // POST: SocialEvent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ushort id, [Bind("Id,Name,Description,EventDate,Image")] SocialEvent socialEvent)
        {
            if (id != socialEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(socialEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SocialEventExists(socialEvent.Id))
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
            return View(socialEvent);
        }

        // GET: SocialEvent/Delete/5
        public async Task<IActionResult> Delete(ushort? id)
        {
            if (id == null || _context.SocialEvents == null)
            {
                return NotFound();
            }

            var socialEvent = await _context.SocialEvents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (socialEvent == null)
            {
                return NotFound();
            }

            return View(socialEvent);
        }

        // POST: SocialEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(ushort id)
        {
            if (_context.SocialEvents == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SocialEvents'  is null.");
            }
            var socialEvent = await _context.SocialEvents.FindAsync(id);
            if (socialEvent != null)
            {
                _context.SocialEvents.Remove(socialEvent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SocialEventExists(ushort id)
        {
          return _context.SocialEvents.Any(e => e.Id == id);
        }
    }
}
