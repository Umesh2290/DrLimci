using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace LaboratorySystem
{
    public class SystemUserSessionContext
    {
        public void SetAuthenticationToken(string name, bool isPersistant, SystemUser sysusrData)
        {
            string host = HttpContext.Current.Request.Url.Host;
            string data = null;
            if (sysusrData != null)
                //data = new JavaScriptSerializer().Serialize(sysusrData);
                data = new JavaScriptSerializer().Serialize(sysusrData.Username);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, name, DateTime.Now, DateTime.Now.AddYears(1), isPersistant, data);

            string cookieData = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(host + "suser", cookieData)
            {
                HttpOnly = true,
                Expires = ticket.Expiration
            };

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public SystemUser GetMemberData()
        {
            SystemUser memdata = null;
            string host = HttpContext.Current.Request.Url.Host;

            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[host + "suser"];
                if (cookie != null)
                {
                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                        //memdata = new JavaScriptSerializer().Deserialize(ticket.UserData, typeof(SystemUser)) as SystemUser;
                        memdata = new SystemUser() { Username = ticket.UserData };
                }
            }
            catch (Exception ex)
            {
            }

            return memdata;
        }

    }
}