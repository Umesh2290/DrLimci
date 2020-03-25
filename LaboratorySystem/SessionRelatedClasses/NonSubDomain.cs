using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LaboratorySystem.SessionRelatedClasses
{
    public class NonSubDomain : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            string sub = HttpContext.Current.Request.Url.DnsSafeHost.GetSubdomain();
            if (sub != null)
            {
                if (!sub.Equals("www") && !sub.Equals("127.0"))
                {
                    filterContext.Result = new RedirectToRouteResult(
                                               new RouteValueDictionary { 
                                                { "action", "Index" }, 
                                                { "controller", "User/Home" } });
                }
            }

        }
    }
}