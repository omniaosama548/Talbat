using System.ComponentModel.DataAnnotations;

namespace Talbat.APIs.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[@$!%*?&#])[A-Za-z\\d@$!%*?&#]+$",
            ErrorMessage = "password  must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
        public string Password { get; set; }
    }
}
