using System.Text.RegularExpressions;
using LMS.Domain.Enums;

namespace LMS.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public string Email { get; private set; }

        public string PasswordHash { get; private set; }

        public Roles Role { get; private set; } = Roles.MEMBER;

        private User() { }

        public User(string name, string email, string passwordHash)
        {
            ValidateName(name);
            ValidateEmail(email);

            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty");
            }
            if (name.Length < 3 || name.Length > 100)
            {
                throw new ArgumentException("Name must be between 3 and 100 characters");
            }
        }

        private static void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty");
            }

            var isValid = Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
                );

            if (!isValid)
            {
                throw new ArgumentException("Email format is invaild");
            }
        }

    }
}
