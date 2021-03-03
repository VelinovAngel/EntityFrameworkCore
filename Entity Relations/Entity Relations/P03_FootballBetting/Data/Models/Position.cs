using System.Collections.Generic;

namespace P03_FootballBetting.Data.Models
{
    public class Position
    {
        //PositionId, Name

        public int PositionId { get; set; }

        public string Name { get; set; }

        //collection of players

        public ICollection<Player> Players { get; set; }
    }
}