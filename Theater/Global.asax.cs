using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Theater.Models;
using Theater.Models.Theater;
using Theater.WorkingDb.Connections;

namespace Theater
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            if(HttpContext.Current.User != null)
            {
                if(HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity id =
                            (FormsIdentity)HttpContext.Current.User.Identity;

                        var user = LoginsTableConnection.Instance.GetUserByEmail(id.Name);
                        if (user == null)
                        {
                            HttpContext.Current.User = null;
                            HttpContext.Current.Items[CurrentUserService.CurrentUserKey] = null;

                        }
                        else
                        {
                            HttpContext.Current.Items[CurrentUserService.CurrentUserKey] = user;
                            HttpContext.Current.User = new GenericPrincipal(id, new string[] { user.Role.ToString() });
                        }
                    }
                }
            }
        }
        
    }
}
