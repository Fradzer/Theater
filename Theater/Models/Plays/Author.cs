using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Theater.Models.Plays
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Author()
        {

        }

        public Author(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}