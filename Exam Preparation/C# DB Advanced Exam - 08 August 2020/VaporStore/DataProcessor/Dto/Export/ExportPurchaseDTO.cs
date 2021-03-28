using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("Purchase")]
    public class ExportPurchaseDTO
    {
        [XmlElement("Card")]
        public string Card { get; set; }

        [XmlElement("Cvc")]
        public string CVC { get; set; }

        [XmlElement("Date")]
        public string Date { get; set; }

        public ExportGameDTO Game { get; set; }

    }
}
