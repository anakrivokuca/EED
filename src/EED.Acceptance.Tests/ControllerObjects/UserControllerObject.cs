using EED.Infrastructure;
using EED.Service.Controller.User;
using EED.Ui.Web.Controllers;
using EED.Ui.Web.Models.User;
using System.Web.Mvc;

namespace EED.Acceptance.Tests.ControllerObjects
{
    public static class UserControllerObject
    {
        public static IUserServiceController ServiceController
        {
            get 
            { 
                DependencyResolver.SetResolver(new NinjectDependencyResolver());
                return DependencyResolver.Current.GetService<IUserServiceController>();
            }
        }

        public static UserController Controller
        {
            get
            {
                return new UserController(ServiceController);
            }
        }
    }
}
