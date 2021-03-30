namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisons = context.Prisoners
                .ToList()
                .Where(x => ids.Contains(x.Id))
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.FullName,
                    CellNumber = x.Cell.CellNumber,
                    Officers = x.PrisonerOfficers.Select(o => new
                    {
                        OfficerName = o.Officer.FullName,
                        Department = o.Officer.Department.Name
                    })
                    .OrderBy(n => n.OfficerName)
                    .ToList(),
                    TotalOfficerSalary = decimal.Parse(x.PrisonerOfficers.Sum(s => s.Officer.Salary).ToString("f2"))
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToList();

           var prisonsToJson =  JsonConvert.SerializeObject(prisons, Formatting.Indented);

            return prisonsToJson;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            return "TODO";
        }
    }
}