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
        
        public IEnumerable<Precinct> FindAllPrecinctsFromProject(int projectId)
        {
            var precincts = FindAllPrecincts().Where(p => p.Project.Id == projectId);

            return precincts;
        }
    }
}
