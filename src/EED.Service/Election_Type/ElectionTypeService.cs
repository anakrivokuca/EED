using EED.DAL;
using EED.Domain;
using System.Collections.Generic;

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
