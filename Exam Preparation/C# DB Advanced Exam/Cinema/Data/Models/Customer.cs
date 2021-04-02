namespace Cinema.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public class Customer
    {
        public Customer()
        {
            this.Tickets = new HashSet<Ticket>();
        }
        public int Id { get; set; }
        //• Id – integer, Primary Key

        [Required]
        [MinLength(3), MaxLength(20)]
        public string FirstName { get; set; }
        //• FirstName – text with length[3, 20] (required)

        [Required]
        [MinLength(3), MaxLength(20)]
        public string LastName { get; set; }
        //• LastName – text with length[3, 20] (required)

        [Required]
        [Range(12, 110)]
        public int Age { get; set; }
        //• Age – integer in the range[12, 110] (required)

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Balance { get; set; }
        //• Balance - decimal (non-negative, minimum value: 0.01) (required)

        public ICollection<Ticket> Tickets { get; set; }
        //• Tickets - collection of type Ticket
    }
}
