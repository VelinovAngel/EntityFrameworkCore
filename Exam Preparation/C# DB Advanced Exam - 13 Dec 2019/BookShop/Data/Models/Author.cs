using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace BookShop.Data.Models
{
    public class Author
    {
        public Author()
        {
            this.AuthorsBooks = new HashSet<AuthorBook>();
        }
        public int Id { get; set; }
        //•	Id - integer, Primary Key

        [Required]
        [MinLength(3), MaxLength(30)]
        public string FirstName { get; set; }
        //•	FirstName - text with length[3, 30]. (required)

        [Required]
        [MinLength(3), MaxLength(30)]
        public string LastName { get; set; }
        //•	LastName - text with length[3, 30]. (required)

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        //•	Email - text(required). Validate it! There is attribute for this job.

        [Required]
        [RegularExpression(@"^[0-9]{3}-[0-9]{3}-[0-9]{4}$")]
        public string Phone { get; set; }
        //•	Phone - text.Consists only of three groups(separated by '-'), the first two consist of three digits and the last one - of 4 digits. (required)

        [JsonIgnore]
        public ICollection<AuthorBook> AuthorsBooks { get; set; }
        //•	AuthorsBooks - collection of type AuthorBook
    }
}
