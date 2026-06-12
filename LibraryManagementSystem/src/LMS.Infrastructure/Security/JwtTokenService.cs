using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LMS.Application.Interfaces;
using LMS.Domain.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace LMS.Infrastructure.Security
{
    public class JwtTokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(JwtUserClaims user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("role",user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)
                );
            var credentals = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256
                );

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: credentals
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
