using EED.Domain;
using System.Collections.Generic;

namespace EED.Service.Jurisdiction_Type
{
    public interface IJurisdictionTypeService
    {
        IEnumerable<JurisdictionType> FindAllJurisdictionTypes();
        JurisdictionType FindJurisdictionType(int id);
    }
}
