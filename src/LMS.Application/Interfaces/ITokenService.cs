using LMS.Domain.Auth;
using LMS.Domain.Entities;

namespace LMS.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(JwtUserClaims claims);
    }
}
