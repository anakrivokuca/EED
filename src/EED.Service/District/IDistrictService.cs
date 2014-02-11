using System.Collections.Generic;

namespace EED.Service.District
{
    public interface IDistrictService
    {
        IEnumerable<Domain.District> FindAllDistricts();
        IEnumerable<Domain.District> FindAllDistrictsFromProject(int projectId);
        IEnumerable<Domain.District> FilterDistricts(IEnumerable<Domain.District> districts,
            string searchText, int parentDistrictId);
        void SaveDistrict(Domain.District district);
    }
}
