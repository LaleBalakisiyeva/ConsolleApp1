using ConsoleApp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Models
{
 public abstract class Person
    {
        public int ID { get; set; }

        private string _name;
        public string Name { 
            get
            {
              
                return _name;
            }
                
            set
            {

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NameEmptyException("Bos ola bilmez");
                }
                if (value.Length<3 || value.Length>25)
                {
                    throw new NameLengthException("Bu ad uzunluga uymur");
                }
                _name = value;
            }
        }

        public Person(int id, string name)
        {
            ID = id;
            Name = name;

        }

        public abstract void PrintInfo();
        

    }
}
