namespace RealEstates.Models
{
    public class Property
    {
        public int Id { get; set; }

        public int Size { get; set; }

        public int YardSize { get; set; }

        public byte? Floor { get; set; }

        public byte? TotalFloors { get; set; }

        public int DistrictId { get; set; }

        public virtual District District { get; set; }
    }
}
