using EED.Domain;
using System.Collections.Generic;

namespace EED.Service.Election_Type
{
    public interface IElectionTypeService
    {
        IEnumerable<ElectionType> FindAllElectionTypes();
    }
}
