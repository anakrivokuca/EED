using EED.DAL;
using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Election_Type
{
    public class ElectionTypeService : IElectionTypeService
    {
        private readonly IRepository<ElectionType> _repository;

        public ElectionTypeService(IRepository<ElectionType> repository)
        {
            _repository = repository;
        }

        public IEnumerable<ElectionType> FindAllElectionTypes()
        {
            return _repository.FindAll();
        }
    }
}
