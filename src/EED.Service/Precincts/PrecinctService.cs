using EED.DAL;
using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Service.Precincts
{
    public class PrecinctService : IPrecinctService
    {
        private readonly IRepository<Precinct> _repository;

        public PrecinctService(IRepository<Precinct> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Precinct> FindAllPrecincts()
        {
            return _repository.FindAll();
        }

        public Precinct FindPrecinct(int id)
        {
            return _repository.Find(id);
        }

        public void SavePrecinct(Precinct precinct)
        {
            _repository.Save(precinct);
        }
    }
}
