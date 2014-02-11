using EED.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EED.Service.District
{
    public class DistrictService : IDistrictService
    {
        private readonly IRepository<Domain.District> _repository;

        public DistrictService(IRepository<Domain.District> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Domain.District> FindAllDistricts()
        {
            return _repository.FindAll();
        }

        public IEnumerable<Domain.District> FindAllDistrictsFromProject(int projectId)
        {
            var districts = FindAllDistricts().Where(d => d.Project.Id == projectId);

            return districts;
        }

        public IEnumerable<Domain.District> FilterDistricts(IEnumerable<Domain.District> districts, 
            string searchText, int districtTypeId)
        {
            string text = searchText.Trim();
            if (!String.IsNullOrEmpty(text))
            {
                districts = districts
                    .Where(d => (String.Equals(d.Name, text, StringComparison.CurrentCultureIgnoreCase)));
            }

            if (districtTypeId != 0)
            {
                districts = districts
                    .Where(d => d.DistrictType != null && d.DistrictType.Id == districtTypeId);
            }

            return districts;
        }

        public void SaveDistrict(Domain.District district)
        {
            try
            {
                _repository.Save(district);
            }
            catch (Exception ex)
            {
                throw new Exception("Error processing project data - " + ex.Message);
            }
        }
    }
}
