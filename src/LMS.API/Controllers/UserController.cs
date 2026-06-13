using LMS.Application.DTOs.User;
using LMS.Application.Interfaces;
using LMS.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService, IPasswordHasherService hasherService, ITokenService tokenService) : ControllerBase
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

            var token = tokenService.GenerateToken(claims);
            return Ok(new AuthLoginResponseDto { Token = token });

        }



    }
}
