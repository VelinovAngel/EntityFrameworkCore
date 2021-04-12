using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeisterMask.Data.Models
{
    public class Employee
    {
        public Employee()
        {
            this.EmployeesTasks = new HashSet<EmployeeTask>();
        }

        public int Id { get; set; }
        //        •	Id - integer, Primary Key

        [Required]
        [RegularExpression(@"^[A-Za-z0-9]$")]
        [MinLength(3), MaxLength(40)]
        public string Username { get; set; }
        //•	Username - text with length[3, 40]. Should contain only lower or upper case letters and/or digits. (required)

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        //•	Email – text(required). Validate it! There is attribute for this job.

        [Required]
        [RegularExpression(@"^[0-9]{3}-[0-9]{3}-[0-9]{4}$")]
        public string Phone { get; set; }
        //•	Phone - text.Consists only of three groups(separated by '-'), the first two consist of three digits and the last one - of 4 digits. (required)

        public ICollection<EmployeeTask> EmployeesTasks { get; set; }
        //•	EmployeesTasks - collection of type EmployeeTask


    }
}
