using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace PatoRestaurant.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Product
        public IActionResult Index()
        {
            return View();
        }

        // GET: Product/GetAll
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
            return Json(new { data = products });
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(ushort? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,CreatedDate,CategoryId,Image")] Product product, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string wwwRoot = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string newFile = Path.Combine(wwwRoot, @"img\products", fileName);
                    using (var stream = new FileStream(newFile, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    product.Image = @"\img\products\" + fileName;
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(ushort? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ushort id, [Bind("Id,Name,Description,Price,CreatedDate,CategoryId,Image")] Product product, IFormFile file)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string wwwRoot = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string newFile = Path.Combine(wwwRoot, @"img\products", fileName);
                    using (var stream = new FileStream(newFile, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    if (product.Image != null)
                    {
                        string oldFile = Path.Combine(wwwRoot, product.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                    }

                    product.Image = @"\img\products\" + fileName;
                }

                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // DELETE: Product/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(ushort? id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return Json( new { success = false, message = "Produto não encontrado"});
            }
            
            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Json( new { success = false, message = "Ocorreu um problema inesperado! Avise ao Suporte!"});
            }
            
            return Json( new { success = true, message = "Produto Excluído com Sucesso!"});
        }

        private bool ProductExists(ushort id)
        {
          return _context.Products.Any(e => e.Id == id);
        }
    }
}
