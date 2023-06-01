using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimalTest.Models
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public bool HasDinner { get; set; }
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
        public Guid GuestId { get; set; }
        public List<Guest> Guests { get; set; }
        public Guid PaymentDetailsId { get; set; }
        public PaymentDetails PaymentDetails { get; set; }
    }
}