namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;


    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {

            var project = context.Projects
                .Where(x => x.Tasks.Any())
                .ToArray()
                .Select(x => new ExportModelProject
                {
                    TasksCount = x.Tasks.Count(),
                    ProjectName = x.Name,
                    HasEndDate = x.DueDate == null ? "NO" : "YES",
                    Tasks = x.Tasks.Select(y => new TaskModel
                    {
                        Name = y.Name,
                        Label = y.LabelType.ToString()
                    })
                    .OrderBy(y => y.Name)
                    .ToArray()

                })
                .OrderByDescending(x => x.TasksCount)
                .ThenBy(x => x.ProjectName)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportModelProject[]), new XmlRootAttribute("Projects"));

            var stringWriter = new StringWriter();

            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            xmlSerializer.Serialize(stringWriter, project, ns);


            return stringWriter.ToString();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {

            var employes = context.Employees
                .ToList()
                .Where(x => x.EmployeesTasks.Any(y => y.Task.OpenDate >= date))
                .Select(e => new
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks
                    .Where(et => et.Task.OpenDate >= date)
                    .OrderByDescending(et => et.Task.DueDate)
                    .ThenBy(et => et.Task.Name)
                    .Select(st => new
                    {
                        TaskName = st.Task.Name,
                        OpenDate = st.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = st.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = st.Task.LabelType.ToString(),
                        ExecutionType = st.Task.ExecutionType.ToString()
                    })
                })
                .OrderByDescending(e => e.Tasks.Count())
                .ThenBy(e => e.Username)
                .Take(10)
                .ToList();

            var jsonConvert = JsonConvert.SerializeObject(employes, Formatting.Indented);

            return jsonConvert;
        }
    }
}