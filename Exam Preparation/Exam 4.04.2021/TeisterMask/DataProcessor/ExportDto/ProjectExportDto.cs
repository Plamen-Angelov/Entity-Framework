using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ProjectExportDto
    {
        [XmlElement("ProjectName")]
        public string Name { get; set; }

        [XmlAttribute("TasksCount")]
        public int TaskCount { get; set; }

        [XmlElement("HasEndDate")]
        public string HasEndDate { get; set; }

        [XmlArray("Tasks")]
        public TaskDto[] Tasks { get; set; }
    }
}
