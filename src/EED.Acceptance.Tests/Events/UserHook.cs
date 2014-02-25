using EED.Acceptance.Tests.ControllerObjects;
using EED.Acceptance.Tests.Utils;
using EED.Domain;
using EED.Service.Controller.User;
using System.Collections.Generic;
using System.Web.Security;
using TechTalk.SpecFlow;

namespace EED.Acceptance.Tests.Events
{
    [Binding]
    public class UserHook
    {
        private static IEnumerable<User> _users;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            _users = new List<User> { new User { Name = "Ana", Surname = "Krivokuca", 
                    Email = "anakrivokuca@gmail.com", UserName = "anakrivokuca", 
                    Password = "anakrivokuca123!", IsApproved = true },
                new User { Name = "Jane", Surname = "Smith", Email = "janesmith@oklahoma.com", 
                    State = "US", UserName = "janesmith", Password = "janesmith123!", 
                    IsApproved = true } };

            foreach (var user in _users)
            {
                DatabaseHelper.SaveOrUpdate(user);
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            _users = DatabaseHelper.FindAll<User>();

            foreach (var user in _users)
            {
                DatabaseHelper.Delete(user);
            }
        }
    }
}