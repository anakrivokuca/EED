using System;
using System.Collections.Generic;

namespace EED.Domain
{
    public class ElectionProject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string JurisdictionName { get; set; }
        public User User { get; set; }
        public ElectionType ElectionType { get; set; }
        public JurisdictionType JurisdictionType { get; set; }

        public IList<DistrictType> DistrictTypes { get; set; }
        public IList<District> Districts { get; set; }
    }
}
