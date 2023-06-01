using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimalTest.Models
{
    public class UpdateEmployeeRest
    {
        public Guid Id { get; set; }    
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OIB { get; set; }
        public decimal Salary { get; set; }
        public bool Certified { get; set; }
    }
}