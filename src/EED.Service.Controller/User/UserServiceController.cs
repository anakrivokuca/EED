using EED.Service.Membership_Provider;
using System.Collections.Generic;
using System.Web.Security;

namespace EED.Service.Controller.User
{
    public class UserServiceController : IUserServiceController
    {
        public IMembershipProvider _provider;

        public UserServiceController()
        {
            //DependencyResolver.SetResolver(new NinjectDependencyResolver());
            //_provider = DependencyResolver.Current.GetService<IMembershipProvider>();
            //_provider.Initialize("", new NameValueCollection());
            _provider = (CustomMembershipProvider)Membership.Providers["CustomMembershipProvider"];
        }

        public IEnumerable<Domain.User> GetAllUsers()
        {
            return _provider.GetAllUsers();
        }

        public Domain.User GetUser(string username)
        {
            return _provider.GetUser(username);
        }

        public string GetUserNameByEmail(string email)
        {
            return _provider.GetUserNameByEmail(email);
        }

        public Domain.User CreateUser(Domain.User user, out MembershipCreateStatus status)
        {
            return _provider.CreateUser(user, out status);
        }

        public void UpdateUser(Domain.User user)
        {
            _provider.UpdateUser(user);
        }

        public bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            return _provider.DeleteUser(username, deleteAllRelatedData);
        }

        public IEnumerable<Domain.User> FilterUsers(IEnumerable<Domain.User> users, string searchText)
        {
            return _provider.FilterUsers(users, searchText);
        }
    }
}
