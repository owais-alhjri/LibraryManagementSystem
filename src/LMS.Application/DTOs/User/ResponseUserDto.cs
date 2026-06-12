namespace LMS.Application.DTOs.User
{
    public class ResponseUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Message { get; set; } = string.Empty;
        public string Role { get; set; } = null!;

    }
}
