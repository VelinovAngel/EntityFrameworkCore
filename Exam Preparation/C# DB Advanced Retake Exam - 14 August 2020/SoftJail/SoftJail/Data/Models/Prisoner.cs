using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Prisoner
    {
        public Prisoner()
        {
            this.Mails = new HashSet<Mail>();
            this.PrisonerOfficers = new HashSet<OfficerPrisoner>();
        }

        [Required]
        public int Id { get; set; }
        //•	Id – integer, Primary Key

        [Required]
        [MinLength(3), MaxLength(20)]
        public string FullName { get; set; }
        //•	FullName – text with min length 3 and max length 20 (required)

        [Required]
        [RegularExpression(@"^The [A-Z][a-z]+$")]
        public string Nickname { get; set; }
        //•	Nickname – text starting with "The " and a single word only of letters with an uppercase letter for beginning(example: The Prisoner) (required)

        [Required]
        [Range(18, 65)]
        public int Age { get; set; }
        //•	Age – integer in the range[18, 65] (required)

        [Required]
        public DateTime IncarcerationDate { get; set; }
        //•	IncarcerationDate ¬– Date(required)

        public DateTime? ReleaseDate { get; set; }
        //•	ReleaseDate– Date

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal? Bail { get; set; }
        //•	Bail– decimal (non-negative, minimum value: 0)

        public int? CellId { get; set; }
        //•	CellId - integer, foreign key

        public Cell Cell { get; set; }
        //•	Cell – the prisoner's cell

        public ICollection<Mail> Mails { get; set; }
        //•	Mails - collection of type Mail

        public ICollection<OfficerPrisoner> PrisonerOfficers { get; set; }
        //•	PrisonerOfficers - collection of type OfficerPrisoner
    }
}