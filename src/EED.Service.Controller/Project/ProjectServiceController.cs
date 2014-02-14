using EED.Domain;
using EED.Service.Election_Type;
using EED.Service.Jurisdiction_Type;
using EED.Service.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Controller.Project
{
    public class ProjectServiceController : IProjectServiceController
    {
        private readonly IProjectService _service;
        private readonly IJurisdictionTypeService _jurisdictionTypeService;
        private readonly IElectionTypeService _electionTypeService;

        public ProjectServiceController(IProjectService service, IJurisdictionTypeService jurisdictionTypeService, 
            IElectionTypeService electionTypeService)
        {
            _service = service;
            _jurisdictionTypeService = jurisdictionTypeService;
            _electionTypeService = electionTypeService;
        }

        public IEnumerable<ElectionType> FindAllElectionTypes()
        {
            return _electionTypeService.FindAllElectionTypes();
        }

        public IEnumerable<JurisdictionType> FindAllJurisdictionTypes()
        {
            return _jurisdictionTypeService.FindAllJurisdictionTypes();
        }

        public JurisdictionType FindJurisdictionType(int id)
        {
            return _jurisdictionTypeService.FindJurisdictionType(id);
        }

        public IEnumerable<ElectionProject> FindAllProjects()
        {
            return _service.FindAllProjects();
        }

        public IEnumerable<ElectionProject> FindAllProjectsFromUser()
        {
            return _service.FindAllProjectsFromUser();
        }

        public ElectionProject FindProject(int id)
        {
            return _service.FindProject(id);
        }

        public IEnumerable<ElectionProject> FilterProjects(IEnumerable<ElectionProject> projects, string searchText)
        {
            return _service.FilterProjects(projects, searchText);
        }

        public void SaveProject(ElectionProject project)
        {
            _service.SaveProject(project);
        }

        public void DeleteProject(ElectionProject project)
        {
            _service.DeleteProject(project);
        }
    }
}
