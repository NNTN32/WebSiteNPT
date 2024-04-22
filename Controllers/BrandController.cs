using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebDACS.Repositories;
using WebShopNPT.Models;

namespace WebDACS.Controllers
{
    public class BrandController : Controller
    {
        private readonly IProduct productR;
        private readonly IBrand brandR;
        private readonly WebSiteDacsContext _context;
        private WebSiteDacsContext? context;

        public BrandController(IProduct productRepository, IBrand brandRepository)
        {
            productR = productRepository;
            brandR = brandRepository;
            _context = context;
        }
        public async Task<IActionResult> IndexB()
        {
            var brands = await brandR.GetAllAsync();
            return View(brands);
        }
        public async Task<IActionResult> AddB()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddB(Brand brand)
        {
            if (ModelState.IsValid)
            {
                await brandR.AddAsync(brand);
                return RedirectToAction(nameof(IndexB));
            }
            var brands = await brandR.GetAllAsync();
            ViewBag.Brands = new SelectList(brands, "Id", "BrandName");
            return View(brand);
        }

        public async Task<IActionResult> DeleteB(int id)
        {
            var brand = await brandR.GetByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }
        [HttpPost, ActionName("DeleteB")]
        public async Task<IActionResult> Delete(int id)
        {
            await brandR.DeleteAsync(id);
            return RedirectToAction(nameof(IndexB));
        }
    }
}
