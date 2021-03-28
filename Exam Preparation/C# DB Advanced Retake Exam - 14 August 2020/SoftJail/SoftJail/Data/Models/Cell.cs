using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Cell
    {
        public Cell()
        {
            this.Prisoners = new HashSet<Prisoner>();
        }

        public int Id { get; set; }
        //•	Id – integer, Primary Key

        [Required]
        [Range(0,1000)]
        public int CellNumber { get; set; }
        //•	CellNumber – integer in the range[1, 1000] (required)

        [Required]
        public bool HasWindow { get; set; }
        //•	HasWindow – bool (required)

        [Required]
        public int DepartmentId { get; set; }
        //•	DepartmentId - integer, foreign key(required)

        [Required]
        public Department Department { get; set; }
        //•	Department – the cell's department (required)

        public ICollection<Prisoner> Prisoners { get; set; }
        //•	Prisoners - collection of type Prisoner

    }
}
