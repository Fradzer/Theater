using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Theater.Models.Account;
using Theater.WorkingDb.Interfaces;

namespace Theater.WorkingDb.Connections
{
    public class LoginsTableConnection : ILoginDao
    {
        public static string ConnectionStr = ConfigurationManager.ConnectionStrings["TheaterDb"].ConnectionString;

        private const string readLogins = @"SELECT *
                                            FROM logins";


        private const string addAccount = @"INSERT INTO logins(name, password, roleId, email, phone)
                                            VALUES(@Name, @Password, @RoleId, @Email, @Phone)";

        private string searchByEmail = @"SELECT *
                                         FROM logins
                                         WHERE email=@Email";

        private string searchByEmailAndPassword = @"SELECT *
                                                 FROM logins
                                                 WHERE email=@Email AND password=@Password";

        private string searchUserById = @"SELECT *
                                         FROM logins
                                         WHERE Id=@Id";

        private static LoginsTableConnection instance;
        public static LoginsTableConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoginsTableConnection();
                }
                return instance;
            }
        }

        private LoginsTableConnection()
        {

        }

        public void AddAccount(User user)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(addAccount, connection);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@RoleId", (int)user.Role);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Phone", user.Phone);

                command.ExecuteNonQuery();
            }
        }

        public User GetUserByEmail(string email)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(searchByEmail, connection);

                command.Parameters.AddWithValue("@Email", email);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new User(Convert.ToInt32(reader.GetValue(0)),
                                        Convert.ToString(reader.GetValue(1)),
                                        Convert.ToString(reader.GetValue(2)),
                                        Convert.ToInt32(reader.GetValue(3)),
                                        email,
                                        Convert.ToString(reader.GetValue(5))
                                        );
                    }
                }
            }
            return null;

        }

        public User GetUserByEmailAndPassword(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(searchByEmailAndPassword, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new User(Convert.ToInt32(reader.GetValue(0)),
                                        Convert.ToString(reader.GetValue(1)),
                                        password,
                                        Convert.ToInt32(reader.GetValue(3)),
                                        email,
                                        Convert.ToString(reader.GetValue(5))
                                        );
                    }
                    return null;
                }
            }
        }

        public User GetUserById(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(searchUserById, connection);

                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new User(id,
                                      Convert.ToString(reader.GetValue(1)),
                                      Convert.ToString(reader.GetValue(2)),
                                      Convert.ToInt32(reader.GetValue(3)),
                                      Convert.ToString(reader.GetValue(4)),
                                        Convert.ToString(reader.GetValue(5))
                                         );
                    }
                }
            }
            return null;
        }

        public List<User> GetAllLogins()
        {
            List<User> logins = new List<User>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand commandDates = new SqlCommand(readLogins, connection);
                using (SqlDataReader readerDates = commandDates.ExecuteReader())
                {
                    while (readerDates.Read())
                    {
                        logins.Add(new User(Convert.ToInt32(readerDates.GetValue(0)),
                                            Convert.ToString(readerDates.GetValue(1)),
                                            Convert.ToString(readerDates.GetValue(2)),
                                            Convert.ToInt32(readerDates.GetValue(3)),
                                            Convert.ToString(readerDates.GetValue(4)),
                                            Convert.ToString(readerDates.GetValue(5))));
                    }
                }
            }
            return logins;
        }
    }
}