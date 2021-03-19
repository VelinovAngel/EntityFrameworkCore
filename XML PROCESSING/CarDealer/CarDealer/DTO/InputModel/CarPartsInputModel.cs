using System.Xml.Serialization;

namespace CarDealer.DTO.InputModel
{
    [XmlType("partId")]
    public class CarPartsInputModel
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}