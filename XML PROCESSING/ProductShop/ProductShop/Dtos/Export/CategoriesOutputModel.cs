﻿using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("Category")]
    public class CategoriesOutputModel
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("cout")]

        public int Count { get; set; }

        [XmlElement("averagePrice")]

        public decimal AveragePrice { get; set; }

        [XmlElement("totalRevenue")]

        public decimal TotalRevenue { get; set; }
    }
}
