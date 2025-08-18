using Microsoft.AspNetCore.Mvc;
using Eticaret.WebUI.Services;

namespace Eticaret.WebUI.ViewComponents
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly ICartService _cart;
        public CartSummaryViewComponent(ICartService cart) => _cart = cart;

        public IViewComponentResult Invoke()
        {
            var model = _cart.Get();
            return View(model); // Views/Shared/Components/CartSummary/Default.cshtml
        }
    }
}
