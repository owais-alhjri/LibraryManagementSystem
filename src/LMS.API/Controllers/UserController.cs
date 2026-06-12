using LMS.Application.DTOs.User;
using LMS.Application.Interfaces;
using LMS.Domain.Auth;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasherService _hasherService;
        private readonly ITokenService _tokenService;
        public UserController(IUserService userService, IPasswordHasherService hasherService, ITokenService tokenService)
        {
            _userService = userService;
            _hasherService = hasherService;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            var user = await _userService.AddUserAsync(registerUserDto);
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
            var user = await _userService.GetUserByEmail(request.Email);

            if (!_hasherService.Verify(user.PasswordHash, request.Password))
            {
                return Unauthorized();
            }

            var claims = new JwtUserClaims
            {
                UserId = user.Id,
                Email = user.Email,
                Role = user.Role
            };

            var token = _tokenService.GenerateToken(claims);
            return Ok(new AuthLoginResponseDto { Token = token });

        }



    }
}
