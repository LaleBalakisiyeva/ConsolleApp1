using ConsoleApp.Exceptions;
using ConsoleApp.Models;
using ConsoleApp.Services;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            try
            {
                EmployeeService.LoadFromFile();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Məlumatlar fayldan uğurla yükləndi.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"FAYL YÜKLƏNMƏ XƏTASI: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine("Proqram boş siyahı ilə davam edir.");
            }

            bool exit = false;

            while (!exit)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("-----Employee Managment System-----");
                Console.ResetColor();
                Console.WriteLine("1- Add Employee");
                Console.WriteLine("2-Remove Employee");
                Console.WriteLine("3-Search Employee");
                Console.WriteLine("4-Update Employee");
                Console.WriteLine("5-List Employees");
                Console.WriteLine("6-Sort Employees");
                Console.WriteLine("7-Filter Employees");
                exit = true;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Secmek istediyiniz emeliyyati daxil edin:");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddEmployee();
                        //Console.WriteLine("Add metodu ise dusdu");
                        break;
                    case 2:
                        RemoveEmployee();
                        //Console.WriteLine("Remove metodu ise dusdu");
                        break;
                    case 3:
                        SearchEmployee();
                        //Console.WriteLine("Search metodu ise dusdu");
                        break;
                    case 4:
                        UpdateEmployee();
                        //Console.WriteLine("Update metodu ise dusdu");
                        break;
                    case 5:
                        ListEmployees();
                        //Console.WriteLine("List metodu ise dusdu");
                        break;
                    case 6:
                        SortEmployee();
                        //Console.WriteLine("Sort metodu ise dusdu");
                        break;
                    case 7:
                        FilterEmployees();
                        //Console.WriteLine("Filter metodu ise dusdu");
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice");
                        break;

                        static void AddEmployee()
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("--- Yeni İşçi Əlavə etmə ---");

                            int newId = EmployeeService.employees.Any() ? EmployeeService.employees.Max(e => e.ID) + 1 : 1;
                            Console.WriteLine($"Avtomatik təyin olunmuş ID: {newId}");

                            Console.Write("Ad (3-25 simvol): ");
                            string name = Console.ReadLine();

                            Console.Write("Department (5-6 simvol): ");
                            string department = Console.ReadLine();

                            DateTime hireDate = DateTime.Now;

                            Console.Write("Salary (Boş buraxmaq üçün Enter): ");
                            decimal? salary = null;
                            if (decimal.TryParse(Console.ReadLine(), out decimal s) && s > 0)
                            {
                                salary = s;
                            }


                            Contact? workInfo = null;
                            Console.Write("Ofis nömrəsi (Boş buraxmaq üçün Enter): ");
                            string officeNumberInput = Console.ReadLine();

                            if (!string.IsNullOrWhiteSpace(officeNumberInput))
                            {
                                Console.Write("Mərtəbə : ");
                                if (int.TryParse(Console.ReadLine(), out int floor) && floor > 0)
                                {
                                    workInfo = new Contact(officeNumberInput, floor);
                                }
                                else
                                {

                                    Console.WriteLine(" Mərtəbə düzgün daxil edilmədi.");
                                }
                            }


                            Employee newEmployee = new Employee(newId, name, department, hireDate, salary, workInfo: workInfo);
                            EmployeeService.AddEmployee(newEmployee);

                            EmployeeService.SaveToFile();

                        }

                        static void RemoveEmployee()
                        {
                            Console.WriteLine("--- İşçi Silmə ---");
                            Console.Write("Silinəcək işçinin ID-sini daxil edin: ");

                            if (int.TryParse(Console.ReadLine(), out int id))
                            {

                                EmployeeService.RemoveEmploye(id);
                            }
                            else
                            {
                                throw new EmployeeNotFoundException("Bele bir ID yoxdur");
                            }

                            EmployeeService.SaveToFile();
                        }

                        static void SearchEmployee()
                        {
                            Console.WriteLine("--- İşçi Axtarışı ---");
                            Console.Write("Axtarış üçün ID daxil edin: ");

                            if (int.TryParse(Console.ReadLine(), out int id))
                            {
                                Employee result = EmployeeService.SearchEmployee(id);

                                Console.WriteLine("--- Axtarış Nəticəsi ---");
                                result.PrintInfo();
                            }
                            else
                            {
                                throw new EmployeeNotFoundException("Bele bir ID yoxdur");

                            }
                        }

                        static void UpdateEmployee()
                        {
                            Console.WriteLine("--- İşçi Məlumatlarının Yenilənməsi ---");
                            Console.Write("Yenilənəcək işçinin ID-sini daxil edin: ");

                            if (!int.TryParse(Console.ReadLine(), out int id))
                            {
                                throw new EmployeeNotFoundException("Bele bir ID yoxdur");
                            }
                            Console.WriteLine("Yeni dəyərləri daxil edin :");

                            Console.Write("Yeni Ad: ");
                            string newName = Console.ReadLine();

                            Console.Write("Yeni Department: ");
                            string newDepartment = Console.ReadLine();

                            Console.Write("Yeni Salary: ");
                            decimal? newSalary = null;
                            string salaryInput = Console.ReadLine();

                            if (!string.IsNullOrWhiteSpace(salaryInput))
                            {
                                if (decimal.TryParse(salaryInput, out decimal s))
                                {
                                    newSalary = s;
                                }
                                else
                                {
                                    throw new InvalidSalaryException("sifirdan kicik ola bilmez");
                                }
                            }
                            EmployeeService.UpdateEmployee(
                             id,
                             string.IsNullOrWhiteSpace(newName) ? null : newName,
                             newSalary,
                             string.IsNullOrWhiteSpace(newDepartment) ? null : newDepartment
                             );
                            EmployeeService.SaveToFile();
                        }

                        static void SortEmployee()
                        {
                            Console.WriteLine("--- İşçilərin Sıralanması ---");
                            Console.Write("Hansı sahə üzrə sıralansın? (ID, Name, HireDate, Salary): ");
                            string sortBy = Console.ReadLine();

                            List<Employee> sortedList = EmployeeService.SortEmployees(sortBy);
                            if (sortedList.Count > 0)
                            {
                                foreach (var emp in sortedList)
                                {
                                    emp.PrintInfo();
                                }
                            }
                            else
                            {
                                throw new EmployeeNotFoundException("Sistemdə heç bir işçi yoxdur.");
                            }

                         }

                        static void ListEmployees()
                        {
                            
                            Console.WriteLine("--- Bütün İşçilərin Siyahısı ---");
                            EmployeeService.ListEmployees();

                           if (!EmployeeService.employees.Any())
                            {
                               throw new EmployeeNotFoundException("Sistemdə qeydə alınmış heç bir işçi yoxdur.");
                            }
                        }

                        static void FilterEmployees()
                        {
                            Console.WriteLine("--- İşçilərin Filterlənməsi ---");
                            Console.Write("Ada görə filter : ");
                            string nameFilter = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(nameFilter))
                            {
                                nameFilter = null;
                            }
                            Console.Write("Min Salary : ");
                            decimal? minSalary = null;
                            string minSalaryInput = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(minSalaryInput))
                            {
                                if (decimal.TryParse(minSalaryInput, out decimal minS))
                                {
                                    minSalary = minS;
                                }
                                else
                                {
                                    
                                    throw new InvalidSalaryException("Min Salary düzgün rəqəm formatında olmalıdır.");
                                }
                            }
                            Console.Write("Max Salary (Rəqəm. Boş buraxın - yoxdur): ");
                            decimal? maxSalary = null;
                            string maxSalaryInput = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(maxSalaryInput))
                            {
                                if (decimal.TryParse(maxSalaryInput, out decimal maxS))
                                {
                                    maxSalary = maxS;
                                }
                                else
                                {
                                    
                                    throw new InvalidSalaryException("Max Salary düzgün rəqəm formatında olmalıdır.");
                                }
                            }

                            List<Employee> filteredList = EmployeeService.FilterEmployees(nameFilter, minSalary, maxSalary);
                            
                            if (filteredList.Count > 0)
                            {
                                Console.WriteLine("** Filterlənmiş İşçilər **");
                                foreach (var emp in filteredList)
                                {
                                    emp.PrintInfo();
                                }
                            }
                            else { 
                                throw new EmployeeNotFoundException ("Verilmiş şərtlərə uyğun heç bir işçi tapılmadı.");
                            }

                        }


                }

                }
            }
        }
    }


