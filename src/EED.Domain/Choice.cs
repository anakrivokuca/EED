using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Domain
{
    public class Choice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Contest Contest { get; set; }
        public ElectionProject Project { get; set; }

        public IList<PoliticalParty> PoliticalParties { get; set; }
    }
}
