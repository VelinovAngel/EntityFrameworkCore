using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ExportModelProject
    {
        [XmlAttribute("TasksCount")]
        public int TasksCount { get; set; }

        [XmlElement("ProjectName")]
        [Required]
        [MinLength(2), MaxLength(40)]
        public string ProjectName { get; set; }

        [XmlElement("HasEndDate")]
        public string HasEndDate { get; set; }


        [XmlArray("Tasks")]
        public TaskModel[] Tasks { get; set; }
    }

    [XmlType("Task")]
    public class TaskModel
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(2), MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [XmlElement("Label")]
        public string Label { get; set; }
    }
}
