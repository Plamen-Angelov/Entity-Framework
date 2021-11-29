using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TeisterMask.Data.Models;

namespace TeisterMask.Dto.Import
{
    [XmlType("Project")]
    public class ProjectImportDto
    {
        [Required]
        [StringLength(Constants.Project_NAME_MAX_LENGHT, MinimumLength = 2)]
        [XmlElement("Name")]
        public string Name { get; set; }

        [Required]
        [XmlElement("OpenDate")]
        public string OpenDate { get; set; }

        [XmlElement("DueDate")]
        public string DueDate { get; set; }

        [XmlArray("Tasks")]
        public virtual List<TaskImportDto> Tasks { get; set; }
    }
}
