using EED.DAL;
using EED.Domain;
using EED.Service.District;
using EED.Service.District_Type;
using EED.Service.Election_Type;
using EED.Service.Jurisdiction_Type;
using EED.Service.Membership_Provider;
using EED.Service.Project;
using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

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
            _kernel.Bind<IRepository<DistrictType>>().To<Repository<DistrictType>>();
            _kernel.Bind<IRepository<District>>().To<Repository<District>>();

            _kernel.Bind<IMembershipProvider>().To<CustomMembershipProvider>();
            _kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
            _kernel.Bind<IProjectService>().To<ProjectService>();
            _kernel.Bind<IJurisdictionTypeService>().To<JurisdictionTypeService>();
            _kernel.Bind<IElectionTypeService>().To<ElectionTypeService>();
            _kernel.Bind<IDistrictTypeService>().To<DistrictTypeService>();
            _kernel.Bind<IDistrictService>().To<DistrictService>();
        }

    }
}
