using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatoRestaurant.ViewModels;

namespace PatoRestaurant.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Menu()
    {
        var menus = new List<Menu>();
        foreach (var category in _context.Categories.ToList())
        {
            var menu = new Menu()
            {
                Category = category,
                Products = _context.Products.Where(p => p.CategoryId == category.Id).ToList()
            };
            menus.Add(menu);
        }
        return View(menus);
    }

    public IActionResult Reservation()
    {
        return View();
    }

    public IActionResult Gallery()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Blog()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
