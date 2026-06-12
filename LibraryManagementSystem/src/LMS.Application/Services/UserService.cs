using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.User;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;

namespace LMS.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasher;
        public UserService(IUserRepository userRepository, IPasswordHasherService passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> AddUserAsync(RegisterUserDto registerUserDto)
        {
            var hashedPassword = _passwordHasher.Hash(registerUserDto.Password);

            var user = new User(
                registerUserDto.Name,
                registerUserDto.Email,
                hashedPassword);



            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var userEmail = await _userRepository.GetByEmailAsync(email) ?? throw new NotFoundException("User email ", email);
            return userEmail;
        }


    }
}
