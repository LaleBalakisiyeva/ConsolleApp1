using ConsoleApp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp.Models
{
    public interface IPrintable
    {
        void PrintEmployeeInfo();
    }
    public  class Employee : Person,IPrintable
    {
        private Position? Position { get; set; }
        private decimal? _salary;
        private string _department;
        private Contact? _workInfo;
        public DateTime HireDate { get; }
        public Contact? WorkInfo
        {
            get => _workInfo;
            set => _workInfo = value;
        }



        public Employee(int id, string name, string department, DateTime hireDate,decimal? salary = null, Position? position = null, Contact? workInfo = null) : base(id, name)
        {
            HireDate = hireDate;
            Department = department;
            Salary = salary;
            Position = position;
            WorkInfo = workInfo;
        }

        public string Department
        {
            get
            {

                return _department;
            }
            set
            {
                if (value == null)
                {
                    throw new NameEmptyException("Bos ola bilmez");
                }
                if (value.Length < 5 || value.Length > 6)
                {
                    throw new NameLengthException("Bu Department uzunluga uymur");
                }
                _department = value;
            }

        }

        public decimal? Salary
        {
            get
            {

                return _salary;
            }
            set
            {
                if(value <= 0)
                {
                    throw new InvalidSalaryException("sifirdan kicik ola bilmez");
                }
                _salary = value;
            }

        }

        public override void PrintInfo()
        {
            PrintEmployeeInfo();
        }

        public void PrintEmployeeInfo()
        {
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine($"ID: {ID}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Position: {(Position.HasValue ? Position.ToString() : "N/A")}");
            Console.WriteLine($"Salary: {(Salary.HasValue ? Salary.Value.ToString("C") : "N/A")}");
            Console.WriteLine($"Department: {(_department ?? "N/A")}");
            Console.WriteLine($"Hire Date: {HireDate.ToShortDateString()}");
            Console.WriteLine($"Work Info: {(WorkInfo.HasValue ? WorkInfo.Value.ToString() : "N/A")}");
            Console.WriteLine("---------------------------------------------");
        }
    }
}
