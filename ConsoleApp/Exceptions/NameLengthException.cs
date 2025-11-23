using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Exceptions
{
    public class NameLengthException : Exception
    {
        public NameLengthException(string message) : base(message) { }
    }
}
