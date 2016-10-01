using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Theater.Models.Plays;
using Theater.WorkingDb.Interfaces;

namespace Theater.WorkingDb.Connections
{
    public class DatesTableConnection: IDateDao
    {
        public static string ConnectionStr = ConfigurationManager.ConnectionStrings["TheaterDb"].ConnectionString;
        
        private const string readDates = @"SELECT *
                                           FROM dates";

        private string searchDatesByIdPaly = @"SELECT *
                                         FROM dates
                                         WHERE playsId=@Id";

        private string searchDateById = @"SELECT *
                                         FROM dates
                                         WHERE Id=@Id";

        private static DatesTableConnection instance = null;
        public static DatesTableConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DatesTableConnection();
                }
                return instance;
            }
        }

        private DatesTableConnection()
        { }

        public List<DatePlay> GetAllDates()
        {
            List<DatePlay> dates = new List<DatePlay>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand commandDates = new SqlCommand(readDates, connection);
                using (SqlDataReader readerDates = commandDates.ExecuteReader())
                {
                    while (readerDates.Read())
                    {
                        dates.Add(new DatePlay(Convert.ToInt32(readerDates.GetValue(0)),
                                       Convert.ToInt32(readerDates.GetValue(1)),
                                       DateTime.Parse(Convert.ToString(readerDates.GetValue(2)))));
                    }
                }
            }
            return dates;

        }    
        public List<DatePlay> GetDatesByIdPlay(int id)
        {
            List<DatePlay> dates = new List<DatePlay>();

            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(searchDatesByIdPaly, connection);

                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dates.Add(new DatePlay(Convert.ToInt32(reader.GetValue(0)),
                                               Convert.ToInt32(reader.GetValue(1)),
                                               DateTime.Parse(Convert.ToString(reader.GetValue(2)))));
                    }
                }
            }
            return dates;

        }
        public DatePlay GetDateById(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(searchDateById, connection);

                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new DatePlay(id,
                                         Convert.ToInt32(reader.GetValue(1)),
                                         DateTime.Parse(Convert.ToString(reader.GetValue(2)))
                                         );
                    }
                }
            }
            return null;
        }     
    }
}