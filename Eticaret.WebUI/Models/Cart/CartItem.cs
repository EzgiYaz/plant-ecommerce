namespace Eticaret.WebUI.Models.Cart
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = "";
        public string? Image { get; set; }     // Product.Image
        public decimal UnitPrice { get; set; } // Product.Price
        public int Quantity { get; set; }

        public decimal LineTotal => UnitPrice * Quantity;
    }
}
