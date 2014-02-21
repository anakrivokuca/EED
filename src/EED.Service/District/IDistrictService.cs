using System.Collections.Generic;

namespace EED.Service.District
{
    public interface IDistrictService
    {
        IEnumerable<Domain.District> FindAllDistricts();
        Domain.District FindDistrict(int id);
        IEnumerable<Domain.District> FilterDistricts(IEnumerable<Domain.District> districts,
            string searchText, int districtTypeId);
        void SaveDistrict(Domain.District district);
        void DeleteDistrict(Domain.District district);
    }
}
