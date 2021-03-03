﻿using System;
using System.Collections.Generic;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    public class Town
    {
        //TownId, Name, CountryId

        public int TownId { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }

        public Country Country { get; set; }

        //collection
        public ICollection<Team> Teams { get; set; }
    }
}
