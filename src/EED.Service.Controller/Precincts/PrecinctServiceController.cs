using EED.Domain;
using EED.Service.Precincts;
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

        public PrecinctServiceController(IPrecinctService service)
        {
            _service = service;
        }

        public IEnumerable<Precinct> FindAllPrecincts()
        {
            return _service.FindAllPrecincts();
        }

        public IEnumerable<Precinct> FindAllPrecinctsFromProject(int projectId)
        {
            return _service.FindAllPrecinctsFromProject(projectId);
        }
    }
}
