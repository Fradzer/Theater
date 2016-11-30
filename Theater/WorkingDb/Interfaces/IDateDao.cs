using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theater.Models.Account;
using Theater.Models.Plays;

namespace Theater.WorkingDb.Interfaces
{
    public interface IDateDao
    {
        /// <summary>
        /// Search all dates from database
        /// </summary>
        /// <returns>return list with all dates</returns>
        List<DatePlay> GetAllDates();

        /// <summary>
        /// Search all dates by play id
        /// </summary>
        /// <param name="id">play id</param>
        /// <returns>return all dates by play's id</returns>
        List<DatePlay> GetDatesByIdPlay(int id);
        
        /// <summary>
        /// Search date by id
        /// </summary>
        /// <param name="id">date id</param>
        /// <returns>return date by id</returns>
        DatePlay GetDateById(int id);
        void DeleteById(int id);
        void Create(DatePlay date);
        void Update(DatePlay date);
        DatePlay GetDate(DateTime newDate);
    }
}
