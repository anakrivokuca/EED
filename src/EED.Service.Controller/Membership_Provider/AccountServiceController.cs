using EED.Domain;
using EED.Service.Membership_Provider;
using System;

namespace EED.Service.Controller.Membership_Provider
{
    public class AccountServiceController : IAccountServiceController
    {
        private readonly IAuthProvider _provider;

        public AccountServiceController(IAuthProvider provider)
        {
            _provider = provider;
        }

        public User GetUserFromCookie()
        {
            return _provider.GetUserFromCookie();
        }

        public bool Authenticate(string username, string password)
        {
            return _provider.Authenticate(username, password);
        }

        public void Logout(User user)
        {
            _provider.Logout(user);
        }
    }
}
