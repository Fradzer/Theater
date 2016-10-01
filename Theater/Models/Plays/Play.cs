using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Theater.Models.Plays
{
    public class Play
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
       
        public string Description { get; set; }

        public Play()
        { }
        public Play(int id, string name, int authorId, int genderId,
                    string description)
        {
            Id = id;
            Name = name;
            AuthorId = authorId;
            GenreId = genderId;
            Description = description;
        }
    }
}