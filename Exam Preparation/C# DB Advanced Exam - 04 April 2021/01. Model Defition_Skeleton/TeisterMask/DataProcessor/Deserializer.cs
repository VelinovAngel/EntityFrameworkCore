namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using System.Linq;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var projectsXmlDes = new XmlSerializer(typeof(ImportProjectModel[]), new XmlRootAttribute("Projects"));

            var strinReader = new StringReader(xmlString);


            var projectsImported = (IEnumerable<ImportProjectModel>)projectsXmlDes.Deserialize(strinReader);

            foreach (var projectDto in projectsImported)
            {

                if (!IsValid(projectDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var opendateParsed = DateTime.TryParseExact(projectDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime);
                var duedateParsed = DateTime.TryParseExact(projectDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var duedate);

                var project = new Project
                {
                    Name = projectDto.Name,
                    OpenDate = dateTime,
                    DueDate = duedate
                };

                foreach (var taskDto in projectDto.Tasks)
                {
                    if (!IsValid(taskDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (duedateParsed && opendateParsed)
                    {
                        var openDateProject = DateTime.ParseExact(projectDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var dueDateProject = DateTime.ParseExact(projectDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        var openDateTask = DateTime.ParseExact(taskDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var dueDateTask = DateTime.ParseExact(taskDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        if (openDateTask < openDateProject || dueDateTask > dueDateProject)
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }



                    }

                    var task = new Task
                    {
                        Name = taskDto.Name,
                        OpenDate = DateTime.ParseExact(taskDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        DueDate = DateTime.ParseExact(taskDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ExecutionType = (ExecutionType)taskDto.ExecutionType,
                        LabelType = (LabelType)taskDto.LabelType
                    };
                    project.Tasks.Add(task);

                }

                context.Projects.Add(project);
                context.SaveChanges();
                sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }


            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var employeeJson = JsonConvert.DeserializeObject<IEnumerable<ImportEmployeeModel>>(jsonString);

            var employeesToList = new List<Employee>();

            foreach (var employeeDTO in employeeJson)
            {
                if (!IsValid(employeeJson))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var employee = new Employee
                {
                    Username = employeeDTO.Username,
                    Email = employeeDTO.Email,
                    Phone = employeeDTO.Phone,

                };

                foreach (var taskDTO in employeeDTO.Tasks.Distinct())
                {
                    var currTask = context.Tasks.FirstOrDefault(x => x.Id == taskDTO);

                    if (currTask == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    employee.EmployeesTasks.Add(new EmployeeTask
                    {
                        Employee = employee,
                        TaskId = taskDTO
                    });
                }
                employeesToList.Add(employee);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
            }
            context.Employees.AddRange(employeesToList);
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