using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentCellsModel
    {
        [Required]
        [MinLength(3), MaxLength(25)]
        public string Name { get; set; }

        public ImportCellNumber[] Cells { get; set; }
    }
}
