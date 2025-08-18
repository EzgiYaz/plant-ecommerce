using System.Text.Json;
using Eticaret.Data;                 
using Eticaret.WebUI.Models.Cart;
using Microsoft.AspNetCore.Http;

namespace Eticaret.WebUI.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _http;
        private readonly DatabaseContext _db;
        private const string Key = "CART";

        public CartService(IHttpContextAccessor http, DatabaseContext db)
        {
            _http = http;
            _db = db;
        }

        private ISession Session => _http.HttpContext!.Session;

        public CartModel Get()
        {
            var json = Session.GetString(Key);
            if (string.IsNullOrEmpty(json)) return new CartModel();
            return JsonSerializer.Deserialize<CartModel>(json) ?? new CartModel();
        }

        public void Save(CartModel cart)
        {
            var json = JsonSerializer.Serialize(cart);
            Session.SetString(Key, json);
        }

        public void Add(int productId, int qty = 1)
        {
            var cart = Get();
            var p = _db.Products.FirstOrDefault(x => x.Id == productId && x.IsActive);
            if (p == null) return;
            cart.Add(p.Id, p.Name, p.Image, p.Price, qty);
            Save(cart);
        }

        public void Update(int productId, int qty)
        {
            var cart = Get();
            cart.Update(productId, qty);
            Save(cart);
        }

        public void Remove(int productId)
        {
            var cart = Get();
            cart.Remove(productId);
            Save(cart);
        }

        public void Clear()
        {
            Save(new CartModel());
        }
    }
}
