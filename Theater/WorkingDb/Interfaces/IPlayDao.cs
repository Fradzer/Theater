using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theater.Models.Plays;

namespace Theater.WorkingDb.Interfaces
{
    public interface IPlayDao
    {
        /// <summary>
        /// Search all plays in database
        /// </summary>
        /// <returns>return list with all plays</returns>
        List<Play> GetAllPlays();

        /// <summary>
        /// Search play by id
        /// </summary>
        /// <param name="id">play id</param>
        /// <returns>return play by id</returns>
        Play GetPlayById(int id);
        void DeleteById(int id);
        void Create(Play play);
        void Update(Play play);
    }
}
