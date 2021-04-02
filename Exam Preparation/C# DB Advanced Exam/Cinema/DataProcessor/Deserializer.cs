namespace Cinema.DataProcessor
{
    using Data;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


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

            return "TODO";
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            return "TODO";
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {

            return "TODO";
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