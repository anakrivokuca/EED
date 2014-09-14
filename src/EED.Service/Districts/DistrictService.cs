using EED.DAL;
using EED.Domain;
using EED.Service.Project;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EED.Service.Districts
{
    public class DistrictService : IDistrictService
    {
        private readonly IRepository<District> _repository;

        public DistrictService(IRepository<District> repository)
        {
            _repository = repository;
        }

        public IEnumerable<District> FindAllDistricts()
        {
            return _repository.FindAll();
        }

        public District FindDistrict(int id)
        {
            return _repository.Find(id);
        }

        public IEnumerable<District> FilterDistricts(IEnumerable<District> districts, 
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

        public void SaveDistrict(District district)
        {
            try
            {
                if (district.Id != 0)
                {
                    var existingDistrict = FindDistrict(district.Id);
                    existingDistrict.Name = district.Name;
                    existingDistrict.Abbreviation = district.Abbreviation;
                    if (existingDistrict.ParentDistrict == null)
                    {
                        existingDistrict.Project.JurisdictionName = district.Name;
                    }
                    else
                    {
                        existingDistrict.ParentDistrict = district.ParentDistrict;
                    }
                    district = existingDistrict;
                }

                _repository.Save(district);
            }
            catch (Exception ex)
            {
                throw new Exception("Error processing project data - " + ex.Message);
            }
        }
        
        public void DeleteDistrict(District district)
        {
            _repository.Delete(district);
        }
    }
}
