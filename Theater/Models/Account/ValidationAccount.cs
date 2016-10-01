using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Theater.WorkingDb.Connections;
using Theater.WorkingDb.Interfaces;

namespace Theater.Models.Account
{
    /// <summary>
    /// Class, which check user on correct
    /// </summary>
    public static class ValidationAccount
    {
        /// <summary>
        /// Checks for correct user
        /// </summary>
        /// <param name="user">User, who need check</param>
        /// <param name="confirmPassword">Field for compare with user password</param>
        /// <returns>If user correct return true, else return false</returns>
        public static bool IsTrueUser(User user, string confirmPassword)
        {
            return (IsTrueName(user.Name) && IsTruePhone(user.Phone) && IsTrueEmail(user.Email) &&
                    IsTruePassword(user.Password) && IsTrueConfirmPassword(user.Password, confirmPassword));
        }

        /// <summary>
        /// Checks for empty, length and correct chars name
        /// </summary>
        /// <param name="name">Name, which need check</param>
        /// <returns>If name correct return true, else return true</returns>
        public static bool IsTrueName(string name)
        {
            return !(string.IsNullOrWhiteSpace(name) ||
                    name.Length < 3 || hasIncorrectChars(name));            
        }        

        /// <summary>
        /// Checks for empty and length password 
        /// </summary>
        /// <param name="password">Password, which need check</param>
        /// <returns>if password corrent return true, else return false</returns>
        public static bool IsTruePassword(string password)
        {            
            return !(string.IsNullOrWhiteSpace(password) || password.Length < 3);
            
        }

        /// <summary>
        /// Compares password and confirm password
        /// </summary>
        /// <param name="password">First password</param>
        /// <param name="confirmPassword">Password for confirm</param>
        /// <returns>If passwords equals return true, else return false</returns>
        public static bool IsTrueConfirmPassword(string password, string confirmPassword)
        {
            return (!string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(confirmPassword) &&
                    password.Equals(confirmPassword));
            
        }

        /// <summary>
        /// Checks for empty, correct chars and database has email
        /// </summary>
        /// <param name="email">Email, which need check</param>
        /// <returns>If email correct return true, else return false</returns>
        public static bool IsTrueEmail(string email)
        {

            return !(string.IsNullOrWhiteSpace(email) || !hasAtChar(email) ||
                    thereIsInDb(email));
            
        }

        /// <summary>
        /// Checks for empty, length and correct chars
        /// </summary>
        /// <param name="phone">Phone, which need check</param>
        /// <returns>If phone correct return true, else return false</returns>
        public static bool IsTruePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            string pattern = @"\+\d{6,14}";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(phone);
        }

        private static bool thereIsInDb(string email)
        {
            ILoginDao sql = LoginsTableConnection.Instance;
            return !(sql.GetUserByEmail(email) == null);
        }

        private static bool hasAtChar(string email)
        {
            return (email.IndexOf('@') > 0);
        }

        private static bool hasIncorrectChars(string name)
        {
            string pattern = @"[^A-Za-zА-Яа-яёЁ]";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(name);
        }
    }
}