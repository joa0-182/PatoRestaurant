using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace PatoRestaurant.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class SocialEventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public SocialEventController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: SocialEvent
        public IActionResult Index()
        {
              return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _context.SocialEvents.ToListAsync();
            return Json(new { data = events });
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
        public async Task<IActionResult> Create([Bind("Id,Name,Description,EventDate,Image")] SocialEvent socialEvent, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string wwwRoot = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string newFile = Path.Combine(wwwRoot, @"img\socialevents", fileName);
                    using (var stream = new FileStream(newFile, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    socialEvent.Image = @"\img\socialevents\" + fileName;
                }
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
        public async Task<IActionResult> Edit(ushort id, [Bind("Id,Name,Description,EventDate,Image")] SocialEvent socialEvent, IFormFile file)
        {
            if (id != socialEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string wwwRoot = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string newFile = Path.Combine(wwwRoot, @"img\socialevents", fileName);
                    using (var stream = new FileStream(newFile, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    if (socialEvent.Image != null)
                    {
                        string oldFile = Path.Combine(wwwRoot, socialEvent.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                    }
                    socialEvent.Image = @"\img\socialevents\" + fileName;
                }

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

        // DELETE: SocialEvent/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(ushort id)
        {
            var socialEvent = _context.SocialEvents.Find(id);
            if (socialEvent == null)
            {
                return Json(new { success = false, message = "Evento não encontrado" });
            }
            try
            {
                _context.SocialEvents.Remove(socialEvent);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Json(new { success = false, message = "Ocorreu um problema inesperado! Avise ao Suporte!" });
            }
            return Json(new { success = true, message = "Evento Excluído com Sucesso" });
        }

        private bool SocialEventExists(ushort id)
        {
          return _context.SocialEvents.Any(e => e.Id == id);
        }
    }
}
