using SoftUni.Data;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
        }

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
