using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EED.DAL;
using EED.Domain;
using EED.Service;
using Ninject;

namespace EED.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver()
        {
            _kernel = new StandardKernel();
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<IRepository<User>>().To<Repository<User>>();
            _kernel.Bind<IUserService>().To<UserService>();
        }

    }
}
