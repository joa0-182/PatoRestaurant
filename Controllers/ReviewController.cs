using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatoRestaurant.Models;

namespace PatoRestaurant.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ReviewController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment; 
        }

        // GET: Review
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _context.Reviews.ToListAsync();
            return Json(new { data = reviews });
        }

        // GET: Review/Details/5
        public async Task<IActionResult> Details(ushort? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Review/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Review/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ReviewText,ReviewDate,Image,Rating")] Review review, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string wwwRoot = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string newFile = Path.Combine(wwwRoot, @"img\reviews", fileName);
                    using (var stream = new FileStream(newFile, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    review.Image = @"\img\reviews\" + fileName;
                }
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        // GET: Review/Edit/5
        public async Task<IActionResult> Edit(ushort? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Review/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ushort id, [Bind("Id,Name,ReviewText,ReviewDate,Image,Rating")] Review review, IFormFile file)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string wwwRoot = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string newFile = Path.Combine(wwwRoot, @"img\reviews", fileName);
                    using (var stream = new FileStream(newFile, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    if (review.Image != null)
                    {
                        string oldFile = Path.Combine(wwwRoot, review.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                    }
                    review.Image = @"\img\reviews\" + fileName;
                }
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
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
            return View(review);
        }

        // DELETE: Review/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(ushort id)
        {
            var review = _context.Reviews.Find(id);
            if (review == null)
            {
                return Json(new { success = false, message = "Avaliação não encontrada" });
            }
            try
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Json(new { success = false, message = "Ocorreu um problema inesperado! Avise ao Suporte!" });
            }
            return Json(new { success = true, message = "Avaliação Excluída com Sucesso" });
        }

        private bool ReviewExists(ushort id)
        {
          return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
