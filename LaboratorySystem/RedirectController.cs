using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using LaboratorySystem;

namespace LaboratorySystem
{
    public class RedirectController:Controller
    {
        public string subdomainurl { get; set; }
        public DBInitializer currentdomaindb { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            string sub = filterContext.RequestContext.HttpContext.Request.Url.DnsSafeHost.GetSubdomain();
            if (sub != null)
            {
                if (!sub.Equals("www") && !sub.Equals("127.0"))
                {
                    if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName.Contains("Admin"))
                    {
                        filterContext.Result= RedirectToRoute(new { controller = "User/Home", action = "Index" });
                        return;
                    }

                    else
                    {
                        try
                        {
                            this.subdomainurl = sub + "."+HelpingClass.GetDomainOnly();
                            var systemobj = SystemUser.GetSystemUserBySubdomain(this.subdomainurl);
                            if (systemobj == null)
                            {
                                filterContext.Result = RedirectToRoute(new { controller = "User/Error", action = "SubDomainNotFound" });
                                return;
                            }
                            else
                            {
                                if (systemobj.IsActive == false)
                                {
                                    filterContext.Result = RedirectToRoute(new { controller = "User/Error", action = "InactiveAccount" });
                                    return;
                                }
                                else
                                {
                                    if (((DBInitializer)System.Web.HttpContext.Current.Session[this.subdomainurl + "db"]) == null)
                                    {
                                        var cl = new Client(this.subdomainurl);
                                        DBInitializer innerdb = new DBInitializer(cl);
                                        this.currentdomaindb = innerdb;
                                        System.Web.HttpContext.Current.Session[this.subdomainurl + "db"] = innerdb;
                                    }
                                    else
                                    {
                                        this.currentdomaindb = ((DBInitializer)System.Web.HttpContext.Current.Session[this.subdomainurl + "db"]);
                                    }
                                }
                            }
                            base.OnActionExecuting(filterContext);
                        }
                        catch (Exception ex)
                        {
                            filterContext.Result = RedirectToRoute(new { controller = "User/Error", action = "Exception" });
                            return;
                        }
                    }
                }

                else
                {
                    if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName.Contains("User"))
                    {
                        filterContext.Result = RedirectToRoute(new { controller = "Admin/Home", action = "Index" });
                        return;
                    }
                    base.OnActionExecuting(filterContext);
                }

            }

            else
            {
                if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName.Contains("User"))
                {
                    filterContext.Result = RedirectToRoute(new { controller = "Admin/Home", action = "Index" });
                    return;
                }
                base.OnActionExecuting(filterContext);
            }
        }

        
    }
}