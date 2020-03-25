using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LaboratorySystem
{
    public class SystemAuthorizeMember : AuthorizeAttribute
    {
        SystemUserSessionContext sescon = new SystemUserSessionContext();
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            string currenturl_controlleraction = filterContext.HttpContext.Request.RawUrl.Split('?')[0];


            if (MySession.SystemSession == null)
            {
                HttpContext.Current.Response.Cookies.Remove(HttpContext.Current.Request.Url.Host + "suser");
                HttpContext.Current.Request.Cookies.Remove(HttpContext.Current.Request.Url.Host + "suser");
                HelpingClass.Clear(HttpContext.Current.Request.Url.Host + "suser", HttpContext.Current.ApplicationInstance.Context);

                filterContext.Result = new RedirectToRouteResult(
                                                   new RouteValueDictionary { 
                                                { "action", "Index" }, 
                                                { "controller", "Admin/Account" } });
            }
            else
            {
                SystemUser member = sescon.GetMemberData();
                if (member == null)
                {


                    filterContext.Result = new RedirectToRouteResult(
                                                   new RouteValueDictionary { 
                                                { "action", "Index" }, 
                                                { "controller", "Admin/Account" } });



                }

                else
                {
                    if (currenturl_controlleraction.EndsWith("/"))
                    {
                        currenturl_controlleraction = currenturl_controlleraction.Substring(0, currenturl_controlleraction.Length - 1);
                    }

                    if (MySession.SystemSession.MenuPriority == MenuPriorityEnum.Employee)
                    {
                        if (MySession.SystemSession.AssignedMenus.Where(x => x.Link.Equals(currenturl_controlleraction)).Count() <= 0)
                        {
                            filterContext.Result = new RedirectToRouteResult(
                                                   new RouteValueDictionary { 
                                                { "action", "NotFound" }, 
                                                { "controller", "Admin/Error" } });
                        }
                    }
                    else
                    {
                        if (MySession.SystemSession.CurrentRole.IsActive==false || MySession.SystemSession.CurrentRole.AssignedMenus.Where(x => x.Link.Equals(currenturl_controlleraction)).Count() <= 0)
                        {
                            filterContext.Result = new RedirectToRouteResult(
                                                   new RouteValueDictionary { 
                                                { "action", "NotFound" }, 
                                                { "controller", "Admin/Error" } });
                        }
                    }
                }
            }

        }
    }
}