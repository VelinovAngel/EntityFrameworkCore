namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.ImportResults;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var games = JsonConvert.DeserializeObject<ImportGameDTO[]>(jsonString);
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
                    
                    game.GameTags.Add(new GameTag { Game = game , Tag = tags });
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
            throw new NotImplementedException();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}