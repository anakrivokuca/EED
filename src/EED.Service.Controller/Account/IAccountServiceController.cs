using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Controller.Account
{
    public interface IAccountServiceController
    {
        Domain.User GetUserFromCookie();
        bool Authenticate(string username, string password);
        void Logout(Domain.User user);
    }
}
