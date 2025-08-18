using System.ComponentModel.DataAnnotations;

namespace Eticaret.Core.Entities
{
    public class Order : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Sipariş No")]
        public string OrderNumber { get; set; } = Guid.NewGuid().ToString("N")[..10].ToUpper();

        [Display(Name = "Kullanıcı")]
        public int AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        [Display(Name = "Durum")]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Display(Name = "Toplam Tutar")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Oluşturma"), ScaffoldColumn(false)]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        // Adres "snapshot"
        [Display(Name = "Teslimat Adı Soyadı")]
        public string ShipFullName { get; set; } = string.Empty;

        [Display(Name = "Telefon")]
        public string ShipPhone { get; set; } = string.Empty;

        [Display(Name = "İl")]
        public string ShipCity { get; set; } = string.Empty;

        [Display(Name = "İlçe")]
        public string ShipDistrict { get; set; } = string.Empty;

        [Display(Name = "Adres")]
        public string ShipAddressLine { get; set; } = string.Empty;

        [Display(Name = "Posta Kodu")]
        public string? ShipPostalCode { get; set; }

        // Ödeme Yöntemi
        [Display(Name = "Ödeme Yöntemi")]
        [StringLength(30)]
        public string PaymentMethod { get; set; } = "Kapıda Ödeme";

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
