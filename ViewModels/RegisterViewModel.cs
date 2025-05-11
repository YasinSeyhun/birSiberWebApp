using System.ComponentModel.DataAnnotations;

namespace BirSiberDanismanlik.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "E-posta")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Lütfen geçerli bir Gmail adresi giriniz.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
            ErrorMessage = "Şifre en az 8 karakter uzunluğunda olmalı ve en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre (Tekrar)")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
} 