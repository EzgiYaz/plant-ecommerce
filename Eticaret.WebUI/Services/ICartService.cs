using Eticaret.WebUI.Models.Cart;

namespace Eticaret.WebUI.Services
{
    public interface ICartService
    {
        CartModel Get();
        void Save(CartModel cart);
        void Add(int productId, int qty = 1);
        void Update(int productId, int qty);
        void Remove(int productId);
        void Clear();
    }
}
