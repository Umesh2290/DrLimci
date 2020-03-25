using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LaboratorySystem
{
    public class SystemNonAuthorizeMember : AuthorizeAttribute
    {
        SystemUserSessionContext sescon = new SystemUserSessionContext();
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            string requiredPermission = String.Format("{0}-{1}",
                   filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                   filterContext.ActionDescriptor.ActionName);


            SystemUser member = sescon.GetMemberData();

            if (member == null)
            {
                MySession.SystemSession = null;
            }

            if (member != null)
            {


                filterContext.Result = new RedirectToRouteResult(
                                               new RouteValueDictionary { 
                                                { "action", "Index" }, 
                                                { "controller", "Admin/Home" } });



            }

        }
    }
}