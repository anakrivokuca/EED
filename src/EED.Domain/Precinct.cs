using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Domain
{
    public class Precinct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfEligibleVoters { get; set; }
        public ElectionProject Project { get; set; }

        public IList<District> Districts { get; set; }
    }
}
