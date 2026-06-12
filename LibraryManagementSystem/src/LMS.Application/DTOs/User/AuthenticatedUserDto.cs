namespace LMS.Application.DTOs.User
{
    public class AuthenticatedUserDto
    {
        public Guid Id { get; init; }
        public string Email { get; init; } = null!;

    }
}
