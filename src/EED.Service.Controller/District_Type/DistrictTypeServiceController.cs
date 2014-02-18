using EED.Domain;
using EED.Service.District_Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Controller.District_Type
{
    public class DistrictTypeServiceController : IDistrictTypeServiceController
    {
        private readonly IDistrictTypeService _service;

        public DistrictTypeServiceController(IDistrictTypeService service)
        {
            _service = service;
        }

        public IEnumerable<DistrictType> FindAllDistrictTypes()
        {
            return _service.FindAllDistrictTypes();
        }

        public IEnumerable<DistrictType> FindAllDistrictTypesFromProject(int projectId)
        {
            return _service.FindAllDistrictTypesFromProject(projectId);
        }

        public DistrictType FindDistrictType(int id)
        {
            return _service.FindDistrictType(id);
        }

        public IEnumerable<DistrictType> FilterDistrictTypes(IEnumerable<DistrictType> districtTypes, string searchText)
        {
            return _service.FilterDistrictTypes(districtTypes, searchText);
        }

        public void SaveDistrictType(DistrictType districtType)
        {
            _service.SaveDistrictType(districtType);
        }

        public void DeleteDistrictType(DistrictType districtType)
        {
            _service.DeleteDistrictType(districtType);
        }
    }
}
