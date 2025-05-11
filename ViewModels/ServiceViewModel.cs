using System.ComponentModel.DataAnnotations;

namespace birSiberDanismanlik.ViewModels
{
    public class ServiceViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        [Display(Name = "Hizmet Adı")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Display(Name = "Fiyat")]
        [Range(0, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Süre zorunludur.")]
        [Display(Name = "Süre (Dakika)")]
        [Range(1, int.MaxValue, ErrorMessage = "Süre 1 dakikadan az olamaz.")]
        public int Duration { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
} 