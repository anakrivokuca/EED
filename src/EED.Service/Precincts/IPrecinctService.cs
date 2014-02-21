using EED.Domain;
using System.Collections.Generic;

namespace EED.Service.Precincts
{
    public interface IPrecinctService
    {
        IEnumerable<Precinct> FindAllPrecincts();
        Precinct FindPrecinct(int id);
        void SavePrecinct(Precinct precinct);
        void DeletePrecinct(Precinct precinct);
    }
}
