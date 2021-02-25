using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var context = new SoftUniContext();

            //Console.WriteLine(GetEmployeesFullInformation(context));
            //Console.WriteLine(GetEmployeesWithSalaryOver50000(context));
            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
            //Console.WriteLine(AddNewAddressToEmployee(context));
            Console.WriteLine(GetEmployeesInPeriod(context));

        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
               .Include(x => x.EmployeesProjects)
               .ThenInclude(x => x.Project)
               .Where(x => x.EmployeesProjects.Any(x => x.Project.StartDate.Year >= 2001 && x.Project.StartDate.Year <= 2003))
               .Select(x => new
               {
                   employessFirstName = x.FirstName,
                   employessLastName = x.LastName,
                   managerFirstName = x.Manager.FirstName,
                   managerLastName = x.Manager.LastName,
                   Projects = x.EmployeesProjects.Select(x => new
                   {
                       ProjectName = x.Project.Name,
                       StartDate = x.Project.StartDate,
                       EndDate = x.Project.EndDate
                   })

               })
               .Take(10)
               .ToList();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.employessFirstName} {emp.employessLastName} – Manager: {emp.managerFirstName} {emp.managerLastName}");

                foreach (var empPro in emp.Projects)
                {
                    string endDate = empPro.EndDate.HasValue ? empPro.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished";

                    sb.AppendLine($"--{empPro.ProjectName} -{empPro.StartDate} - {endDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Address newAddress = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4,
            };

            context.Addresses.Add(newAddress);
            context.SaveChanges();

            Employee nakov = context.Employees
                .FirstOrDefault(x => x.LastName == "Nakov");

            nakov.AddressId = newAddress.AddressId;
            context.SaveChanges();

            var employees = context.Employees
                .Select(x => new
                {
                    x.Address.AddressText,
                    x.Address.AddressId,
                })
                .OrderByDescending(x => x.AddressId)
                .Take(10)
                .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.AddressText}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(x => x.Department.Name == "Research and Development")
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.Salary,
                    x.Department.Name
                })
                .OrderBy(x => x.Salary)
                .ThenByDescending(x => x.FirstName)
                .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.Name} - ${employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Select(x => new
                {
                    x.FirstName,
                    x.Salary
                })
                .Where(x => x.Salary > 50000)
                .OrderBy(x => x.FirstName)
                .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(x => new
                {
                    x.EmployeeId,
                    x.FirstName,
                    x.LastName,
                    x.MiddleName,
                    x.JobTitle,
                    x.Salary
                })
                .OrderBy(x => x.EmployeeId)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} {emp.MiddleName} {emp.JobTitle} {emp.Salary:f2}");

            }

            return sb.ToString().TrimEnd();
        }
    }
}
