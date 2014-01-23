using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Election_Type
{
    public interface IElectionTypeService
    {
        IEnumerable<ElectionType> FindAllElectionTypes();
    }
}
