using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public AuthenticationResult Register(string firstName, string lastName, string email, string password)
        {
            // 1. Validate the user doesn't already exist
            var existingUser = _userRepository.GetUserByEmail(email);
            if (existingUser is not null)
            {
                throw new Exception($"User with given email {email} already exists");
            }

            // 2. Create user (generate unique ID) & Persist to DB
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };
            _userRepository.Add(user);

            // 3. Create JWT token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                user,
                token);
        }

        public AuthenticationResult Login(string email, string password)
        {
            // 1. Validate the user exists
            var existingUser = _userRepository.GetUserByEmail(email);
            if (existingUser is null)
            {
                throw new Exception($"User with given email {email} does not exist");
            }

            // 2. Validate the password is correct
            if (existingUser.Password != password)
            {
                throw new Exception($"Password is incorrect");
            }

            // 3. Create JWT token
            var token = _jwtTokenGenerator.GenerateToken(existingUser);

            return new AuthenticationResult(
                existingUser,
                token);
        }
    }
}
