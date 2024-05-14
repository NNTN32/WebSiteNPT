using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebDACS.Repositories;
using WebShopNPT.Areas.Admin.Models;
using WebShopNPT.Extensions;
using WebShopNPT.Models;
using WebShopNPT.Services;

namespace WebShopNPT.Controllers
{
    [Authorize(Roles = SD.Role_Customer)]
    public class ShoppingCartController : Controller
    {
        private readonly IProduct productR;
        private readonly WebSiteDacsContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IVnPayService _vnPayservice;

        public ShoppingCartController(WebSiteDacsContext context, UserManager<User> userManager ,IProduct productRepo, IVnPayService vnPayService)
        {
            productR = productRepo;
            _context = context;
            _userManager = userManager;
            _vnPayservice = vnPayService;
            
        }
        public IActionResult Checkout()
        {
            return View(new Order());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Order order, string payment = "Thanh Toán VnPAY")
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                // Xử lý giỏ hàng trống... 
                return RedirectToAction("Index");
            }
            var user = await _userManager.GetUserAsync(User);
            order.UserId = user.Id;
            order.OrderDate = DateTime.UtcNow;
            order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
            order.OrderDetails = cart.Items.Select(i => new OrderDetail
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList();
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            if (payment == "Thanh Toán VnPAY")
            {
                var vnPayModel = new VnPaymentRequestModel
                {
                    Amount = (double)order.OrderDetails.Sum(p=> p.Quantity*p.Price),
                    CreatedDate = DateTime.Now,
                    Description = $"{order.Notes} {order.ShippingAddress}",
                    FullName = order.UserId,
                    OrderId = new Random().Next(1000, 100000)
                };
                return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
            }
            HttpContext.Session.Remove("Cart");
            return View("OrderCompleted", order.Id); // Trang xác nhận hoàn thành đơn hàng
        }

        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            // Giả sử bạn có phương thức lấy thông tin sản phẩm từ productId 
            var product = await GetProductFromDatabase(productId);

            var cartItem = new CartItem
            {
                ProductId = productId,
                Name = product.Name,
                Price = product.Price,
                Quantity = quantity,
                ImageUrl = product.ImageUrl
            };
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            cart.AddItem(cartItem);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            //Trả về trang hiển thị sản phẩm
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> Decrease(int Id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            //Lấy ID sp cần giảm quantity
            CartItem cartItem = cart.Items.Where(c => c.ProductId == Id).FirstOrDefault();
            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveItem(Id);
            }
            if (cart.Items.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Increase(int Id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            //Lấy ID sp cần tăng quantity
            CartItem cartItem = cart.Items.Where(c => c.ProductId == Id).FirstOrDefault();
            if (cartItem.Quantity > 0)
            {
                ++cartItem.Quantity;
            }
            else
            {
                cart.RemoveItem(Id);
            }
            if (cart.Items.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        public int Total()
        {
            int totalQuantity = 0;
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if(cart != null)
            {
                totalQuantity = cart.Items.Sum(i => i.Quantity);
            }
            else
            {
                HttpContext.Session.Remove("Cart");
            }
            return totalQuantity;

        }
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            var totalQuantity = Total();
            ViewBag.TotalQuantity = totalQuantity;
            return View(cart);

        }

        public IActionResult Buy()
        {
            return View();
        }
        // Các actions khác... 
        private async Task<Product> GetProductFromDatabase(int productId)
        {
            // Truy vấn cơ sở dữ liệu để lấy thông tin sản phẩm 
            var product = await productR.GetByIdAsync(productId);
            return product;
            
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");

            if (cart is not null)
            {
                cart.RemoveItem(productId);
                // Lưu lại giỏ hàng vào Session sau khi đã xóa mục 
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }


        [Authorize]
        public IActionResult PaymentFail()
        {
            return View();
        }

        [Authorize]
        public IActionResult PaymentCallBack()
        {
            var response = _vnPayservice.PaymentExecute(Request.Query);

            if(response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VnPay:";
                return RedirectToAction("PaymentFail");
            }

            TempData["Message"] = $"Thanh toán VnPay thành công";
            return RedirectToAction("OrderCompleted");
        }

    }
}
