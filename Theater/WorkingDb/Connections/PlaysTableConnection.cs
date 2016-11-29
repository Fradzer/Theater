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
    public class PlaysTableConnection : IPlayDao
    {
        public static string ConnectionStr = ConfigurationManager.ConnectionStrings["TheaterDb"].ConnectionString;

        private const string readPlays = @"SELECT *
                                           FROM plays";

        private string searchPlayById = @"SELECT *
                                         FROM plays
                                         WHERE Id=@Id";

        private string deletePalyById = @"DELETE FROM plays
                                         WHERE Id=@Id";

        private string updatePlayById = @"UPDATE plays
                                         SET name=@name, authorId=@authorId, genreId=@genreId, description=@description
                                         WHERE Id=@Id";

        private string createPlay = @"INSERT plays(name, authorId, genreId, description)
                                         VALUES(@name, @authorId, @genreId, @description)";


        private static PlaysTableConnection instance;
        public static PlaysTableConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlaysTableConnection();
                }
                return instance;
            }
        }

        private PlaysTableConnection()
        {

        }
        public List<Play> GetAllPlays()
        {
            List<Play> plays = new List<Play>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand commandPlays = new SqlCommand(readPlays, connection);

                using (SqlDataReader readerPlays = commandPlays.ExecuteReader())
                {
                    while (readerPlays.Read())
                    {
                        plays.Add(new Play(Convert.ToInt32(readerPlays.GetValue(0)),
                                           Convert.ToString(readerPlays.GetValue(1)),
                                           Convert.ToInt32(readerPlays.GetValue(2)),
                                           Convert.ToInt32(readerPlays.GetValue(3)),
                                           Convert.ToString(readerPlays.GetValue(4))
                                       ));
                    }
                }
            }
            return plays;
        }

        public Play GetPlayById(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(searchPlayById, connection);

                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new Play(id,
                                         Convert.ToString(reader.GetValue(1)),
                                         Convert.ToInt32(reader.GetValue(2)),
                                         Convert.ToInt32(reader.GetValue(3)),
                                         Convert.ToString(reader.GetValue(4))
                                         );
                    }
                }
            }
            return null;
        }

        public void DeleteById(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(deletePalyById, connection);

                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }

        public void Create(Play play)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(createPlay, connection);

                command.Parameters.AddWithValue("@name", play.Name);
                command.Parameters.AddWithValue("@authorId", play.AuthorId);
                command.Parameters.AddWithValue("@genreId", play.GenreId);
                command.Parameters.AddWithValue("@description", play.Description);

                command.ExecuteNonQuery();
            }
        }

        public void Update(Play play)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(updatePlayById, connection);

                command.Parameters.AddWithValue("@Id", play.Id);
                command.Parameters.AddWithValue("@name", play.Name);
                command.Parameters.AddWithValue("@authorId", play.AuthorId);
                command.Parameters.AddWithValue("@genreId", play.GenreId);
                command.Parameters.AddWithValue("@description", play.Description);

                command.ExecuteNonQuery();
            }
        }
    }
}