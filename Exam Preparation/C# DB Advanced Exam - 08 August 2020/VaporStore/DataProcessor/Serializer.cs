namespace VaporStore.DataProcessor
{
    using System;
    using Data;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Xml.Serialization;

    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {

            var genres = context.Genres
                .ToArray()
                .Where(g => genreNames.Contains(g.Name))
                .Select(x => new
                {
                    Id = x.Id,
                    Genre = x.Name,
                    Games = x.Games
                    .Where(ga => ga.Purchases.Any())
                    .Select(g => new
                    {
                        Id = g.Id,
                        Title = g.Name,
                        Developer = g.Developer.Name,
                        Tags = string.Join(", ", g.GameTags
                        .Select(t => t.Tag.Name)
                        .ToArray()),
                        Players = g.Purchases.Count
                    })
                        .OrderByDescending(p => p.Players)
                        .ThenBy(i => i.Id)
                        .ToArray(),
                    TotalPlayers = x.Games.Sum(p => p.Purchases.Count)
                })
                .OrderByDescending(x => x.TotalPlayers)
                .ThenBy(g => g.Id)
                .ToArray();

            var genresToJson = JsonConvert.SerializeObject(genres, Formatting.Indented);


            return genresToJson;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            var purchaseType = Enum.Parse<PurchaseType>(storeType);

            var users = context
                .Users
                .ToArray()
                .Where(u => u.Cards.Any(c => c.Purchases.Any()))
                .Select(u => new ExportUserDTO()
                {
                    Username = u.Username,
                    Purchases = context
                        .Purchases
                        .ToArray()
                        .Where(p => p.Card.User.Username == u.Username && p.Type == purchaseType)
                        .OrderBy(p => p.Date)
                        .Select(p => new ExportPurchaseDTO()
                        {
                            Card = p.Card.Number,
                            CVC = p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Game = new ExportGameDTO()
                            { 
                                Title = p.Game.Name,
                                Genre = p.Game.Genre.Name,
                                Price = p.Game.Price
                            }
                        })
                        .ToArray(),
                    TotalSpent = context
                        .Purchases
                        .ToArray()
                        .Where(p => p.Card.User.Username == u.Username && p.Type == purchaseType)
                        .Sum(p => p.Game.Price)
                })
                .Where(u => u.Purchases.Any())
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportUserDTO[]),new XmlRootAttribute("Users"));

            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            var stringStream = new StringWriter();

            xmlSerializer.Serialize(stringStream, users, ns);

            return stringStream.ToString();
        }
    }
}