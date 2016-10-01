using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theater.Models.Plays;

namespace Theater.WorkingDb.Interfaces
{
    public interface IAuthorDao
    {
        /// <summary>
        /// Search all authors from database
        /// </summary>
        /// <returns>return list with all authors</returns>
        List<Author> GetAllAuthors();

        /// <summary>
        /// Search author by id
        /// </summary>
        /// <param name="id">author id</param>
        /// <returns>return author by id</returns>
        Author GetAuthorById(int id);


    }
}
