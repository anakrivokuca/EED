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
            return _service.FindOffice(id);
        }

        public IEnumerable<Office> FilterOffices(IEnumerable<Office> offices, string searchText)
        {
            return _service.FilterOffices(offices, searchText);
        }

        public void SaveOffice(Office office)
        {
            _service.SaveOffice(office);
        }

        public void DeleteOffice(Office office)
        {
            _service.DeleteOffice(office);
        }
    }
}
