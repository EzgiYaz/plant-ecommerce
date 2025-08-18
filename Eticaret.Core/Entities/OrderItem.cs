using System.ComponentModel.DataAnnotations;

namespace Eticaret.Core.Entities
{
    public class OrderItem : IEntity
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Display(Name = "Adet")]
        public int Quantity { get; set; }

        [Display(Name = "Birim Fiyat")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Ara Toplam")]
        public decimal LineTotal => UnitPrice * Quantity;
    }
}
