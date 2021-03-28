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

            var users = context.Users
                 .ToArray()
                 .Where(x => x.Cards.SelectMany(p => p.Purchases).Any(t => t.Type == purchaseType))
                 .Select(x => new ExportUserDTO
                 {
                     Username = x.Username,
                     Purchases = x.Cards.SelectMany(p => p.Purchases).Select(c => new ExportPurchaseDTO
                     {
                         Card = c.Card.Number,
                         CVC = c.Card.Cvc,
                         Date = c.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                         Game = new ExportGameDTO
                         {
                             Genre = c.Game.Genre.Name,
                             Title = c.Game.Name,
                             Price = c.Game.Price
                         }
                     })
                     .OrderBy(p => p.Date)
                     .ToArray(),
                                TotalSpent = x.Cards.SelectMany(p => p.Purchases)
                     .Sum(p => p.Game.Price)
                 })
                 .Where(u => u.Purchases.Length > 0)
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