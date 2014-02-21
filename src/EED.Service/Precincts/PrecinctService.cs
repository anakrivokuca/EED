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

        public IEnumerable<Precinct> FilterPrecincts(IEnumerable<Precinct> precincts,
            string searchText, int districtId)
        {
            string text = searchText.Trim();
            if (!String.IsNullOrEmpty(text))
            {
                precincts = precincts
                    .Where(p => (String.Equals(p.Name, text, StringComparison.CurrentCultureIgnoreCase)));
            }

            if (districtId != 0)
            {
                precincts = precincts
                    .Where(p => p.Districts != null && p.Districts.Select(d => d.Id).ToArray().Contains(districtId));
            }

            return precincts;
        }

        public void SavePrecinct(Precinct precinct)
        {
            try
            {
                if (precinct.Districts != null)
                {
                    var selectedDistricts = precinct.Districts;
                    var districts = precinct.Districts;

                    while (selectedDistricts.Count() != 0)
                    {
                        var districtIds = districts.Select(sd => sd.Id).ToArray();

                        var parentDistricts = selectedDistricts
                            .Where(sd => sd.ParentDistrict != null &&
                                !districtIds.Contains(sd.ParentDistrict.Id))
                            .Select(sd => sd.ParentDistrict)
                            .Distinct();

                        districts = districts.Concat(parentDistricts).ToList();
                        selectedDistricts = parentDistricts.ToList();
                    }
                    precinct.Districts = districts.ToList();
                }

                _repository.Save(precinct);
            }
            catch (Exception ex)
            {
                throw new Exception("Error processing project data - " + ex.Message);
            }
        }
        
        public void DeletePrecinct(Precinct precinct)
        {
            try
            {
                _repository.Delete(precinct);
            }
            catch (Exception ex)
            {
                throw new Exception("Error processing project data - " + ex.Message);
            }
        }
    }
}
