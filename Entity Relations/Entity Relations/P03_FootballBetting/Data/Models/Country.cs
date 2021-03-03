using System.Collections.Generic;

namespace P03_FootballBetting.Data.Models
{
    public class Country
    {
        //•	Country – CountryId, Name

        public int CountryId { get; set; }

        public string Name { get; set; }

        //collection
        public ICollection<Town> Towns { get; set; }
    }
}