using System.ComponentModel.DataAnnotations;

namespace InfoProtection.Models.ViewModels
{
    public class LoginViewModel
    {

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}
