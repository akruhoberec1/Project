using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimalTest.Models
{
    public class Guest : Person
    {
        public Guid Id { get; set; }
        public string Email { get; set; }   
        public int Phone { get; set; }  
        public Person Person { get; set; }
        public List<Reservation> Reservations { get; set; }

    }
}