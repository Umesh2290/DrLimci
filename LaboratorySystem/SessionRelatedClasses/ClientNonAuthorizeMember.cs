using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LaboratorySystem
{
    public class ClientNonAuthorizeMember : AuthorizeAttribute
    {
        ClientUserSessionContext sescon = new ClientUserSessionContext();
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            string requiredPermission = String.Format("{0}-{1}",
                   filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                   filterContext.ActionDescriptor.ActionName);

            string subdomain = "N/A";
            string sub = HttpContext.Current.Request.Url.DnsSafeHost.GetSubdomain();
            if (sub != null)
            {
                if (!sub.Equals("www") && !sub.Equals("127.0"))
                {
                    subdomain = sub + ".";// + HelpingClass.GetDomainOnly();
                }
            }


            ClientUser member = sescon.GetMemberData();

            if (member == null)
            {
                MySession.SetClientSession(subdomain, null);
            }

            if (member != null)
            {


                filterContext.Result = new RedirectToRouteResult(
                                               new RouteValueDictionary { 
                                                { "action", "Index" }, 
                                                { "controller", "User/Home" } });



            }

        }
    }
}