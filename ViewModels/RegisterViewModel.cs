using System.ComponentModel.DataAnnotations;
using WarehouseSystem.Models;

namespace WarehouseSystem.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }
}