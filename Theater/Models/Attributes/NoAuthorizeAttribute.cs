using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Theater.Models.AuthorizeAttributes
{
    /// <summary>
    /// Attribute for non-authorized users
    /// </summary>
    public class NoAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return !HttpContext.Current.User.Identity.IsAuthenticated;
        }
    }
}