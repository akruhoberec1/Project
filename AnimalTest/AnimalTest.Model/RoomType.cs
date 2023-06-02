using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimalTest.Models
{
    public class RoomType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }    
        public decimal Price { get; set; }  
        public List<Room> Rooms { get; set; }
    }
}