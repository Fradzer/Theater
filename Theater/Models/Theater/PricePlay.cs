using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Theater.Models.Theater
{
    public class PricePlay
    {
        public string Name { get; set; }
        public decimal PriceParterre { get; set; }
        public decimal PriceBalcony { get; set; }

        public PricePlay()
        {
           
        }

        public PricePlay(string name, decimal priceParterre, decimal priceBalcony)
        {
            Name = name;
            PriceParterre = priceParterre;
            PriceBalcony = priceBalcony;
        }
    }
}