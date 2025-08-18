using Eticaret.Core.Entities;
using Eticaret.Data;
using Eticaret.WebUI.Models.Order;
using Eticaret.WebUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eticaret.WebUI.Controllers
{
    [Authorize] // sadece giriş yapan kullanıcılar
    public class OrderController : Controller
    {
        private readonly DatabaseContext _db;
        private readonly ICartService _cart;

        public OrderController(DatabaseContext db, ICartService cart)
        {
            _db = db;
            _cart = cart;
        }

        // GET: /Order/Checkout
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var cart = _cart.Get();
            if (cart.IsEmpty)
            {
                TempData["Info"] = "Sepetiniz boş.";
                return RedirectToAction("Index", "Cart");
            }

            var vm = new CheckoutViewModel { Cart = cart };

            // Varsayılan adresi doldur
            var userGuid = HttpContext.User.FindFirst("UserGuid")?.Value;
            if (!string.IsNullOrEmpty(userGuid))
            {
                var user = await _db.AppUsers
                    .FirstOrDefaultAsync(u => u.UserGuid.ToString() == userGuid);

                if (user != null)
                {
                    var addr = await _db.Addresses
                        .Where(a => a.AppUserId == user.Id)
                        .OrderByDescending(a => a.IsDefault)
                        .FirstOrDefaultAsync();

                    if (addr != null)
                    {
                        vm.FullName = addr.FullName;
                        vm.Phone = addr.Phone ?? string.Empty;
                        vm.City = addr.City ?? string.Empty;
                        vm.District = addr.District ?? string.Empty;
                        vm.AddressLine = addr.AddressLine ?? string.Empty;
                        vm.PostalCode = addr.PostalCode;
                    }
                }
            }

            return View(vm);
        }

        // POST: /Order/Checkout
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var cart = _cart.Get();
            if (cart.IsEmpty)
                ModelState.AddModelError(string.Empty, "Sepetiniz boş.");

            if (!ModelState.IsValid)
            {
                model.Cart = cart;
                return View(model);
            }

            var userGuid = HttpContext.User.FindFirst("UserGuid")?.Value;
            var user = await _db.AppUsers
                .FirstOrDefaultAsync(u => u.UserGuid.ToString() == userGuid);

            if (user == null)
                return Challenge(); // oturum yoksa girişe yönlendir

            // Siparişi oluştur
            var order = new Order
            {
                AppUserId = user.Id,
                Status = OrderStatus.Pending,
                TotalAmount = cart.Subtotal,
                ShipFullName = model.FullName,
                ShipPhone = model.Phone,
                ShipCity = model.City,
                ShipDistrict = model.District,
                ShipAddressLine = model.AddressLine,
                ShipPostalCode = model.PostalCode,

                // Ödeme yöntemi
                PaymentMethod = model.PaymentMethod
            };

            foreach (var line in cart.Items)
            {
                var p = await _db.Products.FindAsync(line.ProductId);
                if (p == null) continue;

                order.Items.Add(new OrderItem
                {
                    ProductId = p.Id,
                    Quantity = line.Quantity,
                    UnitPrice = p.Price
                });

                // Stok düş
                if (p.Stock > 0)
                    p.Stock = Math.Max(0, p.Stock - line.Quantity);
            }

            // Adres defterine kaydet
            if (model.SaveAddress)
            {
                var existing = await _db.Addresses
                    .Where(a => a.AppUserId == user.Id)
                    .FirstOrDefaultAsync(a =>
                        (a.FullName ?? "").Trim().ToLower() == (model.FullName ?? "").Trim().ToLower() &&
                        (a.Phone ?? "").Trim().ToLower() == (model.Phone ?? "").Trim().ToLower() &&
                        (a.City ?? "").Trim().ToLower() == (model.City ?? "").Trim().ToLower() &&
                        (a.District ?? "").Trim().ToLower() == (model.District ?? "").Trim().ToLower() &&
                        (a.AddressLine ?? "").Trim().ToLower() == (model.AddressLine ?? "").Trim().ToLower() &&
                        (a.PostalCode ?? "").Trim().ToLower() == (model.PostalCode ?? "").Trim().ToLower()
                    );

                if (existing is null)
                {
                    bool hasAny = await _db.Addresses.AnyAsync(a => a.AppUserId == user.Id);

                    _db.Addresses.Add(new Address
                    {
                        AppUserId = user.Id,
                        FullName = model.FullName,
                        Phone = model.Phone,
                        City = model.City,
                        District = model.District,
                        AddressLine = model.AddressLine,
                        PostalCode = model.PostalCode,
                        IsDefault = !hasAny
                    });
                }
                else
                {
                    existing.FullName = model.FullName;
                    existing.Phone = model.Phone;
                    existing.City = model.City;
                    existing.District = model.District;
                    existing.AddressLine = model.AddressLine;
                    existing.PostalCode = model.PostalCode;
                }
            }

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            _cart.Clear();

            return RedirectToAction(nameof(Success), new { id = order.Id });
        }

        // GET: /Order/Success/5
        [HttpGet]
        public async Task<IActionResult> Success(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }
    }
}
