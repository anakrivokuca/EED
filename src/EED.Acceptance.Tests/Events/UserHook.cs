using EED.Domain;
using EED.Infrastructure;
using EED.Service.Controller.User;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using TechTalk.SpecFlow;

namespace EED.Acceptance.Tests.Events
{
    [Binding]
    public class UserHook
    {
        private static IUserServiceController _serviceController;
        private static IEnumerable<User> _users;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            DependencyResolver.SetResolver(new NinjectDependencyResolver());
            _serviceController = DependencyResolver.Current.GetService<IUserServiceController>();

            _users = new List<User> { new User { Name = "Ana", Surname = "Krivokuca", 
                Email = "anakrivokuca@gmail.com", UserName = "anakrivokuca", 
                Password = "anakrivokuca123!" },
                new User { Name = "John", Surname = "Doe", Email = "johndoe@ny.com", 
                    UserName = "johndoe", Password = "johndoe123!" },
                new User { Name = "Jane", Surname = "Smith", Email = "janesmith@oklahoma.com", 
                    UserName = "janesmith", Password = "jjanesmith123!" } };

            var status = new MembershipCreateStatus();
            foreach (var user in _users)
            {
                _serviceController.CreateUser(user, out status);
            }
        }

        [AfterScenario("addNewUser")]
        public void AfterScenario()
        {
            _serviceController.DeleteUser("janemclaren", true);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            _users = _serviceController.GetAllUsers();

            foreach (var user in _users)
            {
                _serviceController.DeleteUser(user.UserName, true);
            }
        }
    }
}