
namespace EED.Domain
{
    public class DistrictType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public DistrictType ParentDistrictType { get; set; }
        public ElectionProject Project { get; set; }
    }
}
