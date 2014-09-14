using EED.Domain;
using System.Collections.Generic;

namespace EED.Service.Political_Party
{
    public interface IPoliticalPartyService
    {
        IEnumerable<PoliticalParty> FindAllPoliticalParties();
        PoliticalParty FindPoliticalParty(int id);
        IEnumerable<PoliticalParty> FilterPoliticalParties(IEnumerable<PoliticalParty> politicalParties,
            string searchText);
        void SavePoliticalParty(PoliticalParty politicalParty);
        void DeletePoliticalParty(PoliticalParty politicalParty);
    }
}
