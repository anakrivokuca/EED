using EED.Domain;
using EED.Service.Precincts;
using EED.Service.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Controller.Precincts
{
    public class PrecinctServiceController : IPrecinctServiceController
    {
        private readonly IPrecinctService _service;
        private readonly IProjectService _projectService;

        public PrecinctServiceController(IPrecinctService service, IProjectService projectService)
        {
            _service = service;
            _projectService = projectService;
        }

        public ElectionProject FindProject(int id)
        {
            return _projectService.FindProject(id);
        }

        public IEnumerable<Precinct> FindAllPrecincts()
        {
            return _service.FindAllPrecincts();
        }
        
        public Precinct FindPrecinct(int id)
        {
            return _service.FindPrecinct(id);
        }

        public void SavePrecinct(Precinct precinct)
        {
            _service.SavePrecinct(precinct);
        }

        public void DeletePrecinct(Precinct precinct)
        {
            _service.DeletePrecinct(precinct);
        }
    }
}
