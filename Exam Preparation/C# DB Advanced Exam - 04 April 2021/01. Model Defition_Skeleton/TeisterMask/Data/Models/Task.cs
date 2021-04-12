using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.Data.Models
{
    public class Task
    {
        public Task()
        {
            this.EmployeesTasks = new HashSet<EmployeeTask>();
        }

        public int Id { get; set; }
        //•	Id - integer, Primary Key

        [Required]
        [MinLength(2), MaxLength(40)]
        public string Name { get; set; }
        //•	Name - text with length[2, 40] (required)

        [Required]
        public DateTime OpenDate { get; set; }
        //•	OpenDate - date and time(required)

        [Required]
        public DateTime DueDate { get; set; }
        //•	DueDate - date and time(required)

        [Required]
        public ExecutionType ExecutionType { get; set; }
        //•	ExecutionType - enumeration of type ExecutionType, with possible values(ProductBacklog, SprintBacklog, InProgress, Finished) (required)

        [Required]
        public LabelType LabelType { get; set; }
        //•	LabelType - enumeration of type LabelType, with possible values(Priority, CSharpAdvanced, JavaAdvanced, EntityFramework, Hibernate) (required)

        [Required]
        public int ProjectId { get; set; }
        //•	ProjectId - integer, foreign key(required)

        public Project Project { get; set; }
        //•	Project - Project 

        public ICollection<EmployeeTask> EmployeesTasks { get; set; }
        //•	EmployeesTasks - collection of type EmployeeTask

    }
}
