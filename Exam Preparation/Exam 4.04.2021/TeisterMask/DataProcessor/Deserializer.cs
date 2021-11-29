namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using TeisterMask.Dto.Import;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProjectImportDto[]), new XmlRootAttribute("Projects"));
            using StringReader reader = new StringReader(xmlString);

            ProjectImportDto[] dtos = (ProjectImportDto[])serializer.Deserialize(reader);
            List<Project> projects = new List<Project>();
            StringBuilder sb = new StringBuilder();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime openDate;
                bool isOpenDateValid = DateTime.TryParseExact(dto.OpenDate, "dd/MM/yyyy",
                     CultureInfo.InvariantCulture, DateTimeStyles.None, out openDate);

                if (!isOpenDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime? dueDate = null;

                if (!string.IsNullOrWhiteSpace(dto.DueDate))
                {
                    DateTime dueDateD;
                    bool isDueDateValid = DateTime.TryParseExact(dto.DueDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out dueDateD);

                    if (!isDueDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    dueDate = dueDateD;
                }

                Project project = new Project()
                {
                    Name = dto.Name,
                    OpenDate = openDate,
                    DueDate = dueDate
                };

                foreach (var task in dto.Tasks)
                {
                    if (!IsValid(task))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime taskOpenDate;

                    bool isTaskOpenDateValid = DateTime.TryParseExact(task.OpenDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out taskOpenDate);

                    if (!isTaskOpenDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime taskDueDate = default;

                    bool isTaskDueDateValid = DateTime.TryParseExact(task.DueDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out taskDueDate);

                    if (!isTaskDueDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (taskOpenDate < openDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (dueDate.HasValue && taskDueDate > dueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Task current = new Task()
                    {
                        Name = task.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDueDate,
                        ExecutionType = (ExecutionType)task.ExecutionType,
                        LabelType = (LabelType)task.LabelType
                    };

                    project.Tasks.Add(current);
                }
                projects.Add(project);
                sb.AppendLine(String.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }

            context.Projects.AddRange(projects);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            EmployeeImportDto[] employeeDtos = JsonConvert.DeserializeObject<EmployeeImportDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Employee> employees = new List<Employee>();

            foreach (var dto in employeeDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Employee employee = new Employee()
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    Phone = dto.Phone
                };

                List<EmployeeTask> employeeTasks = new List<EmployeeTask>();

                foreach (var dtoTask in dto.Tasks.Distinct())
                {
                    if (!IsValid(dtoTask))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!context.Tasks.Select(t => t.Id).Contains(dtoTask))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    EmployeeTask employeeTask = new EmployeeTask()
                    {
                        Employee = employee,
                        TaskId = dtoTask
                    };

                    employeeTasks.Add(employeeTask);
                }
                employee.EmployeesTasks = employeeTasks;
                employees.Add(employee);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count()));
            }
            context.Employees.AddRange(employees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}