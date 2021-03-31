using BookShop.Data.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Data.Models
{
    public class Book
    {
        public Book()
        {
            this.AuthorsBooks = new HashSet<AuthorBook>();
        }

        public int Id { get; set; }
        //•	Id - integer, Primary Key

        [Required]
        [MinLength(3), MaxLength(30)]
        public string Name { get; set; }
        //•	Name - text with length[3, 30]. (required)

        [Required]
        public Genre Genre { get; set; }
        //•	Genre - enumeration of type Genre, with possible values(Biography = 1, Business = 2, Science = 3) (required)

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        //•	Price - decimal in range between 0.01 and max value of the decimal

        [Range(50, 5000)]
        public int Pages { get; set; }
        //•	Pages – integer in range between 50 and 5000

        public DateTime PublishedOn { get; set; }
        //•	PublishedOn - date and time(required)

        [JsonIgnore]
        public ICollection<AuthorBook> AuthorsBooks { get; set; }
        //•	AuthorsBooks - collection of type AuthorBook

    }
}
