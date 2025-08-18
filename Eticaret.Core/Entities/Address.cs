using System.ComponentModel.DataAnnotations;

namespace Eticaret.Core.Entities
{
    public class Address : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Başlık (Ev/İş)")]
        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Ad Soyad")]
        [Required, StringLength(120)]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Telefon")]
        [Required, StringLength(30)]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "İl")]
        [Required, StringLength(60)]
        public string City { get; set; } = string.Empty;

        [Display(Name = "İlçe")]
        [Required, StringLength(60)]
        public string District { get; set; } = string.Empty;

        [Display(Name = "Mahalle/Semt")]
        [StringLength(120)]
        public string? Neighborhood { get; set; }

        [Display(Name = "Adres")]
        [Required, StringLength(500)]
        public string AddressLine { get; set; } = string.Empty;

        [Display(Name = "Posta Kodu")]
        [StringLength(15)]
        public string? PostalCode { get; set; }

        [Display(Name = "Varsayılan")]
        public bool IsDefault { get; set; } = false;

        [Display(Name = "Kullanıcı")]
        public int AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
