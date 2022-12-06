namespace Horus.Api.Account
{
    public class SignInRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class SignInResponse
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string AuthorizationToken { get; set; }
    }
}