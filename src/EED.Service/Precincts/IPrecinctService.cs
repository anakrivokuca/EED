using EED.Domain;
using System.Collections.Generic;

namespace EED.Service.Precincts
{
    public interface IPrecinctService
    {
        IEnumerable<Precinct> FindAllPrecincts();
        Precinct FindPrecinct(int id);
        IEnumerable<Precinct> FilterPrecincts(IEnumerable<Precinct> precincts,
            string searchText, int districtId);
        void SavePrecinct(Precinct precinct);
        void DeletePrecinct(Precinct precinct);
    }
}
