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
    public class AuthorsTableConnection : IAuthorDao
    {
        public static string ConnectionStr = ConfigurationManager.ConnectionStrings["TheaterDb"].ConnectionString;

        private const string readAuthors = @"SELECT *
                                           FROM authors";

        private string searchAuthorById = @"SELECT *
                                         FROM authors
                                         WHERE Id=@Id";

        private static AuthorsTableConnection instance;
        public static AuthorsTableConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AuthorsTableConnection();
                }
                return instance;
            }
        }

        private AuthorsTableConnection()
        { }
        public List<Author> GetAllAuthors()
        {
            List<Author> authors = new List<Author>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand commandPlays = new SqlCommand(readAuthors, connection);
                using (SqlDataReader readerPlays = commandPlays.ExecuteReader())
                {
                    while (readerPlays.Read())
                    {
                        authors.Add(new Author(Convert.ToInt32(readerPlays.GetValue(0)),
                                           Convert.ToString(readerPlays.GetValue(1))
                                       ));
                    }
                }
            }
            return authors;
        }

        public Author GetAuthorById(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(searchAuthorById, connection);

                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new Author(id,
                                         Convert.ToString(reader.GetValue(1)));
                    }
                }
            }
            return null;
        }
    }
}