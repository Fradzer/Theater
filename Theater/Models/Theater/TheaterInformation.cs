using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Theater.Models.Theater
{
    public static class TheaterInformation
    {
        private static TypeSeat parterre = new TypeSeat();
        private static TypeSeat balcony = new TypeSeat();
        private static PriceCollection prices = new PriceCollection();

        public static TypeSeat Parterre
        {
            get
            {
                return parterre;
            }
            set
            {
                parterre = value;
            }
        }
        public static TypeSeat Balcony
        {
            get
            {
                return balcony;
            }
            set
            {
                balcony = value;
            }
        }
        public static PriceCollection Prices
        {
            get
            {
                return prices;
            }
            set
            {
                prices = value;
            }
        }
       
    }
}