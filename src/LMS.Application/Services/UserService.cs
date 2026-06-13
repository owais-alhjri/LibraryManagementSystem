using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.User;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;

namespace LMS.Application.Services
{
    public class UserService(IUserRepository userRepository, IPasswordHasherService passwordHasher) : IUserService
    {
        public async Task<User> AddUserAsync(RegisterUserDto registerUserDto)
        {
            var hashedPassword = passwordHasher.Hash(registerUserDto.Password);

            var user = new User(
                registerUserDto.Name,
                registerUserDto.Email,
                hashedPassword);



            await userRepository.AddUserAsync(user);
            await userRepository.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var userEmail = await userRepository.GetByEmailAsync(email) ?? throw new NotFoundException("User email ", email);
            return userEmail;
        }


    }
}
