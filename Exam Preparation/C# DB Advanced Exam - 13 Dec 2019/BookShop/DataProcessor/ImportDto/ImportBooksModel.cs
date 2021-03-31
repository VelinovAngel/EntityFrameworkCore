namespace BookShop.DataProcessor.ImportDto
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    using BookShop.Data.Models.Enums;

    [XmlType("Book")]
    public class ImportBooksModel
    {
        [Required]
        [MinLength(3), MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }


        [Range(50, 5000)]
        public int Pages { get; set; }

        public DateTime PublishedOn { get; set; }
    }
}
