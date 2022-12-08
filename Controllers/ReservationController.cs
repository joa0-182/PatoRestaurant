using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PatoRestaurant.ViewModels;

namespace PatoRestaurant.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public ReservationController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: Reservation
        public IActionResult Index()
        {
            return View();
        }

        // GET: Reservation
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _context.Reservations
                .Include(r => r.StatusReservation)
                .ToListAsync() });
        }

        // GET: Reservation/Details/5
        public async Task<IActionResult> Details(ushort? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.StatusReservation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservation/Create
        public IActionResult Create()
        {
            ViewData["StatusReservationId"] = new SelectList(_context.StatusReservations, "Id", "Name");
            return View();
        }

        // POST: Reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ReservationDate,Phone,Guests,Email,CreatedDate,StatusReservationId")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StatusReservationId"] = new SelectList(_context.StatusReservations, "Id", "Name", reservation.StatusReservationId);
            return View(reservation);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CustomerRequest(ReservationVM model)
        {
            if (ModelState.IsValid)
            {
                var reservation = new Reservation()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    ReservationDate = DateTime.Parse(model.ReservationDate + " " + model.ReservationTime),
                    Guests = model.Guests
                };
                reservation.StatusReservationId = 1;
                reservation.CreatedDate = DateTime.Now;
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                string mensagem = $"Solicitação de Reserva Recebida<br>"
                                + $"Nome...: {model.Name}<br>"
                                + $"Celular: {model.Phone}<br>"
                                + $"Data...: {model.ReservationDate}<br>";
                await _emailSender.SendEmailAsync("gallojunior@gmail.com", "Reserva de Mesa", mensagem);
                return Json(new { success = true, message = "Sua reserva foi recebida e entraremos em contato com breve!!!" });
            }
            return Json(new { success = false, message = "Sua reserva não foi realizada, verifique seus dados!!" });
        }

        // GET: Reservation/Edit/5
        public async Task<IActionResult> Edit(ushort? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["StatusReservationId"] = new SelectList(_context.StatusReservations, "Id", "Name", reservation.StatusReservationId);
            return View(reservation);
        }

        // POST: Reservation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ushort id, [Bind("Id,Name,ReservationDate,Phone,Guests,Email,CreatedDate,StatusReservationId")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            ViewData["StatusReservationId"] = new SelectList(_context.StatusReservations, "Id", "Name", reservation.StatusReservationId);
            return View(reservation);
        }

        // DELETE: Reservation/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(ushort? id)
        {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return Json(new { success = false, message = "Reserva não encontrada" });
            }

            try
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Json(new { success = false, message = "Ocorreu um problema inesperado! Avise ao Suporte!" });
            }

            return Json(new { success = true, message = "Reserva Excluída com Sucesso!" });
        }

        private bool ReservationExists(ushort id)
        {
          return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
