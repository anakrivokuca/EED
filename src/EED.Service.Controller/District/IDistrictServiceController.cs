using EED.Domain;
using System.Collections.Generic;

namespace EED.Service.Controller.District
{
    public interface IDistrictServiceController
    {
        ElectionProject FindProject(int id);
        IEnumerable<Domain.District> FindAllDistricts();
        Domain.District FindDistrict(int id);
        IEnumerable<Domain.District> FilterDistricts(IEnumerable<Domain.District> districts,
            string searchText, int parentDistrictId);
        void SaveDistrict(Domain.District district);
        void DeleteDistrict(Domain.District district);
    }
}
