using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EED.DAL;
using EED.Domain;
using Ninject;
using EED.Service.Membership_Provider;
using EED.Service.Project;
using EED.Service.Election_Type;
using EED.Service.Jurisdiction_Type;

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
            _kernel.Bind<IRepository<ElectionProject>>().To<Repository<ElectionProject>>();
            _kernel.Bind<IRepository<JurisdictionType>>().To<Repository<JurisdictionType>>();
            _kernel.Bind<IRepository<ElectionType>>().To<Repository<ElectionType>>();

            _kernel.Bind<IMembershipProvider>().To<CustomMembershipProvider>();
            _kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
            _kernel.Bind<IProjectService>().To<ProjectService>();
            _kernel.Bind<IJurisdictionTypeService>().To<JurisdictionTypeService>();
            _kernel.Bind<IElectionTypeService>().To<ElectionTypeService>();
        }

    }
}
