using System.Linq;

namespace Eticaret.WebUI.Models.Cart
{
    public class CartModel
    {
        public List<CartItem> Items { get; set; } = new();

        public void Add(int productId, string name, string? image, decimal price, int qty = 1)
        {
            var line = Items.FirstOrDefault(x => x.ProductId == productId);
            if (line == null)
                Items.Add(new CartItem { ProductId = productId, Name = name, Image = image, UnitPrice = price, Quantity = qty });
            else
                line.Quantity += qty;
        }

        public void Update(int productId, int qty)
        {
            var line = Items.FirstOrDefault(x => x.ProductId == productId);
            if (line == null) return;
            if (qty <= 0) Items.Remove(line);
            else line.Quantity = qty;
        }

        public void Remove(int productId) => Items.RemoveAll(x => x.ProductId == productId);
        public void Clear() => Items.Clear();

        public int TotalQuantity => Items.Sum(x => x.Quantity);
        public decimal Subtotal => Items.Sum(x => x.LineTotal);
        public bool IsEmpty => Items.Count == 0;
    }
}
