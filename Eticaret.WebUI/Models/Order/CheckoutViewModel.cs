using System.ComponentModel.DataAnnotations;
using Eticaret.WebUI.Models.Cart;

namespace Eticaret.WebUI.Models.Order
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Ad Soyad alanı zorunludur.")]
        [StringLength(120, ErrorMessage = "Ad Soyad en fazla 120 karakter olmalıdır.")]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon alanı zorunludur.")]
        [StringLength(30, ErrorMessage = "Telefon en fazla 30 karakter olmalıdır.")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "İl alanı zorunludur.")]
        [StringLength(60, ErrorMessage = "İl en fazla 60 karakter olmalıdır.")]
        [Display(Name = "İl")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "İlçe alanı zorunludur.")]
        [StringLength(60, ErrorMessage = "İlçe en fazla 60 karakter olmalıdır.")]
        [Display(Name = "İlçe")]
        public string District { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adres alanı zorunludur.")]
        [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olmalıdır.")]
        [Display(Name = "Adres")]
        public string AddressLine { get; set; } = string.Empty;

        [StringLength(15, ErrorMessage = "Posta Kodu en fazla 15 karakter olmalıdır.")]
        [Display(Name = "Posta Kodu")]
        public string? PostalCode { get; set; }

        // Sepet özeti için
        public CartModel Cart { get; set; } = new();

        // Satın alırken adres defterine kaydet (admin'deki Adres Yönetimi'ne düşsün)
        [Display(Name = "Bu adresi adres defterime kaydet")]
        public bool SaveAddress { get; set; } = true;

        // Ödeme Yöntemi
        [Required(ErrorMessage = "Lütfen bir ödeme yöntemi seçiniz.")]
        [Display(Name = "Ödeme Yöntemi")]
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
