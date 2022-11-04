using System.ComponentModel.DataAnnotations;
using PReMaSys.Data;

namespace PReMaSys.Models
{
    public class AdminUser
    {
        [Required]
        [Display(Name ="Email")]
        public string? Email { get; set; }

        [Required]
        [Display(Name = "Password")]

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
