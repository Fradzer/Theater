using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Theater.Models.Plays
{
    public enum Category
    {
        Parterre = 1,
        Balcony = 0
    }

    public enum StatusOrder
    {
        NotСonfirmed = 0,
        Performing = 1,
        Completed = 2
    }
    public class Order
    {
        public int Id { get; set; }
        public int DateId { get; set; }
        public int LoginId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public StatusOrder Status { get; set; }

        public Order()
        {

        }

        public Order(int id, int dateId, int loginId, int categoryId, int quatity, decimal price, int statusId)
        {
            Id = id;
            DateId = dateId;
            LoginId = loginId;
            Category = (Category)categoryId;
            Quantity = quatity;
            Price = price;
            Status = (StatusOrder)statusId;
        }
    }
}