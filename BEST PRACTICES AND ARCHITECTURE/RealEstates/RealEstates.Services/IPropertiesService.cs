namespace RealEstates.Services
{
    public interface IPropertiesService
    {
        void Add(string district, int floor, int maxFloor, int size, int yardSize, int year, string propertyType, string buildingType, int price);
    }
}
