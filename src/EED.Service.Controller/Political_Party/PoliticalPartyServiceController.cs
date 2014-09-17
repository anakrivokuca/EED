using EED.Domain;
using EED.Service.Political_Party;
using EED.Service.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Controller.Political_Party
{
    public class PoliticalPartyServiceController : IPoliticalPartyServiceController
    {
        private readonly IPoliticalPartyService _service;
        private readonly IProjectService _projectService;

        public PoliticalPartyServiceController(IPoliticalPartyService service, IProjectService projectService)
        {
            _service = service;
            _projectService = projectService;
        }

        public ElectionProject FindProject(int id)
        {
            return _projectService.FindProject(id);
        }

        public PoliticalParty FindPoliticalParty(int id)
        {
            return _service.FindPoliticalParty(id);
        }

        public IEnumerable<PoliticalParty> FilterPoliticalParties(IEnumerable<PoliticalParty> politicalParties, string searchText)
        {
            return _service.FilterPoliticalParties(politicalParties, searchText);
        }

        public void SavePoliticalParty(PoliticalParty politicalParty)
        {
            _service.SavePoliticalParty(politicalParty);
        }

        public void DeletePoliticalParty(PoliticalParty politicalParty)
        {
            throw new NotImplementedException();
        }
    }
}
