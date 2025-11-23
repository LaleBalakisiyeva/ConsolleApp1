using ConsoleApp.Exceptions;
using ConsoleApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Services
{
    public static class EmployeeService
    {
        public static List<Employee> employees = new();
        public static string Path = "C:\\Users\\USER\\source\\tasklar\\ConsoleApp\\ConsoleApp\\Data\\employees.json";

        public static void AddEmployee(Employee employee)
        {
            if (employees.Any(e => e.ID == employee.ID))
            {
                throw new DuplicateEmployeeException("Bu ID Duplicatedir.");
            }
            else
            {
                employees.Add(employee);
                Console.WriteLine("Bu ID elave olundu");
            }
        }

       public static void RemoveEmploye(int id)
        {
            if (employees.Any(e => e.ID == id))
            {
                Employee findedEmployee = employees.Find(e => e.ID == id);
                employees.Remove(findedEmployee);
                Console.WriteLine("Bu id silindi");
            }
            else
            {
               
                Console.WriteLine("Bele bir id yoxdur.");
            }

        }

        public static Employee SearchEmployee(int id)
        {
            if (employees.Any(e => e.ID == id))
            {
                Employee findedEmployee = employees.Find(e => e.ID == id);
                return findedEmployee;
                
            }
            else
            {

                Console.WriteLine("Bele bir id yoxdur.");
            }
            return null;
        }

        public static void UpdateEmployee(int id, string newName, decimal? newSalary, string newDepartment)
        {
            Employee employeeToUpdate = employees.Find(e => e.ID == id);
            if (employeeToUpdate == null)
            {
                throw new EmployeeNotFoundException("Yenilənmək üçün ID olan işçi tapılmadı.");
            }
            
            if (newName!=null)
             {
               employeeToUpdate.Name = newName;
             }
            if (newSalary != null)
            {
                employeeToUpdate.Salary = newSalary;
            }
            if (newDepartment != null)
            {
                employeeToUpdate.Department = newDepartment;
            }

            Console.WriteLine("ID  olan işçi məlumatları yeniləndi.");

        }

        public static void ListEmployees()
        {
            foreach(var employee in employees)
            {
                employee.PrintInfo();
            }
        }

        public static List<Employee> SortEmployees(string sortBy)
        {
            List<Employee> sortedList = new List<Employee>(employees);
            switch (sortBy.ToLower())
            {
                case "id":
                    return sortedList.OrderBy(e => e.ID).ToList();
                case "name":
                    return sortedList.OrderBy(e => e.Name).ToList();
                case "hiredate":
                    return sortedList.OrderBy(e => e.HireDate).ToList();
                case "salary":
                    return sortedList.OrderBy(e => e.Salary).ToList();
                default:
                    Console.WriteLine("Yanlış sıralanib.");
                    return sortedList.OrderBy(e => e.ID).ToList();
            }
        }
        public static List<Employee> FilterEmployees(string nameFilter, decimal? minSalary, decimal? maxSalary)
        {
            List<Employee> filteredList = new List<Employee>();
            foreach (var employee in employees)
            {
                bool isMatch = true;

               if (nameFilter!=null)
                {
                  if (!employee.Name.Contains(nameFilter, StringComparison.OrdinalIgnoreCase))
                    {
                        isMatch = false;
                       
                    }
                }
                if (isMatch && minSalary.HasValue)
                {
                     if (!employee.Salary.HasValue || employee.Salary.Value < minSalary.Value)
                    {
                      
                        isMatch = false;
                    }
                }

                if (isMatch && maxSalary.HasValue)
                {
                   if (!employee.Salary.HasValue || employee.Salary.Value > maxSalary.Value)
                    {
                       
                        isMatch = false;
                    }
                }
                if (isMatch)
                {
                    filteredList.Add(employee);
                }
            }
            return filteredList;
        }

        public static void SaveToFile()
        {
            try
            {
                var json = JsonConvert.SerializeObject(employees, Newtonsoft.Json.Formatting.Indented);
                using (StreamWriter sw = new StreamWriter(Path))
                {
                    sw.WriteLine(json);
                }
            }
            catch (Exception ex)
            {
                throw new System.IO.FileLoadException($"Məlumatlar fayla yazıla bilmədi: {ex.Message}");
            }
        }

        public static void LoadFromFile ()
        {
            if (!File.Exists(Path))
            {
                employees = new List<Employee>();
                return;
            }
            try
            {
                string result;
                using (StreamReader sr = new StreamReader(Path))
                {
                    result = sr.ReadToEnd();
                }
                var loadedEmployees = JsonConvert.DeserializeObject<List<Employee>>(result);
                employees = loadedEmployees ?? new List<Employee>();

            }
            catch (Exception ex)
            {
                employees = new List<Employee>();
                throw new System.IO.FileLoadException($"Fayl oxunarkən xəta baş verdi. Məlumatlar yüklənmədi: {ex.Message}");
            }
        }


    }

 }

 

