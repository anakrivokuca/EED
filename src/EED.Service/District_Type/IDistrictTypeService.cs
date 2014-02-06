using EED.Domain;
using System.Collections.Generic;

namespace EED.Service.District_Type
{
    public interface IDistrictTypeService
    {
        IEnumerable<DistrictType> FindAllDistrictTypes();
        IEnumerable<DistrictType> FindAllDistrictTypesFromProject(int projectId);
        IEnumerable<DistrictType> FilterDistrictTypes(IEnumerable<DistrictType> districtTypes,
            string searchText);
        void SaveDistrictType(DistrictType districtType);
        void DeleteDistrictType(DistrictType districtType);
    }
}
