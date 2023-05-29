using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimalTest.Models
{
    public class Employee
    {
        public Guid Id { get; set; }    
        public decimal Salary { get; set; } 
        public bool Certified { get; set; } 
    }
}