using System.ComponentModel.DataAnnotations;
namespace LMS.Application.DTOs.User
{
    public class RegisterUserDto
    {
        [Required]
        [StringLength(100,MinimumLength =3)]

        public string Name { get; set; } = null!;
        [Required]
        [EmailAddress]

        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;

    }
}
