using EED.Domain;
using System.Collections.Generic;

namespace EED.Service.Controller.Precincts
{
    public interface IPrecinctServiceController
    {
        ElectionProject FindProject(int id);
        Precinct FindPrecinct(int id);
        IEnumerable<Precinct> FilterPrecincts(IEnumerable<Precinct> precincts,
            string searchText, int districtId);
        void SavePrecinct(Precinct precinct);
        void DeletePrecinct(Precinct precinct);
    }
}
