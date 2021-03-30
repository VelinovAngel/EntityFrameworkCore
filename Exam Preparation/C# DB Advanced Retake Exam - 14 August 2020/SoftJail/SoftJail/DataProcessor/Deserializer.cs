﻿namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
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
            var result = new StringBuilder();

            var prisonersDto = JsonConvert.DeserializeObject<IEnumerable<ImportPrisonersAndMailsModel>>(jsonString);

            foreach (var prisonerJs in prisonersDto)
            {
                if (!IsValid(prisonerJs) || !prisonerJs.Mails.All(IsValid))
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                var prisoner = new Prisoner
                {
                    FullName = prisonerJs.FullName,
                    Nickname = prisonerJs.Nickname,
                    Age = prisonerJs.Age,
                    IncarcerationDate = DateTime.ParseExact(prisonerJs.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ReleaseDate = DateTime.TryParseExact(prisonerJs.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : (DateTime?)null,
                    Bail = prisonerJs.Bail,
                    CellId = prisonerJs.CellId,
                    Mails = prisonerJs.Mails
                                        .Select(m => new Mail
                                        {
                                            Address = m.Address,
                                            Description = m.Description,
                                            Sender = m.Sender
                                        }).ToArray()
                };

                context.Prisoners.Add(prisoner);
                context.SaveChanges();
                result.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }


            return result.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var result = new StringBuilder();

            var xmlSerializer = new XmlSerializer(typeof(ImportOfficersPrisonersModel[]), new XmlRootAttribute("Officers"));

            var stringReader = new StringReader(xmlString);

            var officersXml = (IEnumerable<ImportOfficersPrisonersModel>)xmlSerializer.Deserialize(stringReader);

            foreach (var officerDto in officersXml)
            {
                bool isParsedWeapon = Enum.TryParse(typeof(Weapon), officerDto.Weapon, out var weapon);
                bool isParsedPosition = Enum.TryParse(typeof(Position), officerDto.Position, out var position);

                if (!IsValid(officerDto) || !isParsedWeapon || !isParsedPosition)
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                var officer = new Officer
                {
                    FullName = officerDto.Name,
                    Salary = officerDto.Money,
                    Position = (Position)position,
                    Weapon = (Weapon)weapon,
                    DepartmentId = officerDto.DepartmentId,
                    OfficerPrisoners = officerDto.Prisoners.Select(x => new OfficerPrisoner { PrisonerId = x.Id })
                    .ToArray()
                };

                context.Officers.Add(officer);
                context.SaveChanges();
                result.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
            }

            return result.ToString().TrimEnd();
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