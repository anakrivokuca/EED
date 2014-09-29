using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Domain
{
    public class Contest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfPositions { get; set; }
        public Office Office { get; set; }
        public District District { get; set; }
        public ElectionProject Project { get; set; }

        public IList<Choice> Choices { get; set; }
    }
}
