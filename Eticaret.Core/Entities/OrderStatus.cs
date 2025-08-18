using System.ComponentModel.DataAnnotations;

namespace Eticaret.Core.Entities
{
    public enum OrderStatus
    {
        [Display(Name = "Beklemede")]
        Pending = 0,     // Sipariş alındı, ödemesi bekleniyor

        [Display(Name = "Ödendi")]
        Paid = 1,        // Ödeme alındı

        [Display(Name = "Hazırlanıyor")]
        Preparing = 2,   // Paket hazırlanıyor

        [Display(Name = "Kargoya Verildi")]
        Shipped = 3,     // Kargoya teslim edildi

        [Display(Name = "Teslim Edildi")]
        Delivered = 4,   // Müşteriye ulaştı

        [Display(Name = "İptal Edildi")]
        Cancelled = 5    // Sipariş iptal edildi
    }
}
