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
