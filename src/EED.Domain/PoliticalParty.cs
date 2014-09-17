using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Domain
{
    public class PoliticalParty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public byte[] Image { get; set; }
        public ElectionProject Project { get; set; }
    }
}
