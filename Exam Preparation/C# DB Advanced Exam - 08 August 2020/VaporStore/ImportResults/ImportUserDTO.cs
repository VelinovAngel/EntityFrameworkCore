using System.ComponentModel.DataAnnotations;

namespace VaporStore.ImportResults
{
    public class ImportUserDTO
    {
        
        [RegularExpression(@"^[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+$")]
        public string FullName { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Range(3, 20)]
        public int Age { get; set; }

        public CardsDTO[] Cards { get; set; }
    }
}
