using EED.DAL;
using EED.Domain;
using System.Collections.Generic;

namespace EED.Service.Jurisdiction_Type
{
    public class JurisdictionTypeService : IJurisdictionTypeService
    {
        private readonly IRepository<JurisdictionType> _repository;

        public JurisdictionTypeService(IRepository<JurisdictionType> repository)
        {
            _repository = repository;
        }

        public IEnumerable<JurisdictionType> FindAllJurisdictionTypes()
        {
            return _repository.FindAll();
        }
    }
}
