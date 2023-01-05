using System.ComponentModel.DataAnnotations;

namespace WEB_PROGRAMLAMA_ODEVİ.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, ErrorMessage = "Username can be max 30 characters.")]
        public string Username { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = "RE-Password is required.")]
        [MinLength(6, ErrorMessage = "RE-Password can be min 6 characters.")]
        [MaxLength(16, ErrorMessage = "RE-Password can be max 16 characters.")]
        [Compare(nameof(Password))]
        public string RePassword { get; set; }
    }
}
