﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ExportDto
{
    [XmlType("Book")]
    public class ExportBooksDto
    {
        [XmlAttribute("Pages")]
        public string Pages { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Data")]
        public string Data { get; set; }
    }
}
