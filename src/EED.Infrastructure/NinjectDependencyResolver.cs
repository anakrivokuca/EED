using EED.DAL;
using EED.Domain;
using EED.Service.Contests;
using EED.Service.Controller.Account;
using EED.Service.Controller.Contests;
using EED.Service.Controller.Districts;
using EED.Service.Controller.District_Type;
using EED.Service.Controller.Offices;
using EED.Service.Controller.Precincts;
using EED.Service.Controller.Project;
using EED.Service.Controller.User;
using EED.Service.Districts;
using EED.Service.District_Type;
using EED.Service.Election_Type;
using EED.Service.Jurisdiction_Type;
using EED.Service.Membership_Provider;
using EED.Service.Offices;
using EED.Service.Precincts;
using EED.Service.Project;
using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EED.Service.Political_Party;
using EED.Service.Controller.Political_Party;

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
            _kernel.Bind<IRepository<Precinct>>().To<Repository<Precinct>>();
            _kernel.Bind<IRepository<Office>>().To<Repository<Office>>();
            _kernel.Bind<IRepository<Contest>>().To<Repository<Contest>>();
            _kernel.Bind<IRepository<PoliticalParty>>().To<Repository<PoliticalParty>>();

            _kernel.Bind<IMembershipProvider>().To<CustomMembershipProvider>();
            _kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
            _kernel.Bind<IProjectService>().To<ProjectService>();
            _kernel.Bind<IJurisdictionTypeService>().To<JurisdictionTypeService>();
            _kernel.Bind<IElectionTypeService>().To<ElectionTypeService>();
            _kernel.Bind<IDistrictTypeService>().To<DistrictTypeService>();
            _kernel.Bind<IDistrictService>().To<DistrictService>();
            _kernel.Bind<IPrecinctService>().To<PrecinctService>();
            _kernel.Bind<IOfficeService>().To<OfficeService>();
            _kernel.Bind<IContestService>().To<ContestService>();
            _kernel.Bind<IPoliticalPartyService>().To<PoliticalPartyService>();

            _kernel.Bind<IAccountServiceController>().To<AccountServiceController>();
            _kernel.Bind<IUserServiceController>().To<UserServiceController>();
            _kernel.Bind<IProjectServiceController>().To<ProjectServiceController>();
            _kernel.Bind<IDistrictTypeServiceController>().To<DistrictTypeServiceController>();
            _kernel.Bind<IDistrictServiceController>().To<DistrictServiceController>();
            _kernel.Bind<IPrecinctServiceController>().To<PrecinctServiceController>();
            _kernel.Bind<IOfficeServiceController>().To<OfficeServiceController>();
            _kernel.Bind<IPoliticalPartyServiceController>().To<PoliticalPartyServiceController>();
        }
    }
}
