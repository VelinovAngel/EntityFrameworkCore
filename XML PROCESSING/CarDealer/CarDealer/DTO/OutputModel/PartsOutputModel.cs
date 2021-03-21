using System.Xml.Serialization;

namespace CarDealer.DTO.OutputModel
{
    [XmlType("part")]
    public class PartsOutputModel
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }
    }
}
