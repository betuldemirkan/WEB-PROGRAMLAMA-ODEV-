using System.ComponentModel.DataAnnotations;

namespace WEB_PROGRAMLAMA_ODEVİ.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username zorunludur.")]
        [StringLength(30, ErrorMessage = "Username en fazla 30 karakterden oluşabilir.")]
        public string Username { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = "Passwor tekrar girilmelidir.")]
        [MinLength(6, ErrorMessage = "RE-Password minimum 6 karakter almalıdır.")]
        [MaxLength(16, ErrorMessage = "RE-Password en fazla 16 karakter alabilir.")]
        [Compare(nameof(Password))]
        public string RePassword { get; set; }
    }
}
