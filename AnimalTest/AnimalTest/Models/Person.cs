using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimalTest.Models
{
    public class Person
    {
        public Guid Id { get; set; }    
        public string FirstName { get; set; }
        public string LastName { get; set; }    
        public string OIB { get; set; }
        public Guest Guest { get; set; }
        public Employee Employee { get; set; }  

    }
}