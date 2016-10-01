using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Theater.Models.Theater
{
    public class PriceCollection
    {
        public List<PricePlay> Prices { get; set; }

        public int Count
        {
            get
            {
                return Prices.Count;
            }
        }
        public PriceCollection()
        {
            Prices = new List<PricePlay>();
        }

        public List<PricePlay> GetAll()
        {
            return Prices;
        }

        public PricePlay GetPricesByName(string name)
        {
            foreach (var item in Prices)
            {
                if (item.Name.Equals(name))
                {
                    return item;
                }
            }
            return null;
        }

        public void AddPrice(PricePlay pricePlay)
        {
            if (pricePlay != null)
            {
                Prices.Add(pricePlay);
            }
        }

        public decimal GetPricesByNameAndCategoryId(string name, int categoryId)
        {
            PricePlay price = GetPricesByName(name);
            if (categoryId == 0)
            {
                return price.PriceBalcony;
            }
            if (categoryId == 1)
            {
                return price.PriceParterre;
            }
            throw new InvalidOperationException();
        }
    }
}