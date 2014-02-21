using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Domain
{
    public class District
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public DistrictType DistrictType { get; set; }
        public District ParentDistrict { get; set; }
        public ElectionProject Project { get; set; }

        public IList<District> ChildDistricts { get; set; }
        public IList<Precinct> Precincts { get; set; }
    }
}
