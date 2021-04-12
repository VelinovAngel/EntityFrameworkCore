using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeisterMask.Data.Models
{
    public class EmployeeTask
    {
        [Required]
        public int EmployeeId { get; set; }
        //•	EmployeeId - integer, Primary Key, foreign key(required)

        public Employee Employee { get; set; }
        //•	Employee -  Employee

        [Required]
        public int TaskId { get; set; }
        //•	TaskId - integer, Primary Key, foreign key(required)

        public Task Task { get; set; }
        //•	Task - Task

    }
}
