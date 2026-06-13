using LMS.Application.DTOs.User;
using LMS.Domain.Entities;
using LMS.Domain.Enums;

namespace LMS.Application.Interfaces
{
    public interface IUserService
    {
         Task<User> AddUserAsync(RegisterUserDto registerUserDto);
         Task<User> GetUserByEmail(string email);
         Task UpdateRoleAsync(Guid id, Roles role);
    }
}
