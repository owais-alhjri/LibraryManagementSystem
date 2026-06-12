namespace LMS.Application.DTOs.User
{
    public class AuthLoginRequestDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
