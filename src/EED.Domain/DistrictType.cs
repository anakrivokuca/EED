
using System.Collections.Generic;
namespace EED.Domain
{
    public class DistrictType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public DistrictType ParentDistrictType { get; set; }
        public ElectionProject Project { get; set; }

        public IList<DistrictType> ChildDistrictTypes { get; set; }
        public IList<District> Districts { get; set; }
        public IList<Office> Offices { get; set; }
    }
}
