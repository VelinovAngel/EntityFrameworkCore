﻿using System;
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
            //Console.WriteLine(GetEmployeesInPeriod(context));
            //Console.WriteLine(GetAddressesByTown(context));
            //Console.WriteLine(GetEmployee147(context));
            //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));
            //Console.WriteLine(GetLatestProjects(context));
            //Console.WriteLine(IncreaseSalaries(context));
            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));
            //Console.WriteLine(DeleteProjectById(context));
            //Console.WriteLine(RemoveTown(context));

        }

        public static string RemoveTown(SoftUniContext context)
        {
            var town = context.Towns
                .Include(x=>x.Addresses)
                .FirstOrDefault(x => x.Name == "Seattle");

            var allAddresses = town.Addresses.Select(x => x.AddressId).ToList();

            var employeesId = context.Employees
                .Where(x =>x.AddressId.HasValue && allAddresses.Contains(x.AddressId.Value));

            foreach (var employee in employeesId)
            {
                employee.AddressId = null;
            }

            foreach (var address in allAddresses)
            {
                var currAddress = context.Addresses
                    .FirstOrDefault(x => x.AddressId == address);

                context.Addresses.Remove(currAddress);
            }

            context.Towns.Remove(town);

            context.SaveChanges();

            string TownsDeleted = $"{allAddresses.Count} addresses in {town.Name} were deleted";

            return TownsDeleted;
        }
        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesProjectsToDelete = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2);

            var project = context.Projects
                .Where(p => p.ProjectId == 2)
                .Single();

            foreach (var ep in employeesProjectsToDelete)
            {
                context.EmployeesProjects.Remove(ep);
            }

            context.Projects.Remove(project);

            context.SaveChanges();

            var projects = context.Projects
                .Take(10)
                .Select(p => p.Name)
                .ToList();

            foreach (var proj in projects)
            {
                sb.AppendLine(proj);
            }
                

            return sb.ToString().TrimEnd();
        }
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(x => x.FirstName.ToLower().StartsWith("sa"))
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            context.Employees
                .Where(x => new[]
                {
                    "Engineering",
                    "Tool Design",
                    "Marketing",
                    "Information Services",
                }.Contains(x.Department.Name))
                .ToList()
                .ForEach(x => x.Salary *= 1.12M);

            context.SaveChanges();

            var employees = context.Employees
                  .Where(x => new[]
                {
                    "Engineering",
                    "Tool Design",
                    "Marketing",
                    "Information Services",
                }.Contains(x.Department.Name))
                  .Select(x => new
                  {
                      x.FirstName,
                      x.LastName,
                      x.Salary
                  })
                  .OrderBy(x => x.FirstName)
                  .ThenBy(x => x.LastName)
                  .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:f2})");
            }

            return sb.ToString().TrimEnd();

        }
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projects = context.Projects
                .OrderByDescending(x => x.StartDate)
                .Take(10)
                .Select(x => new
                {
                    Name = x.Name,
                    Description = x.Description,
                    StartData = x.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                })
                .OrderBy(x => x.Name)
                .ToList();

            foreach (var project in projects)
            {
                sb.AppendLine(project.Name)
                .AppendLine(project.Description)
                .AppendLine(project.StartData);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments
                .Where(x => x.Employees.Count > 5)
                .OrderBy(x => x.Employees.Count)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    DepartmentName = x.Name,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Employees = x.Employees.Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle
                    })
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName)
                    .ToList()
                })
                .ToList();

            foreach (var department in departments)
            {
                sb.AppendLine($"{department.DepartmentName} - {department.ManagerFirstName} {department.ManagerLastName}");

                foreach (var employes in department.Employees)
                {
                    sb.AppendLine($"{employes.FirstName} {employes.LastName} - {employes.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee147 = context.Employees
                .Select(x => new Employee
                {
                    EmployeeId = x.EmployeeId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    JobTitle = x.JobTitle,
                    EmployeesProjects = x.EmployeesProjects.Select(p => new EmployeeProject
                    {
                        Project = p.Project
                    })
                    .OrderBy(x => x.Project.Name).
                    ToList()

                })
                .FirstOrDefault(x => x.EmployeeId == 147);


            sb.AppendLine($"{employee147.FirstName} {employee147.LastName} - {employee147.JobTitle}");

            foreach (var project in employee147.EmployeesProjects)
            {
                sb.AppendLine($"{project.Project.Name}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesByTown = context.Addresses
                .OrderByDescending(x => x.Employees.Count)
                .ThenBy(x => x.Town.Name)
                .ThenBy(x => x.AddressText)
                .Take(10)
                .Select(x => new
                {
                    AddressText = x.AddressText,
                    TownName = x.Town.Name,
                    EmployeesCount = x.Employees.Count,
                })
                .ToList();

            foreach (var employee in employeesByTown)
            {
                sb.AppendLine($"{employee.AddressText}, {employee.TownName} - {employee.EmployeesCount} employees");
            }

            return sb.ToString().TrimEnd();
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
                sb.AppendLine($"{emp.employessFirstName} {emp.employessLastName} - Manager: {emp.managerFirstName} {emp.managerLastName}");

                foreach (var empPro in emp.Projects)
                {
                    string endDate = empPro.EndDate.HasValue ? empPro.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished";
                    string startDate = empPro.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                    sb.AppendLine($"--{empPro.ProjectName} - {startDate} - {endDate}");
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
