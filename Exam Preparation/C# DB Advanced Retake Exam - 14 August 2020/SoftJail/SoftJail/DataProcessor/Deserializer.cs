namespace SoftJail.DataProcessor
{

    using Data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            //TODO: NB
            return "TODO";
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            return "TODO";
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            return "TODO";
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}