﻿using EED.Domain;
using System.Collections.Generic;

namespace EED.Service.Controller.District_Type
{
    public interface IDistrictTypeServiceController
    {
        IEnumerable<DistrictType> FindAllDistrictTypesFromProject(int projectId);
        DistrictType FindDistrictType(int id);
        IEnumerable<DistrictType> FilterDistrictTypes(IEnumerable<DistrictType> districtTypes,
            string searchText);
        void SaveDistrictType(DistrictType districtType);
        void DeleteDistrictType(DistrictType districtType);
    }
}
