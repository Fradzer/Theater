using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Theater.Models.Attribute
{
    /// <summary>
    /// attribute for destination desired language
    /// </summary>
    public class CultureAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (HttpContext.Current.Session["lang"] != null)
            {
                string lang = HttpContext.Current.Session["lang"].ToString();
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang);
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["lang"] != null)
            {
                string lang = HttpContext.Current.Session["lang"].ToString();
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang);
            }
        }
    }
}