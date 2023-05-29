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
        public int OIB { get; set; }
        public bool IsEmployee { get; set; }    
        public bool IsGuest { get; set; }   

    }
}