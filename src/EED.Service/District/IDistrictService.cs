using System.Collections.Generic;

namespace EED.Service.District
{
    public interface IDistrictService
    {
        IEnumerable<Domain.District> FindAllDistricts();
        IEnumerable<Domain.District> FindAllDistrictsFromProject(int projectId);
        void SaveDistrict(Domain.District district);
    }
}
