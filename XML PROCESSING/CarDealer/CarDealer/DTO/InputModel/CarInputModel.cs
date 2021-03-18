using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DTO.InputModel
{
    [XmlType("Cars")]
    public class CarInputModel
    {
        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }


        public long TravelledDistance { get; set; }

        [XmlElement("parts")]
        public int[] Parts { get; set; }

    }
}
