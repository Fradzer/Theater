using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Theater.Models.Account;

namespace Theater.Models
{
    public static class CurrentUserService
    {
        public static string CurrentUserKey = "CurrentUserKey";

        public static User GetCurrentUser()
        {
            if(HttpContext.Current.Items[CurrentUserKey] == null)
            {
                return null;
            }
            return HttpContext.Current.Items[CurrentUserKey] as User;
        }
    }
}