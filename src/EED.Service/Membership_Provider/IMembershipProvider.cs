using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace EED.Service.Membership_Provider
{
    public interface IMembershipProvider
    {
        IEnumerable<User> GetAllUsers();
        User CreateUser(User user, out MembershipCreateStatus status);
        void UpdateUser(User user);
        bool DeleteUser(string username, bool deleteAllRelatedData);
    }
}
