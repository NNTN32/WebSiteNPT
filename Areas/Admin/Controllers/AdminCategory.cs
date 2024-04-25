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
    public class AdminCategory : Controller
    {
        private readonly IProduct productR;
        private readonly ICategory categoryR;
        private readonly WebSiteDacsContext _context;
        private WebSiteDacsContext? context;
        public AdminCategory(IProduct productRepository, ICategory categoryRepository)
        {
            productR = productRepository;
            categoryR = categoryRepository;
            _context = context;
        }

        public async Task<IActionResult> IndexCA()
        {
            var categories = await categoryR.GetAllAsync();
            return View(categories);
        }

        public async Task<IActionResult> AddCA()
        {
            return View();
        }

        //Hàm xử lý add cate
        [HttpPost]
        public async Task<IActionResult> AddCA(Category category)
        {
            if (ModelState.IsValid)
            {
                await categoryR.AddAsync(category);
                return RedirectToAction(nameof(IndexCA));
            }
            var categories = await categoryR.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(category);
        }

        public async Task<IActionResult> DeleteCA(int id)
        {
            var category = await categoryR.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("DeleteCA")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteC(int id)
        {
            await categoryR.DeleteAsync(id);
            return RedirectToAction(nameof(IndexCA));
        }
    }
}
