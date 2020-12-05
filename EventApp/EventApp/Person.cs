using System;
using System.Collections.Generic;
using System.Text;

namespace EventApp
{
    class Person
    {   
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int OIB { get; set; }
        public int PhoneNumber { get; set; }

        public Person(string firstName, string lastName, int oib, int phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            OIB = oib;
            PhoneNumber = phoneNumber;
        }
    }
}
