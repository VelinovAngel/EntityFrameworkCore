﻿namespace Cinema.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    using Cinema.Data.Models.Enums;

    public class Movie
    {
        public Movie()
        {
            this.Projections = new HashSet<Projection>();
        }
        public int Id { get; set; }
        // Id – integer, Primary Key

        [Required]
        [MinLength(3), MaxLength(20)]
        public string Title { get; set; }
        //• Title – text with length[3, 20] (required)

        [Required]
        public Genre Genre { get; set; }
        //• Genre – enumeration of type Genre, with possible values(Action, Drama, Comedy, Crime, Western, Romance, Documentary, Children, Animation, Musical) (required)

        [Required]
        public TimeSpan Duration { get; set; }
        //• Duration – TimeSpan (required)

        [Required]
        [Range(1, 10)]
        public double Rating { get; set; }
        //• Rating – double in the range [1,10] (required)

        [Required]
        [MinLength(3), MaxLength(20)]
        public string Director { get; set; }
        //• Director – text with length[3, 20] (required)

        public ICollection<Projection> Projections { get; set; }
        //• Projections - collection of type Projection
    }
}
