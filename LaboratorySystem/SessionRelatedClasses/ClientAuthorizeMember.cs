using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LaboratorySystem
{
    public class ClientAuthorizeMember : AuthorizeAttribute
    {
        ClientUserSessionContext sescon = new ClientUserSessionContext();
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            string currenturl_controlleraction = filterContext.HttpContext.Request.RawUrl.Split('?')[0];
            string subdomain = "N/A";
            string sub = HttpContext.Current.Request.Url.DnsSafeHost.GetSubdomain();
            if (sub != null)
            {
                if (!sub.Equals("www") && !sub.Equals("127.0"))
                {
                    subdomain = sub + ".";// + HelpingClass.GetDomainOnly();
                }
            }

            if (MySession.GetClientSession(subdomain) == null)
            {
                HttpContext.Current.Response.Cookies.Remove(subdomain + "cuser");
                HttpContext.Current.Request.Cookies.Remove(subdomain + "cuser");
                HelpingClass.Clear(subdomain + "cuser", HttpContext.Current.ApplicationInstance.Context);

                filterContext.Result = new RedirectToRouteResult(
                                                   new RouteValueDictionary { 
                                                { "action", "Index" }, 
                                                { "controller", "User/Account" } });
            }
            else
            {
                ClientUser member = sescon.GetMemberData();
                if (member == null)
                {


                    filterContext.Result = new RedirectToRouteResult(
                                                   new RouteValueDictionary { 
                                                { "action", "Index" }, 
                                                { "controller", "User/Account" } });



                }

                else
                {
                    if (currenturl_controlleraction.EndsWith("/"))
                    {
                        currenturl_controlleraction = currenturl_controlleraction.Substring(0, currenturl_controlleraction.Length - 1);
                    }

                    if (MySession.GetClientSession(subdomain).MenuPriority == MenuPriorityEnum.Employee)
                    {
                        if (MySession.GetClientSession(subdomain).AssignedMenus.Where(x => x.Link.Equals(currenturl_controlleraction)).Count() <= 0)
                        {
                            filterContext.Result = new RedirectToRouteResult(
                                                   new RouteValueDictionary { 
                                                { "action", "NotFound" }, 
                                                { "controller", "User/Error" } });
                        }
                    }
                    else
                    {
                        if (MySession.GetClientSession(subdomain).CurrentRole.IsActive == false || MySession.GetClientSession(subdomain).CurrentRole.AssignedMenus.Where(x => x.Link.Equals(currenturl_controlleraction)).Count() <= 0)
                        {
                            filterContext.Result = new RedirectToRouteResult(
                                                   new RouteValueDictionary { 
                                                { "action", "NotFound" }, 
                                                { "controller", "User/Error" } });
                        }
                    }
                }
            }

        }
    }
}