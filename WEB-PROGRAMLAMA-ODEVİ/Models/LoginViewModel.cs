using System.ComponentModel.DataAnnotations;

namespace WEB_PROGRAMLAMA_ODEVİ.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username girilmesi zorunludur.")]
        [StringLength(30, ErrorMessage = "Username en fazla 30 karakterden oluşabilir.")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Password girilmesi zorunludur.")]
        [MinLength(6, ErrorMessage = "Password minimum 6 karakter olmalıdır.")]
        [MaxLength(16, ErrorMessage = "Password en fazla 16 karakter alabilir.")]
        public string Password { get; set; }
    }
}
