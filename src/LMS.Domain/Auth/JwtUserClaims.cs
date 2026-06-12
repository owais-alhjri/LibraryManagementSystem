using LMS.Domain.Enums;

namespace LMS.Domain.Auth
{
    public class JwtUserClaims
    {
        public Guid UserId { get; set; } 
        public string Email { get; set; } = null!;
        public Roles Role { get; set; }
    }
}
