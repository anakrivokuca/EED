using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Offices
{
    public interface IOfficeService
    {
        IEnumerable<Office> FindAllOffices();
        Office FindOffice(int id);
        IEnumerable<Office> FilterOffices(IEnumerable<Office> offices, string searchText);
        void SaveOffice(Office office);
        void DeleteOffice(Office office);
    }
}
