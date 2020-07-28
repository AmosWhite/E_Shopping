using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using WebApp.Custom_Security;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.RegisterComponents();

        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            //geting the cookie that is stored in request object
            HttpCookie authCookie = Request.Cookies.Get
                (
                FormsAuthentication.FormsCookieName
                ); 

            //checking if cookie is null
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

                string udata = ticket.UserData;

                string[] userData = udata.Split('|');

                CustomPrincipal myUser = new CustomPrincipal(userData[1]);
                myUser.Id = Convert.ToInt32(userData[0]);
                myUser.Email = userData[2];
                myUser.RoleName = userData[1];


                HttpContext.Current.User = myUser;

            }
        }
    }
}
