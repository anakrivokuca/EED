using System.Collections.Generic;
using System.Web.Security;

namespace EED.Service.Controller.User
{
    public interface IUserServiceController
    {
        IEnumerable<Domain.User> GetAllUsers();
        Domain.User GetUser(string username);
        Domain.User CreateUser(Domain.User user, out MembershipCreateStatus status);
        void UpdateUser(Domain.User user);
        bool DeleteUser(string username, bool deleteAllRelatedData);
        IEnumerable<Domain.User> FilterUsers(IEnumerable<Domain.User> users, string searchText);
    }
}
