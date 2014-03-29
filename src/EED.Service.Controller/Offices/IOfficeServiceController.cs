using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Controller.Offices
{
    public interface IOfficeServiceController
    {
        ElectionProject FindProject(int id);
        Office FindOffice(int id);
        IEnumerable<Office> FilterOffices(IEnumerable<DistrictType> districtTypes, string searchText);
        void SaveOffice(Office office);
        void DeleteOffice(Office office);
    }
}
