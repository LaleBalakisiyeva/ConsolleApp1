using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Exceptions
{
    public class DuplicateEmployeeException : Exception
    {
        public DuplicateEmployeeException(string message) : base(message) { }
    }
}
