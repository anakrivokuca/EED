using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Membership_Provider
{
    public interface IAuthProvider
    {
        User GetUserFromCookie();
        bool Authenticate(string username, string password);
        void Logout(User user);
    }
}
