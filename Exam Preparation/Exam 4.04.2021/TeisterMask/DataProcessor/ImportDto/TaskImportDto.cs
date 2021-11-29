using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TeisterMask.Data.Models;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.Dto.Import
{
    [XmlType("Task")]
    public class TaskImportDto
    {
        [Required]
        [StringLength(Constants.Task_NAME_MAX_LENGHT, MinimumLength = 2)]
        [XmlElement("Name")]
        public string Name { get; set; }

        [Required]
        [XmlElement("OpenDate")]
        public string OpenDate { get; set; }

        [Required]
        [XmlElement("DueDate")]
        public string DueDate { get; set; }

        [XmlElement("ExecutionType")]
        [Range(0,3)]
        public int ExecutionType { get; set; }

        [XmlElement("LabelType")]
        [Range(0,4)]
        public int LabelType { get; set; }
    }
}
