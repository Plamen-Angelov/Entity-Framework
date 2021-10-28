using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            Console.WriteLine(RemoveTown(context));
        }

        //Problem 15

        public static string RemoveTown(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Address.Town.Name == "Seattle")
                .ToArray();

            foreach (var e in employees)
            {
                e.AddressId = null;
            }

            Address[] addresses = context.Addresses
                .Where(a => a.Town.Name == "Seattle")
                .ToArray();

            context.RemoveRange(addresses);

            Town seattle = context.Towns
                .FirstOrDefault(t => t.Name == "Seattle");

            context.Remove(seattle);

            context.SaveChanges();

            return $"{addresses.Count()} addresses in Seattle were deleted";
        }

        //Problem 14

        public static string DeleteProjectById(SoftUniContext context)
        {
            var employeesProjects = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2);

            context.EmployeesProjects.RemoveRange(employeesProjects);

            Project project = context.Projects
                .Find(2);

            context.Projects.Remove(project);

            context.SaveChanges();

            var projects = context.Projects
                .Take(10)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var p in projects)
            {
                sb.AppendLine(p.Name);
            }

            return sb.ToString().Trim();
        }

        // Problem 13

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})");
            }

            return sb.ToString().Trim();
        }

        //Problem 12

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Department.Name == "Engineering"
                            || e.Department.Name == "Tool Design"
                            || e.Department.Name == "Marketing"
                            || e.Department.Name == "Information Services")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            employees.ForEach(e => e.Salary *= 1.12m);

            context.SaveChanges();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:F2})");
            }

            return sb.ToString().Trim();
        }

        //Problem 11

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(p => new { p.Name, p.Description, p.StartDate })
                .OrderBy(p => p.Name)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var p in projects)
            {
                sb.AppendLine(p.Name);
                sb.AppendLine(p.Description);
                string date = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                sb.AppendLine(date);
            }

            return sb.ToString().Trim();
        }

        //Problem 10

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new { d.Name, d.Manager, d.Employees })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.Name} - {d.Manager.FirstName}  {d.Manager.LastName}");

                var orderedEmployees = d.Employees
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToArray();

                foreach (var e in orderedEmployees)
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }

            return sb.ToString().Trim();
        }

        //Problem 9

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
                .FirstOrDefault(e => e.EmployeeId == 147);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            var projects = context.EmployeesProjects
                .Where(ep => ep.EmployeeId == 147)
                .Select(ep => ep.Project)
                .OrderBy(p => p.Name);

            foreach (var p in projects)
            {
                sb.AppendLine($"{p.Name}");
            }

            return sb.ToString().Trim();
        }

        //Problem 8
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .Select(a => new
                {
                    a.AddressText,
                    townName = a.Town.Name,
                    employeeCount = a.Employees.Count
                })
                .OrderByDescending(a => a.employeeCount)
                .ThenBy(a => a.townName)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.AddressText}, {a.townName} - {a.employeeCount} employees");
            }

            return sb.ToString().Trim();
        }

        //Problem 7
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.EmployeesProjects
                    .Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    managerFirstName = e.Manager.FirstName,
                    managerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects
                    .Select(ep => ep.Project)
                })
                .Take(10)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.managerFirstName} {e.managerLastName}");

                foreach (var p in e.Projects)
                {
                    string startDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.GetCultureInfo("en-US"));
                    string endDate = p.EndDate is null
                        ? "not finished"
                        : p.EndDate?.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.GetCultureInfo("en-US"));

                    sb.AppendLine($"--{p.Name} - {startDate} - {endDate}");
                }
            }
            return sb.ToString().Trim();
        }

        //Problem 6
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Employee employee = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            employee.Address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.SaveChanges();

            string[] addresses = context.Addresses
                .OrderByDescending(a => a.AddressId)
                .Select(a => a.AddressText)
                .Take(10)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (string a in addresses)
            {
                sb.AppendLine(a);
            }

            return sb.ToString().Trim();
        }

        //Problem 5
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:F2}");
            }

            return sb.ToString().Trim();
        }

        //Problem 4
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employeesWithSalaries = context.Employees
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .Select(e => new { e.FirstName, e.Salary })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employeesWithSalaries)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:F2}");
            }

            return sb.ToString().Trim();
        }

        //Problem 3
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employeeInfo = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new { e.FirstName, e.LastName, e.MiddleName, e.JobTitle, e.Salary })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employeeInfo)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}");
            }

            return sb.ToString().Trim();
        }
    }
}
