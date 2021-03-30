namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

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

            var prisonsToJson = JsonConvert.SerializeObject(prisons, Formatting.Indented);

            return prisonsToJson;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            string[] names = prisonersNames
                .Split(",")
                .ToArray();

            var prisoners = context.Prisoners
                .ToList()
                .Where(x => names.Contains(x.FullName))
                .Select(p => new ExportPrisonersDTO
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd"),
                    EncryptedMessages = p.Mails.Select(m => new MessagesExportDTO
                    {
                        Description = String.Join("", m.Description.Reverse())
                    }).ToArray()

                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportPrisonersDTO[]), new XmlRootAttribute("Prisoners"));

            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            var strimWriter = new StringWriter();

            xmlSerializer.Serialize(strimWriter, prisoners, ns);

            return strimWriter.ToString();
        }
    }
}