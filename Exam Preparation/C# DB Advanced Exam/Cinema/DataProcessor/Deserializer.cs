namespace Cinema.DataProcessor
{
    using Cinema.Data.Models;
    using Cinema.Data.Models.Enums;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie
            = "Successfully imported {0} with genre {1} and rating {2:F2}!";
        private const string SuccessfulImportHallSeat
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var result = new StringBuilder();

            var moviesJson = JsonConvert.DeserializeObject<IEnumerable<ImportMoviesModels>>(jsonString);

            foreach (var movie in moviesJson)
            {
                if (!IsValid(movie))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var existTitile = context.Movies.FirstOrDefault(x => x.Title == movie.Title);

                if (existTitile != null)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var currGenre = Enum.Parse<Genre>(movie.Genre);

                var currMovie = new Movie
                {
                    Title = movie.Title,
                    Genre = currGenre,
                    Duration = TimeSpan.Parse(movie.Duration),
                    Rating = movie.Rating,
                    Director = movie.Director
                };

                context.Movies.Add(currMovie);
                context.SaveChanges();
                result.AppendLine(string.Format(SuccessfulImportMovie, currMovie.Title, currMovie.Genre, currMovie.Rating));
            }

            return result.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var result = new StringBuilder();

            var hallseatsJson = JsonConvert.DeserializeObject<IEnumerable<ImportHallSeatsModel>>(jsonString);

            foreach (var currHall in hallseatsJson)
            {
                if (!IsValid(currHall))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var newHall = new Hall
                {
                    Name = currHall.Name,
                    Is4Dx = currHall.Is4Dx,
                    Is3D = currHall.Is3D,
                };

                for (int i = 0; i < currHall.Seats; i++)
                {
                    var seat = new Seat { Hall = newHall };
                    newHall.Seats.Add(seat);
                }

                var type = string.Empty;
                if (newHall.Is3D)
                {
                    type = "3D";
                }
                else if (newHall.Is4Dx)
                {
                    type = "4Dx";
                }
                else if (newHall.Is3D && newHall.Is4Dx)
                {
                    type = "4Dx/3D";
                }
                else
                {
                    type = "Normal";
                }

                context.Halls.Add(newHall);
                context.SaveChanges();
                result.AppendLine(string.Format(SuccessfulImportHallSeat, newHall.Name, type, newHall.Seats.Count));

            }

            return result.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var result = new StringBuilder();

            var xmlSerailizer = new XmlSerializer(typeof(ImportProjectionModel[]), new XmlRootAttribute("Projections"));

            var strinReader = new StringReader(xmlString);

            var projectionXml = (IEnumerable<ImportProjectionModel>)xmlSerailizer.Deserialize(strinReader);

            foreach (var currProjection in projectionXml)
            {
                if (!IsValid(currProjection))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var movie = context.Movies.FirstOrDefault(x => x.Id == currProjection.MovieId);
                var hall = context.Halls.FirstOrDefault(x => x.Id == currProjection.HallId);

                if (movie == null)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                if (hall == null)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var projection = new Projection
                {
                    Hall = hall,
                    Movie = movie,
                    DateTime = DateTime.ParseExact(currProjection.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                };

                context.Projections.Add(projection);
                context.SaveChanges();
                result.AppendLine(string.Format(SuccessfulImportProjection, projection.Movie.Title, projection.DateTime.ToString("MM/dd/yyyy")));
            }

            return result.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {

            return "TODO";
        }


        private static bool IsValid(object entity)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return isValid;
        }


    }
}