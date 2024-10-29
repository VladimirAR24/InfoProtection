using System.ComponentModel.DataAnnotations;

namespace InfoProtection.Models.ViewModels
{
    public class EncryptionViewModel
    {
        [Required]
        [Display(Name = "Algorithm")]
        public required string Algorithm { get; set; }

        [Display(Name = "TextStart")]
        public required string TextStart { get; set; }

        [Display(Name = "TextEnd")]
        public string? TextEnd { get; set; }
    }
}
