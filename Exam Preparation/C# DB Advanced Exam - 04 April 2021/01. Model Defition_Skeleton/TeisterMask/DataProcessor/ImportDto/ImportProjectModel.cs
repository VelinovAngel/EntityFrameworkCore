using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Project")]
    public class ImportProjectModel
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(2), MaxLength(40)]
        public string Name { get; set; }

        [XmlElement("OpenDate")]
        [Required]
        public string OpenDate { get; set; }

        [XmlElement("DueDate")]
        public string DueDate { get; set; }

        [XmlArray("Tasks")]
        public ImportTaskModel[] Tasks { get; set; }
    }

    [XmlType("Task")]
    public class ImportTaskModel
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(2), MaxLength(40)]
        public string Name { get; set; }

        [XmlElement("OpenDate")]
        [Required]
        public string OpenDate { get; set; }

        [XmlElement("DueDate")]
        [Required]
        public string DueDate { get; set; }

        [XmlElement("ExecutionType")]
        [Required]
        public int ExecutionType { get; set; }

        [XmlElement("LabelType")]
        [Required]
        public int LabelType { get; set; }
    }
}
