namespace Cinema.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Seat
    {
        public int Id { get; set; }
        //• Id – integer, Primary Key

        [Required]
        public int HallId { get; set; }
        //• HallId – integer, foreign key(required)

        public Hall Hall { get; set; }
        //• Hall – the seat’s hall
    }
}
