using EED.DAL;
using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EED.Service.District_Type
{
    public class DistrictTypeService : IDistrictTypeService
    {
        private readonly IRepository<DistrictType> _repository;

        public DistrictTypeService(IRepository<DistrictType> repository)
        {
            _repository = repository;
        }

        public IEnumerable<DistrictType> FindAllDistrictTypes()
        {
            return _repository.FindAll();
        }

        public IEnumerable<DistrictType> FindAllDistrictTypesFromProject(int projectId)
        {
            var districtTypes = FindAllDistrictTypes().Where(dt => dt.Project.Id == projectId);
            
            return districtTypes;
        }

        public DistrictType FindDistrictType(int id)
        {
            return _repository.Find(id);
        }

        public IEnumerable<DistrictType> FilterDistrictTypes(IEnumerable<DistrictType> districtTypes,
            string searchText)
        {
            string text = searchText.Trim();
            if (!String.IsNullOrEmpty(text))
            {
                districtTypes = districtTypes
                    .Where(dt => (String.Equals(dt.Name, text, StringComparison.CurrentCultureIgnoreCase)));
            }

            return districtTypes;
        }

        public void SaveDistrictType(DistrictType districtType)
        {
            try
            {
                _repository.Save(districtType);
            }
            catch (Exception ex)
            {
                throw new Exception("Error processing project data - " + ex.Message);
            }
        }

        public void DeleteDistrictType(DistrictType districtType)
        {
            try
            {
                _repository.Delete(districtType);
            }
            catch (Exception ex)
            {
                throw new Exception("Error processing project data - " + ex.Message);
            }
        }
    }
}
