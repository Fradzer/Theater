using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theater.Models.Plays;

namespace Theater.WorkingDb.Interfaces
{
    public interface IOrderDao
    {
        /// <summary>
        /// Search all orders
        /// </summary>
        /// <returns>return list with all orders</returns>
        List<Order> GetAllOrders();

        /// <summary>
        /// Search order by id
        /// </summary>
        /// <param name="id">order id</param>
        /// <returns>return order by id</returns>
        Order GetOrderById(int id);

        /// <summary>
        /// Seach count busy seets in date and category
        /// </summary>
        /// <param name="dateId">order date</param>
        /// <param name="categoryId">category seat for order</param>
        /// <returns>return count busy seats</returns>
        int GetCountBusySeetsByDateIdAndCategory(int dateId, int categoryId);

        /// <summary>
        /// Add new order in database
        /// </summary>
        /// <param name="order">order, who need add</param>
        void AddOrder(Order order);

        /// <summary>
        /// Search order by login id
        /// </summary>
        /// <param name="loginId">login id</param>
        /// <returns>return order by login id</returns>
        List<Order> GetOrdersByIdLogin(int loginId);

        /// <summary>
        /// Change status order by id
        /// </summary>
        /// <param name="id">order id</param>
        /// <param name="newStatus">new status for order</param>
        void UpdateOrderStatusById(int id, int newStatus);
        List<Order> GetOrderByCountTicketsAndUserId(int countTickets, int userId);

        /// <summary>
        /// Delete order by id
        /// </summary>
        /// <param name="orderId">order id, which need delete</param>
        void DeleteOrderById(int orderId);
        List<Order> GetOrderByCountTickets(int countTickets);
    }
}
