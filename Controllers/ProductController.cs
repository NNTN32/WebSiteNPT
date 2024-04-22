using WebShopNPT.Models;
using Microsoft.AspNetCore.Mvc;
using WebDACS.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebShopNPT.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebDACS.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProduct productR;
        private readonly ICategory categoryR;
        private readonly IBrand brandR;
        private readonly WebSiteDacsContext _context;
        private WebSiteDacsContext? context;

        public ProductController(IProduct productRepository, ICategory categoryRepository, IBrand brandRepository)
        {
            productR = productRepository;
            categoryR = categoryRepository;
            brandR = brandRepository;
            _context = context;
        }
        public async Task<IActionResult> IndexP()
        {
            var products = await productR.GetAllAsync();
            var brand = await brandR.GetAllAsync();
            ViewBag.Brands = await brandR.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Add()
        {
            var categories = await categoryR.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            var brands = await brandR.GetAllAsync();
            ViewBag.Brands = new SelectList(brands, "Id", "BrandName");
            return View();
        }

        // Xử lý thêm sản phẩm mới 
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    // Lưu hình ảnh đại diện tham khảo bài 02 hàm SaveImage 
                    product.ImageUrl = await SaveImage(imageUrl);
                }

                await productR.AddAsync(product);
                return RedirectToAction(nameof(IndexP));
            }
            // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập 
            var categories = await categoryR.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            var brands = await brandR.GetAllAsync();
            ViewBag.Brands = new SelectList(brands, "Id", "BrandName");
            return View();
        }

        // Viết thêm hàm SaveImage (tham khảo bài 02) 

        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName); // 
            //Thay đổi đường dẫn theo cấu hình của bạn
             using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName; // Trả về đường dẫn tương đối 
        }
        // Hiển thị thông tin chi tiết sản phẩm
        public async Task<IActionResult> Display(int id)
        {
            var product = await productR.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await categoryR.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            var brands = await brandR.GetAllAsync();
            ViewBag.Brands = new SelectList(brands, "Id", "BrandName", product.BrandId);
            return View(product);
        }

        // Hiển thị form xác nhận xóa sản phẩm
        public async Task<IActionResult> Delete(int id)
        {
            var product = await productR.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete1(int id)
        {
            await productR.DeleteAsync(id);
            return RedirectToAction(nameof(IndexP));
        }

        // Hiển thị form cập nhật sản phẩm
        public async Task<IActionResult> Update(int id)
        {
            var product = await productR.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await categoryR.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            var brands = await brandR.GetAllAsync();
            ViewBag.Brands = new SelectList(brands, "Id", "BrandName", product.BrandId);
            return View(product);
        }

        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product, IFormFile imageUrl)
        {

            ModelState.Remove("ImageUrl");
            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingProduct = await productR.GetByIdAsync(id); // Giả định có phương thức GetByIdAsync 

                // Giữ nguyên thông tin hình ảnh nếu không có hình mới được tải lên
                if (imageUrl == null)
                {
                    product.ImageUrl = existingProduct.ImageUrl;
                }
                else
                {
                    // Lưu hình ảnh mới 
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.Discount = product.Discount;
                existingProduct.SKU = product.SKU;
                existingProduct.ImageUrl = product.ImageUrl;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.BrandId = product.BrandId;

                await productR.UpdateAsync(existingProduct);
                return RedirectToAction(nameof(IndexP));
            }
            var categories = await categoryR.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            var brands = await brandR.GetAllAsync();
            ViewBag.Brands = new SelectList(brands, "Id", "BrandName", product.BrandId);
            return View(product);
        }

    }
}
