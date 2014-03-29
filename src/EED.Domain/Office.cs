
namespace EED.Domain
{
    public class Office
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfPositions { get; set; }
        public OfficeType OfficeType { get; set; }
        public DistrictType DistrictType { get; set; }
        public ElectionProject Project { get; set; }
    }

    public enum OfficeType
    {
        Candidacy = 0,
        Measurement = 1
    }
}
