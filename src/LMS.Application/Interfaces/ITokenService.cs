using LMS.Infrastructure.Security;

namespace LMS.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(JwtUserClaims claims);
    }
}
