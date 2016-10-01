using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Theater.Models.Plays;
using Theater.WorkingDb.Interfaces;

namespace Theater.WorkingDb.Connections
{
    public class OrdersTableConnection : IOrderDao
    {
        public static string ConnectionStr = ConfigurationManager.ConnectionStrings["TheaterDb"].ConnectionString;

        string readOrders = @"SELECT *
                              FROM orders";

        private const string searchOrderById = @"SELECT *
                                         FROM orders
                                         WHERE Id=@Id";

        private const string deleteOrderById = @"DELETE FROM orders
                                         WHERE Id=@Id";

        private const string searchOrdersByIdLogin = @"SELECT *
                                                FROM orders
                                                WHERE loginId=@loginId";

        private string searchCountBusySeetsByDateIdAndCategory = @"SELECT SUM(quantity)
                                                                   FROM orders
                                                                   Where dateId=@dateId AND categoryId=@categoryId";

        private const string addOrder = @"INSERT INTO orders(dateId, loginId, categoryId, quantity, price, statusOrderId)
                                            VALUES(@DateId, @LoginId, @CategoryId, @Quantity, @Price, @StatusOrderId)";

        private const string changeOrderStatusById = @"UPDATE orders 
	                                                   SET statusOrderId = @NewStatus
	                                                   WHERE Id = @Id";

        private static OrdersTableConnection instance;
        public static OrdersTableConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrdersTableConnection();
                }
                return instance;
            }
        }

        private OrdersTableConnection()
        {

        }


        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand commandDates = new SqlCommand(readOrders, connection);
                using (SqlDataReader readerDates = commandDates.ExecuteReader())
                {
                    while (readerDates.Read())
                    {
                        orders.Add(new Order(Convert.ToInt32(readerDates.GetValue(0)),
                                      Convert.ToInt32(readerDates.GetValue(1)),
                                      Convert.ToInt32(readerDates.GetValue(2)),
                                      Convert.ToInt32(readerDates.GetValue(3)),
                                      Convert.ToInt32(readerDates.GetValue(4)),
                                      Convert.ToDecimal(readerDates.GetValue(5)),
                                      Convert.ToInt32(readerDates.GetValue(6))
                                       ));
                    }
                }
            }
            return orders;
        }

        public Order GetOrderById(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(searchOrderById, connection);

                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new Order(id,
                                      Convert.ToInt32(reader.GetValue(1)),
                                      Convert.ToInt32(reader.GetValue(2)),
                                      Convert.ToInt32(reader.GetValue(3)),
                                      Convert.ToInt32(reader.GetValue(4)),
                                      Convert.ToDecimal(reader.GetValue(5)),
                                      Convert.ToInt32(reader.GetValue(6))
                                         );
                    }
                }
            }
            return null;
        }

        public int GetCountBusySeetsByDateIdAndCategory(int dateId, int categoryId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(searchCountBusySeetsByDateIdAndCategory, connection);

                command.Parameters.AddWithValue("@dateId", dateId);
                command.Parameters.AddWithValue("@categoryId", categoryId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read() && reader.GetValue(0) != DBNull.Value)
                    {
                        return Convert.ToInt32(reader.GetValue(0));
                    }
                }
            }
            return 0;
        }

        public void AddOrder(Order order)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(addOrder, connection);
                command.Parameters.AddWithValue("@DateId", order.DateId);
                command.Parameters.AddWithValue("@LoginId", order.LoginId);
                command.Parameters.AddWithValue("@CategoryId", (int)order.Category);
                command.Parameters.AddWithValue("@Quantity", order.Quantity);
                command.Parameters.AddWithValue("@Price", order.Price);
                command.Parameters.AddWithValue("@StatusOrderId", order.Status);

                command.ExecuteNonQuery();
            }
        }

        public List<Order> GetOrdersByIdLogin(int loginId)
        {
            List<Order> orders = new List<Order>(); 
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(searchOrdersByIdLogin, connection);

                command.Parameters.AddWithValue("@loginId", loginId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new Order(Convert.ToInt32(reader.GetValue(0)),
                                      Convert.ToInt32(reader.GetValue(1)),
                                      Convert.ToInt32(reader.GetValue(2)),
                                      Convert.ToInt32(reader.GetValue(3)),
                                      Convert.ToInt32(reader.GetValue(4)),
                                      Convert.ToDecimal(reader.GetValue(5)),
                                      Convert.ToInt32(reader.GetValue(6))
                                         ));
                    }
                }
            }
            return orders;
        }

        public void DeleteOrderById(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(deleteOrderById, connection);

                command.Parameters.AddWithValue("@Id", orderId);
                command.ExecuteNonQuery();
            }             
        }

        public void UpdateOrderStatusById(int id, int newStatus)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(changeOrderStatusById, connection);

                command.Parameters.AddWithValue("@NewStatus", newStatus);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }
    }

}