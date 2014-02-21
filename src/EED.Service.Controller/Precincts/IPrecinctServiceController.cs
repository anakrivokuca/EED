using EED.Domain;
using System.Collections.Generic;

namespace EED.Service.Controller.Precincts
{
    public interface IPrecinctServiceController
    {
        ElectionProject FindProject(int id);
        IEnumerable<Precinct> FindAllPrecincts();
        Precinct FindPrecinct(int id);
        void SavePrecinct(Precinct precinct);
        void DeletePrecinct(Precinct precinct);
    }
}
