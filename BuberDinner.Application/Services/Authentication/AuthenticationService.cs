namespace BuberDinner.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        public AuthenticationResult Register(string firstName, string lastName, string email, string password)
        {
            return new AuthenticationResult();
        }

        public AuthenticationResult Login(string email, string password)
        {
            return new AuthenticationResult();
        }
    }
}
