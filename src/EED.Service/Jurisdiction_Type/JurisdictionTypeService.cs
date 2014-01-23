using EED.DAL;
using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
