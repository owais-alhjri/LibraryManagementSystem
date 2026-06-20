using LMS.Application.DTOs.User;
using LMS.Application.Interfaces;
using LMS.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(
        IUserService userService,
        IPasswordHasherService hasherService,
        ITokenService tokenService,
        IRefreshTokenService refreshTokenService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            var user = await userService.AddUserAsync(registerUserDto);
            return Ok(new ResponseUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                Message = "User is registered successfully"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginRequestDto request)
        {
            var user = await userService.GetUserByEmail(request.Email);

            if (!hasherService.Verify(user.PasswordHash, request.Password))
            {
                return Unauthorized();
            }

            var claims = new JwtUserClaims
            {
                UserId = user.Id,
                Email = user.Email,
                Role = user.Role
            };

            var accessToken = tokenService.GenerateToken(claims);
            var refreshToken = await refreshTokenService.GenerateAndStoreAsync(user.Id);

            SetRefreshTokenCookie(refreshToken.Token, refreshToken.ExpiresAt);
            return Ok(new AuthLoginResponseDto { Token = accessToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var token = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token)) return Unauthorized();

            var refreshToken = await refreshTokenService.ValidateAsync(token);
            if (refreshToken == null) return Unauthorized();

            var user = await userService.GetUserById(refreshToken.UserId);
            var newRefreshToken = await refreshTokenService.RotateAsync(refreshToken);

            SetRefreshTokenCookie(newRefreshToken.Token, newRefreshToken.ExpiresAt);

            var claims = new JwtUserClaims
            {
                UserId = user.Id,
                Email = user.Email,
                Role = user.Role
            };
            var accessToken = tokenService.GenerateToken(claims);
            return Ok(new AuthLoginResponseDto { Token = accessToken });

        }

        [HttpPatch("{id}/role")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleRequestDto request)
        {
            await userService.UpdateRoleAsync(id, request.Role);
            return NoContent();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Cookies["refreshToken"];
            if (!string.IsNullOrEmpty(token))
            {
                var refreshToken = await refreshTokenService.ValidateAsync(token);
                if (refreshToken != null)
                {
                    await refreshTokenService.RevokeAsync(refreshToken);
                }
            }
            Response.Cookies.Delete("refreshToken");
            return NoContent();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userService.GetAllUsersExceptAdminAsync();
            return Ok(users);
        }

        private void SetRefreshTokenCookie(string token, DateTime expires)
        {
            Response.Cookies.Append("refreshToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, //note that you should flip it to true once using HTTPS
                SameSite = SameSiteMode.Lax,
                Expires = expires
            });
        }
    }
}