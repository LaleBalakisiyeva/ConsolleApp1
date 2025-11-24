using ConsoleApp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Models
{
   public struct Contact
    {
        public string OfficeNumber { get; set; }
        public int Floor { get; set; }

        public Contact(string officenumber,int floor)
        {
           this.OfficeNumber = officenumber;
           this.Floor = floor;


            if (string.IsNullOrWhiteSpace(officenumber))
            {
                throw new InvalidWorkInfoException("OfficeNumber bos ola bilmez");
            }
            if(floor <0)
            {
                throw new InvalidWorkInfoException("Floor sifirdan kicik ola bilmez");
            }
        }


    }
}
