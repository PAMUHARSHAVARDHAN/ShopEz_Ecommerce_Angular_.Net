using System.ComponentModel.DataAnnotations;

namespace UserService.DTO
{
    public class RegisterDTO
    {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [MinLength(3)]
            public string FullName { get; set; } = string.Empty;

            [Required]
            [Phone]
            public string MobileNo { get; set; } = string.Empty;

            [Required]
            [MinLength(6)]
            public string Password { get; set; } = string.Empty;
        }
        public class LoginDto
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            public string Password { get; set; } = string.Empty;
        }
    
}
