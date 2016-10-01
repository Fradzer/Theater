using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theater.Models.Account;

namespace Theater.WorkingDb.Interfaces
{
    public interface ILoginDao
    {
        /// <summary>
        /// Add account in database
        /// </summary>
        /// <param name="user">User, who need add in database</param>
        void AddAccount(User user);

        /// <summary>
        /// Search all account in database
        /// </summary>
        /// <returns>return list with accounts</returns>
        List<User> GetAllLogins();

        /// <summary>
        /// Search account by id
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>return account by id</returns>
        User GetUserById(int id);

        /// <summary>
        /// Search user by email
        /// </summary>
        /// <param name="email">A email</param>
        /// <returns>return account by email</returns>
        User GetUserByEmail(string email);
        
        /// <summary>
        /// Search account by email and password
        /// </summary>
        /// <param name="email">account email</param>
        /// <param name="password">account password</param>
        /// <returns>return account by email and password</returns>
        User GetUserByEmailAndPassword(string email, string password);
    }
}
