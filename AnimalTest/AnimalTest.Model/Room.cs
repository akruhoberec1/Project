using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;

namespace AnimalTest.Models
{
    public class Room
    {
        public Guid Id { get; set; }    
        public int RoomNumber { get; set; }
        public string RoomTypeId { get; set; }  
        public RoomType RoomType { get; set; }
        public List<Reservation> Reservations { get; set; } 
    }
}
