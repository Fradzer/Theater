using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Theater.Models.Theater
{
    public class TypeSeat
    {
        public string Name { get; set; }
        public int CountSeats { get; set; }

        public TypeSeat()
        {

        }

        public TypeSeat(string name, int count)
        {
            Name = name;
            CountSeats = count;
        }
    }
}