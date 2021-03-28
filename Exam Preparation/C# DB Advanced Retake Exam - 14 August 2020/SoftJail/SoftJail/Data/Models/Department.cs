using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Department
    {
        public Department()
        {
            this.Cells = new HashSet<Cell>();
        }

        public int Id { get; set; }
        //•	Id – integer, Primary Key

        [Required]
        [MinLength(3), MaxLength(25)]
        public string Name { get; set; }
        //•	Name – text with min length 3 and max length 25 (required)

        public ICollection<Cell> Cells { get; set; }
        //•	Cells - collection of type Cell

    }
}
