using System.ComponentModel.DataAnnotations;

namespace WarehouseSystem.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(20)]
        public string MobileNumber { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        User,
        Admin
    }
}