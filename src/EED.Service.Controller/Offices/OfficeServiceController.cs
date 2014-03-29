using EED.Domain;
using EED.Service.Offices;
using EED.Service.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Controller.Offices
{
    public class OfficeServiceController : IOfficeServiceController
    {
        private readonly IOfficeService _service;
        private readonly IProjectService _projectService;

        public OfficeServiceController(IOfficeService service, IProjectService projectService)
        {
            _service = service;
            _projectService = projectService;
        }

        public ElectionProject FindProject(int id)
        {
            return _projectService.FindProject(id);
        }

        public Office FindOffice(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Office> FilterOffices(IEnumerable<DistrictType> districtTypes, string searchText)
        {
            throw new NotImplementedException();
        }

        public void SaveOffice(Office office)
        {
            throw new NotImplementedException();
        }

        public void DeleteOffice(Office office)
        {
            throw new NotImplementedException();
        }
    }
}
