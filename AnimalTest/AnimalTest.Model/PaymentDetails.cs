using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimalTest.Models
{
    public class PaymentDetails
    {
        public Guid Id { get; set; }    
        public string PaymentMethod { get; set; }
        public int CardNumber { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}