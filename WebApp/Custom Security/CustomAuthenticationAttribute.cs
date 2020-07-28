using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace WebApp.Custom_Security
{
    public class CustomAuthenticationAttribute : FilterAttribute, IAuthenticationFilter
    {
        //executes b4 action method      
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            // through argument we can access our httpContext and through that context we can access our user
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        //executes after the action method but b4 generation of the action method result
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller =
                    "Account",
                    action = "login",
                
                }));
            }
        }
    }
}