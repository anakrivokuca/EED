using EED.DAL;
using EED.Domain;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace EED.Service.Membership_Provider
{
    public class FormsAuthProvider : IAuthProvider
    {
        private readonly IRepository<User> _repository;

        public FormsAuthProvider(IRepository<User> repository)
        {
            _repository = repository;
        }

        public User GetUserFromCookie()
        {
            FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;
            string username = ticket.Name;

            var user = _repository.FindAll().SingleOrDefault
                    (u => u.UserName == username);

            return user;
        }

        public bool Authenticate(string username, string password)
        {
            bool result = Membership.ValidateUser(username, password);

            if (result)
            {
                FormsAuthentication.SetAuthCookie(username, false);
            }
            return result;
        }

        public void Logout(User user)
        {
            FormsAuthentication.SignOut();
            user.IsOnline = false;
            _repository.Save(user);
        }
    }
}
