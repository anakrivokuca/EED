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
            _users = new List<User> { new User { 
                    Name = "Jane", Surname = "Smith", Email = "janesmith@oklahoma.com", 
                    State = "US", UserName = "janesmith", Password = "janesmith123!", 
                    IsApproved = true },
                new User { Name = "User2", Email = "user2@us.com", 
                    UserName = "user2", Password = "user2123!" },
                new User { Name = "User3", Email = "user3@us.com", 
                    UserName = "user3", Password = "user3123!" },
                new User { Name = "User4", Email = "user4@us.com", 
                    UserName = "user4", Password = "user4123!" },
                new User { Name = "User5", Email = "user5@us.com", 
                    UserName = "user5", Password = "user5123!" },
                new User { Name = "User6", Email = "user6@us.com", 
                    UserName = "user6", Password = "user6123!" },
                new User { Name = "User7", Email = "user7@us.com", 
                    UserName = "user7", Password = "user7123!" },
                new User { Name = "User8", Email = "user8@us.com", 
                    UserName = "user8", Password = "user8123!" },
                new User { Name = "User9", Email = "user9@us.com", 
                    UserName = "user9", Password = "user9123!" },
                new User { Name = "User10", Email = "user10@us.com", 
                    UserName = "user10", Password = "user10123!" },
                new User { Name = "User11", Email = "user11@us.com", 
                    UserName = "user11", Password = "user11123!" }};

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