using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebDACS.Repositories;
using WebShopNPT.Models;

namespace WebDACS.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IProduct productR;
        private readonly ICategory categoryR;
        private readonly WebSiteDacsContext _context;
        private WebSiteDacsContext? context;

        public CategoryController(IProduct productRepository, ICategory categoryRepository)
        {
            productR = productRepository;
            categoryR = categoryRepository;
            _context = context;
        }
        public async Task<IActionResult> IndexC()
        {
            var categories = await categoryR.GetAllAsync();
            return View(categories);
        }

        public async Task<IActionResult> AddC()
        {
            return View();
        }

        //Hàm xử lý add cate
        [HttpPost]
        public async Task<IActionResult> AddC(Category category)
        {
            if (ModelState.IsValid)
            {
                await categoryR.AddAsync(category);
                return RedirectToAction(nameof(IndexC));
            }
            var categories = await categoryR.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await categoryR.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteC(int id)
        {
            await categoryR.DeleteAsync(id);
            return RedirectToAction(nameof(IndexC));
        }
    }
}
