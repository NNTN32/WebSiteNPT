using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebDACS.Repositories;
using WebShopNPT.Areas.Admin.Models;
using WebShopNPT.Models;

namespace WebShopNPT.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class AdminBrand : Controller
    {
        private readonly IProduct productR;
        private readonly IBrand brandR;
        private readonly WebSiteDacsContext _context;
        private WebSiteDacsContext? context;

        public AdminBrand(IProduct productRepository, IBrand brandRepository)
        {
            productR = productRepository;
            brandR = brandRepository;
            _context = context;
        }
        public async Task<IActionResult> IndexBA()
        {
            var brands = await brandR.GetAllAsync();
            return View(brands);
        }

        public async Task<IActionResult> AddBA()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBA(Brand brand)
        {
            if (ModelState.IsValid)
            {
                await brandR.AddAsync(brand);
                return RedirectToAction(nameof(IndexBA));
            }
            var brands = await brandR.GetAllAsync();
            ViewBag.Brands = new SelectList(brands, "Id", "BrandName");
            return View(brand);
        }

        public async Task<IActionResult> DeleteBA(int id)
        {
            var brand = await brandR.GetByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }
        [HttpPost, ActionName("DeleteBA")]
        public async Task<IActionResult> Delete(int id)
        {
            await brandR.DeleteAsync(id);
            return RedirectToAction(nameof(IndexBA));
        }
    }
}
