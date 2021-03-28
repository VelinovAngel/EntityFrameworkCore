using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class OfficerPrisoner
    {
        public int PrisonerId { get; set; }
        //•	PrisonerId – integer, Primary Key

        [Required]
        public Prisoner Prisoner { get; set; }
        //•	Prisoner – the officer’s prisoner(required)

        public int OfficerId { get; set; }
        //•	OfficerId – integer, Primary Key

        [Required]
        public Officer Officer { get; set; }
        //•	Officer – the prisoner’s officer(required)

    }
}
