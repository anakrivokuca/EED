using EED.Domain;
using System.Collections.Generic;

namespace EED.Service.Controller.Political_Party
{
    public interface IPoliticalPartyServiceController
    {
        ElectionProject FindProject(int id);
        PoliticalParty FindPoliticalParty(int id);
        IEnumerable<PoliticalParty> FilterPoliticalParties(IEnumerable<PoliticalParty> politicalParties,
            string searchText);
        void SavePoliticalParty(PoliticalParty politicalParty);
        void DeletePoliticalParty(PoliticalParty politicalParty);
    }
}
