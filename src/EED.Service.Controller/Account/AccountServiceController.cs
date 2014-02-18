using EED.Service.Membership_Provider;

namespace EED.Service.Controller.Account
{
    public class AccountServiceController : IAccountServiceController
    {
        private readonly IAuthProvider _provider;

        public AccountServiceController(IAuthProvider provider)
        {
            _provider = provider;
        }

        public Domain.User GetUserFromCookie()
        {
            return _provider.GetUserFromCookie();
        }

        public bool Authenticate(string username, string password)
        {
            return _provider.Authenticate(username, password);
        }

        public void Logout(Domain.User user)
        {
            _provider.Logout(user);
        }
    }
}
