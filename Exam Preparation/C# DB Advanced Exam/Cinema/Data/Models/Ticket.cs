namespace Cinema.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;


    public class Ticket
    {
        public int Id { get; set; }
        //Id – integer, Primary Key

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        //• Price – decimal (non-negative, minimum value: 0.01) (required)

        [Required]
        public int CustomerId { get; set; }
        //• CustomerId – integer, foreign key(required)

        public Customer Customer { get; set; }
        //• Customer – the customer’s ticket

        [Required]
        public int ProjectionId { get; set; }
        //• ProjectionId – integer, foreign key(required)

        public Projection Projection { get; set; }
        //• Projection – the projection’s ticket
    }
}
