using System.Collections.Generic;

namespace VaporStore.ImportResults
{
    public class ImprtGameDTO
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ReleaseData { get; set; }

        public string Developer { get; set; }

        public string Genre { get; set; }

        public List<string> Tags { get; set; }
        //"Name": "Dota 2",
        //"Price": 0,
        //"ReleaseDate": "2013-07-09",
        //"Developer": "Valve",
        //"Genre": "Action",
        //"Tags": [
        //	"Multi-player",
        //	"Co-op",
        //	"Steam Trading Cards",
        //	"Steam Workshop",
        //	"SteamVR Collectibles",
        //	"In-App Purchases",
        //	"Valve Anti-Cheat enabled"
        //]
    }
}
