using System;
using System.Xml.Serialization;
using VaporStore.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models;

namespace VaporStore.ImportResults
{
    [XmlType("Purchase")]
    public class InputPurchaseDTO
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        
        [Required]
        [XmlElement("Type")]
        public string Type { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$")]
        [XmlElement("Key")]
        public string ProductKey { get; set; }

        [Required]
        [XmlElement("Card")]
        public string Card { get; set; }

        [Required]
        [XmlElement("Date")]
        public string Date { get; set; }
    }
}
