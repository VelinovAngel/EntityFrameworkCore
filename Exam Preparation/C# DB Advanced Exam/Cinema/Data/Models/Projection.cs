namespace Cinema.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public class Projection
    {
        public Projection()
        {
            this.Tickets = new HashSet<Ticket>();
        }
        public int Id { get; set; }
        // Id – integer, Primary Key

        [Required]
        public int MovieId { get; set; }
        //• MovieId – integer, foreign key(required)

        public Movie Movie { get; set; }
        //• Movie – the projection’s movie

        [Required]
        public int HallId { get; set; }
        //• HallId – integer, foreign key(required)

        public Hall Hall { get; set; }
        //• Hall – the projection’s hall

        [Required]
        public DateTime DateTime { get; set; }
        //• DateTime - DateTime(required)

        public ICollection<Ticket> Tickets { get; set; }
        //• Tickets - collection of type Ticket
    }
}
