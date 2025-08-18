using Eticaret.Core.Entities;
using Eticaret.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eticaret.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class OrdersController : Controller
    {
        private readonly DatabaseContext _db;
        public OrdersController(DatabaseContext db) => _db = db;

        // Liste
        public async Task<IActionResult> Index()
        {
            var list = await _db.Orders
                .Include(o => o.AppUser)   // Kullanıcıyı getir
                .OrderByDescending(o => o.Id)
                .ToListAsync();

            return View(list);
        }

        // Detay
        public async Task<IActionResult> Details(int id)
        {
            var order = await _db.Orders
                .Include(o => o.AppUser)   // Kullanıcı bilgisi
                .Include(o => o.Items).ThenInclude(i => i.Product) // Ürünler
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order is null) return NotFound();
            return View(order);
        }

        // Durum güncelle
        [HttpPost]
        public async Task<IActionResult> SetStatus(int id, OrderStatus status)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order is null) return NotFound();

            order.Status = status;
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
