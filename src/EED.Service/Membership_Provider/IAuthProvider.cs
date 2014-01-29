using EED.Domain;

namespace EED.Service.Membership_Provider
{
    public interface IAuthProvider
    {
        User GetUserFromCookie();
        bool Authenticate(string username, string password);
        void Logout(User user);
    }
}
