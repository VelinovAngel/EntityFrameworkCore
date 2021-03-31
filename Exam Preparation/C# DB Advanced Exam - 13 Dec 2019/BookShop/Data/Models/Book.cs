using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Data.Models
{
    public class Book
    {
        public int MyProperty { get; set; }
        //•	Id - integer, Primary Key
        //•	Name - text with length[3, 30]. (required)
        //•	Genre - enumeration of type Genre, with possible values(Biography = 1, Business = 2, Science = 3) (required)
        //•	Price - decimal in range between 0.01 and max value of the decimal
        //•	Pages – integer in range between 50 and 5000
        //•	PublishedOn - date and time(required)
        //•	AuthorsBooks - collection of type AuthorBook

    }
}
