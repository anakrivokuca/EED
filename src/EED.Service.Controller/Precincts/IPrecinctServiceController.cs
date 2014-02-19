using EED.Domain;
using System.Collections.Generic;

namespace EED.Service.Controller.Precincts
{
    public interface IPrecinctServiceController
    {
        IEnumerable<Precinct> FindAllPrecincts();
        IEnumerable<Precinct> FindAllPrecinctsFromProject(int projectId);
    }
}
