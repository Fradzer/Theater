using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Theater.Models.Theater
{
    public static class TheaterInformation
    {
        public static decimal PriceBalcony = 50000;
        public static decimal PriceParterre = 35000;
        public static int TotalCountBalconySeats = 50;
        public static int TotalCountParterreSeats = 50;

        internal static decimal GetPriceByCategoryId(int category)
        {
            if (category == 1) return PriceParterre;
            else  return PriceBalcony;            
        }
    }
}