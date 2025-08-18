using Microsoft.AspNetCore.Mvc;
using Eticaret.WebUI.Services;

namespace Eticaret.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cart;
        public CartController(ICartService cart) => _cart = cart;

        public IActionResult Index()
        {
            var model = _cart.Get();
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(int id, int qty = 1, string? returnUrl = null)
        {
            _cart.Add(id, qty);
            if (!string.IsNullOrEmpty(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(int id, int qty)
        {
            _cart.Update(id, qty);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            _cart.Remove(id);
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            _cart.Clear();
            return RedirectToAction("Index");
        }
    }
}
