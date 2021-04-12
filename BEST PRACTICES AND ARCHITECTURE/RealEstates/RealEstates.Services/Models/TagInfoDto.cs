namespace RealEstates.Services.Models
{
    using System.Xml.Serialization;

    [XmlType("Tag")]
    public class TagInfoDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }
    }
}