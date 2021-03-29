namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departments = JsonConvert.DeserializeObject<IEnumerable<ImportDepartmentCellsModel>>(jsonString);

            var result = new StringBuilder();


            foreach (var departmentJson in departments)
            {
                if (!IsValid(departmentJson) || !departmentJson.Cells.All(IsValid))
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                var department = new Department
                {
                    Name = departmentJson.Name,
                };

                foreach (var cellJson in departmentJson.Cells)
                {
                    var cell = context.Cells.FirstOrDefault(x => x.CellNumber == cellJson.CellNumber)
                        ?? new Cell { CellNumber = cellJson.CellNumber, HasWindow = cellJson.HasWindow };

                    department.Cells.Add(cell);
                }
                context.Departments.Add(department);
                context.SaveChanges();
                result.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }
            
            return result.ToString().TrimEnd();
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