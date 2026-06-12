using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LMS.Infrastructure.Security
{
    public class IdentityPasswordHasher : IPasswordHasherService
    {
        private readonly PasswordHasher<User> _passwordHasher;

        public IdentityPasswordHasher()
        {
            _passwordHasher = new PasswordHasher<User>();
        }
        public string Hash(string password)
        {
            return _passwordHasher.HashPassword(null!, password);
        }

        public bool Verify(string hashedPassword, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(null!, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
