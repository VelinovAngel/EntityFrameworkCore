
namespace BookShop.DataProcessor.ImportDto
{
    using BookShop.Data.Models;
    using System.ComponentModel.DataAnnotations;
    public class ImportAuthorsModel
    {
        [Required]
        [MinLength(3), MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3), MaxLength(30)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{3}-[0-9]{3}-[0-9]{4}$")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public BookInputModel[] Books { get; set; }
    }

    public class BookInputModel
    {
        [Required]
        public int? Id { get; set; }
    }
}
