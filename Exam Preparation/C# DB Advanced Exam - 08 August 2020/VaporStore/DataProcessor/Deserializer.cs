namespace VaporStore.DataProcessor
{
    using Data;
    using System;
    using System.IO;
    using System.Text;
    using System.Linq;

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using Newtonsoft.Json;

    using VaporStore.Data.Models;
    using VaporStore.ImportResults;
    using VaporStore.Data.Models.Enums;
    using System.Xml.Serialization;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var games = JsonConvert.DeserializeObject<IEnumerable<ImportGameDTO>>(jsonString);
            var gameCollection = new List<Game>();

            //var gamesDTO = AutoMapper.Mapper.Map<Game>(games);

            foreach (var gameDTO in games)
            {
                if (!IsValid(gameDTO) || gameDTO.Tags.Count == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var game = new Game
                {
                    Name = gameDTO.Name,
                    Price = gameDTO.Price,
                    ReleaseDate = DateTime.ParseExact(gameDTO.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                };

                var developer = GetDeveloper(context, gameDTO);
                var genre = GetGenre(context, gameDTO);

                game.Developer = developer;
                game.Genre = genre;

                foreach (var tag in gameDTO.Tags)
                {
                    var tags = GetTag(context, tag);

                    game.GameTags.Add(new GameTag { Game = game, Tag = tags });
                }
                gameCollection.Add(game);
                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
            }

            context.Games.AddRange(gameCollection);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static Tag GetTag(VaporStoreDbContext context, string tag)
        {
            var tags = context.Tags.FirstOrDefault(x => x.Name == tag);
            if (tags == null)
            {
                tags = new Tag { Name = tag };

                context.Tags.Add(tags);
                context.SaveChanges();
            }
            return tags;
        }

        private static Genre GetGenre(VaporStoreDbContext context, ImportGameDTO game)
        {
            var genre = context.Genres.FirstOrDefault(x => x.Name == game.Genre);

            if (genre == null)
            {
                genre = new Genre { Name = game.Genre };
                context.Genres.Add(genre);
                context.SaveChanges();
            }
            return genre;
        }

        private static Developer GetDeveloper(VaporStoreDbContext context, ImportGameDTO game)
        {
            var developer = context.Developers.FirstOrDefault(x => x.Name == game.Developer);
            if (developer == null)
            {
                developer = new Developer { Name = game.Developer };
                context.Developers.Add(developer);
                context.SaveChanges();
            }

            return developer;
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var usersDto = JsonConvert.DeserializeObject<IEnumerable<ImportUserDTO>>(jsonString);

            List<User> users = new List<User>();

            foreach (var userDTO in usersDto)
            {

                if (!IsValid(userDTO) || !userDTO.Cards.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }


                bool isValidEnum = false;

                var currUser = new User
                {
                    Username = userDTO.Username,
                    FullName = userDTO.FullName,
                    Email = userDTO.Email,
                    Age = userDTO.Age
                };

                foreach (var card in userDTO.Cards)
                {

                    bool isParsed = Enum.TryParse<CardType>(card.Type, out CardType result);
                    if (!isParsed)
                    {
                        isValidEnum = true;
                        break;
                    }

                    if (isValidEnum)
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }


                    currUser.Cards.Add(new Card
                    {
                        Number = card.Number,
                        Cvc = card.CVC,
                        Type = Enum.Parse<CardType>(card.Type)
                    });
                }

                users.Add(currUser);
                sb.AppendLine($"Imported {userDTO.Username} with {userDTO.Cards.Count()} cards");
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            var xmlSerializer = new XmlSerializer(typeof(InputPurchaseDTO[]), new XmlRootAttribute("Purchases"));

            var streanWriter = new StringReader(xmlString);

            var purchasesDTO = (IEnumerable<InputPurchaseDTO>)xmlSerializer.Deserialize(streanWriter);

            StringBuilder sb = new StringBuilder();

            List<Purchase> purchases = new List<Purchase>();

            foreach (var purchaseDTO in purchasesDTO)
            {
                if (!IsValid(purchaseDTO))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var isValidEnum = Enum.TryParse<PurchaseType>(purchaseDTO.Type, out PurchaseType purchaseType);

                if (!isValidEnum)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var game = context.Games.FirstOrDefault(x => x.Name == purchaseDTO.Title);
                var card = context.Cards.FirstOrDefault(x => x.Number == purchaseDTO.Card);

                if (game == null || card == null)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var purchase = new Purchase
                {
                    Type = purchaseType,
                    ProductKey = purchaseDTO.ProductKey,
                    Card = card,
                    Game = game,
                    Date = DateTime.ParseExact(purchaseDTO.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
                };

                //var user = context.Users.FirstOrDefault(x => x.Cards.Contains(card));
                purchases.Add(purchase);
                sb.AppendLine($"Imported {purchase.Game.Name} for {purchase.Card.User.Username}");
            }

            context.Purchases.AddRange(purchases);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}