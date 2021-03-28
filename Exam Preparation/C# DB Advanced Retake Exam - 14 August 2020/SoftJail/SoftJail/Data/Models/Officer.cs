using SoftJail.Data.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Officer
    {
        public Officer()
        {
            this.OfficerPrisoners = new HashSet<OfficerPrisoner>();
        }

        public int Id { get; set; }
        //•	Id – integer, Primary Key

        [Required]
        [MinLength(3), MaxLength(30)]
        public string FullName { get; set; }
        //•	FullName – text with min length 3 and max length 30 (required)

        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Salary { get; set; }
        //•	Salary – decimal (non-negative, minimum value: 0) (required)

        [Required]
        public Position Position { get; set; }
        //•	Position - Position enumeration with possible values: “Overseer, Guard, Watcher, Labour” (required)

        [Required]
        public Weapon Weapon { get; set; }
        //•	Weapon - Weapon enumeration with possible values: “Knife, FlashPulse, ChainRifle, Pistol, Sniper” (required)

        [Required]
        public int DepartmentId { get; set; }
        //•	DepartmentId - integer, foreign key(required)

        [Required]
        public Department Department { get; set; }
        //•	Department – the officer's department (required)

        public ICollection<OfficerPrisoner> OfficerPrisoners { get; set; }
        //•	OfficerPrisoners - collection of type OfficerPrisoner

    }
}
