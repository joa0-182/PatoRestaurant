using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PatoRestaurant.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class StatusReservationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatusReservationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StatusReservation
        public async Task<IActionResult> Index()
        {
              return View(await _context.StatusReservations.ToListAsync());
        }

        // GET: StatusReservation/Details/5
        public async Task<IActionResult> Details(byte? id)
        {
            if (id == null || _context.StatusReservations == null)
            {
                return NotFound();
            }

            var statusReservation = await _context.StatusReservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statusReservation == null)
            {
                return NotFound();
            }

            return View(statusReservation);
        }

        // GET: StatusReservation/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StatusReservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] StatusReservation statusReservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(statusReservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(statusReservation);
        }

        // GET: StatusReservation/Edit/5
        public async Task<IActionResult> Edit(byte? id)
        {
            if (id == null || _context.StatusReservations == null)
            {
                return NotFound();
            }

            var statusReservation = await _context.StatusReservations.FindAsync(id);
            if (statusReservation == null)
            {
                return NotFound();
            }
            return View(statusReservation);
        }

        // POST: StatusReservation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(byte id, [Bind("Id,Name")] StatusReservation statusReservation)
        {
            if (id != statusReservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statusReservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusReservationExists(statusReservation.Id))
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
            return View(statusReservation);
        }

        // GET: StatusReservation/Delete/5
        public async Task<IActionResult> Delete(byte? id)
        {
            if (id == null || _context.StatusReservations == null)
            {
                return NotFound();
            }

            var statusReservation = await _context.StatusReservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statusReservation == null)
            {
                return NotFound();
            }

            return View(statusReservation);
        }

        // POST: StatusReservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(byte id)
        {
            if (_context.StatusReservations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.StatusReservations'  is null.");
            }
            var statusReservation = await _context.StatusReservations.FindAsync(id);
            if (statusReservation != null)
            {
                _context.StatusReservations.Remove(statusReservation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusReservationExists(byte id)
        {
          return _context.StatusReservations.Any(e => e.Id == id);
        }
    }
}
