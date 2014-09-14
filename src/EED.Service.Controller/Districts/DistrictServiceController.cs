using EED.Domain;
using EED.Service.Districts;
using EED.Service.Project;
using System;
using System.Collections.Generic;

namespace EED.Service.Controller.Districts
{
    public class DistrictServiceController : IDistrictServiceController
    {
        private readonly IDistrictService _service;
        private readonly IProjectService _projectService;

        public DistrictServiceController(IDistrictService service, IProjectService projectService)
        {
            _service = service;
            _projectService = projectService;
        }

        public ElectionProject FindProject(int id)
        {
            return _projectService.FindProject(id);
        }

        public Domain.District FindDistrict(int id)
        {
            return _service.FindDistrict(id);
        }

        public IEnumerable<Domain.District> FilterDistricts(IEnumerable<District> districts, string searchText, int parentDistrictId)
        {
            return _service.FilterDistricts(districts, searchText, parentDistrictId);
        }

        public void SaveDistrict(District district)
        {
            _service.SaveDistrict(district);
        }

        public void DeleteDistrict(District district)
        {
            _service.DeleteDistrict(district);
        }
    }
}
