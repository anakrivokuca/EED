using EED.Domain;
using System.Collections.Generic;

namespace EED.Service.Precincts
{
    public interface IPrecinctService
    {
        IEnumerable<Precinct> FindAllPrecincts();
        IEnumerable<Precinct> FindAllPrecinctsFromProject(int projectId);
    }
}
