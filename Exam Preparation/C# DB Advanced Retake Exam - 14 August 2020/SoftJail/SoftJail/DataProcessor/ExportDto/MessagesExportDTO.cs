using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{

    [XmlType("Message")]
    public class MessagesExportDTO
    {
        public string Description { get; set; }
    }
}
