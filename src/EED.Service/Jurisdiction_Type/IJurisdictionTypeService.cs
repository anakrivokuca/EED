using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Jurisdiction_Type
{
    public interface IJurisdictionTypeService
    {
        IEnumerable<JurisdictionType> FindAllJurisdictionTypes();
    }
}
