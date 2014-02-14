using EED.DAL;
using EED.Service.Project;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EED.Service.District
{
    public class DistrictService : IDistrictService
    {
        private readonly IRepository<Domain.District> _repository;
        //private readonly IProjectService _projectService;

        public DistrictService(IRepository<Domain.District> repository
            //, IProjectService projectService
            )
        {
            _repository = repository;
            //_projectService = projectService;
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

        public Domain.District FindDistrict(int id)
        {
            return _repository.Find(id);
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
                if (district.Id != 0)
                {
                    var existingDistrict = FindDistrict(district.Id);
                    existingDistrict.Name = district.Name;
                    existingDistrict.Abbreviation = district.Abbreviation;
                    existingDistrict.Project.JurisdictionName = district.Name;
                    district = existingDistrict;
                }

                _repository.Save(district);
            }
            catch (Exception ex)
            {
                throw new Exception("Error processing project data - " + ex.Message);
            }
        }
    }
}
