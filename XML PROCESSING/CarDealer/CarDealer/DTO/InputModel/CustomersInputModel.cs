﻿using System;
using System.Xml.Serialization;

namespace CarDealer.DTO.InputModel
{
    [XmlType("Customer")]
    public class CustomersInputModel
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("birthDate")]
        public DateTime BirthDate { get; set; }

        [XmlElement("isYoungDriver")]
        public bool IsYoungDriver { get; set; }


    }
}
