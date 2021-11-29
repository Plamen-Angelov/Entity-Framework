namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            ProjectExportDto[] projects = context
                .Projects
                .Where(p => p.Tasks.Any())
                .ToArray()
                .Select(p => new ProjectExportDto()
                {
                    Name = p.Name,
                    TaskCount = p.Tasks.Count(),
                    HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
                    Tasks = p.Tasks
                    .ToArray()
                    .Select(t => new TaskDto()
                    {
                        Name = t.Name,
                        LabelType = t.LabelType.ToString()
                    })
                    .OrderBy(t => t.Name)
                    .ToArray()
                })
                .OrderByDescending(p => p.TaskCount)
                .ThenBy(p => p.Name)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ProjectExportDto[]), new XmlRootAttribute("Projects"));
            StringBuilder sb = new StringBuilder();

            using StringWriter writer = new StringWriter(sb);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            serializer.Serialize(writer, projects, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            EmployeeExportDto[] employees = context
                .Employees
                .Where(e => e.EmployeesTasks.Any(et => et.Task.OpenDate >= date))
                .ToArray()
                .Select(e => new EmployeeExportDto
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks
                    .Where(et => et.Task.OpenDate >= date)
                    .ToArray()
                    .OrderByDescending(et => et.Task.DueDate)
                    .OrderBy(et => et.Task.Name)
                    .Select(et => new TaskExportDto
                    {
                        Name = et.Task.Name,
                        OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = et.Task.LabelType.ToString(),
                        ExecutionType = et.Task.ExecutionType.ToString()
                    })
                    .ToList()
                })
                .OrderByDescending(e => e.Tasks.Count())
                .OrderBy(e => e.Username)
                .Take(10)
                .ToArray();


            string json = JsonConvert.SerializeObject(employees, Formatting.Indented);

            return json;
        }
    }
}