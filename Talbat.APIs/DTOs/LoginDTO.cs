using System.ComponentModel.DataAnnotations;

namespace Talbat.APIs.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public  string Password { get; set; }
    }
}
