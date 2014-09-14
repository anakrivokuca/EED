using EED.Domain;
using System.Collections.Generic;

namespace EED.Service.Controller.Districts
{
    public interface IDistrictServiceController
    {
        ElectionProject FindProject(int id);
        District FindDistrict(int id);
        IEnumerable<District> FilterDistricts(IEnumerable<District> districts,
            string searchText, int districtTypeId);
        void SaveDistrict(District district);
        void DeleteDistrict(District district);
    }
}
