namespace BookShop.DataProcessor.ImportDto
{
    using System;
    using System.Xml.Serialization;

    using BookShop.Data.Models.Enums;

    [XmlType("Book")]
    public class ImportBooksModel
    {
        public string Name { get; set; }

        public Genre Genre { get; set; }

        public decimal Price { get; set; }

        public int Pages { get; set; }

        public DateTime PublishedOn { get; set; }
    }
}
