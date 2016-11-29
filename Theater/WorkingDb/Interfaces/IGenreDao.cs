using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theater.Models.Plays;

namespace Theater.WorkingDb.Interfaces
{
    public interface IGenreDao
    {
        /// <summary>
        /// Search all genres
        /// </summary>
        /// <returns>return list with all genres</returns>
        List<Genre> GetAllGenres();
        
        /// <summary>
        /// Search genre by id
        /// </summary>
        /// <param name="id">genre id</param>
        /// <returns>return genre by id</returns>
        Genre GetGenreById(int id);
        void DeleteById(int id);
        void Create(Genre genre);
        void Update(Genre genre);
    }
}
