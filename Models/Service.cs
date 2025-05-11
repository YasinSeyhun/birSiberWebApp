using System.ComponentModel.DataAnnotations;

namespace BirSiberDanismanlik.Models;

public class Service
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Hizmet adı zorunludur.")]
    [StringLength(100, ErrorMessage = "Hizmet adı en fazla 100 karakter olabilir.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Açıklama zorunludur.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Fiyat zorunludur.")]
    [Range(0, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
    public decimal Price { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 