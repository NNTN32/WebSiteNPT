using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebDACS.Repositories;
using WebShopNPT.Models;

namespace WebShopNPT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProduct productR;

        public HomeController(ILogger<HomeController> logger, IProduct productRepo)
        {
            _logger = logger;
            productR = productRepo;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> IndexItem()
        {
            var products = await productR.GetAllAsync();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
