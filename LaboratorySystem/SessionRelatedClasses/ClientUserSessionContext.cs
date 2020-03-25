using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace LaboratorySystem
{
    public class ClientUserSessionContext
    {
        public void SetAuthenticationToken(string name, bool isPersistant, ClientUser clientusrData)
        {
            string subdomain = "";
            string sub = HttpContext.Current.Request.Url.DnsSafeHost.GetSubdomain();
            if (sub != null)
            {
                if (!sub.Equals("www") && !sub.Equals("127.0"))
                {
                    subdomain = sub + ".";// + HelpingClass.GetDomainOnly();
                }
            }

            string data = null;
            if (clientusrData != null)
                //data = new JavaScriptSerializer().Serialize(clientusrData);
                data = new JavaScriptSerializer().Serialize(clientusrData.Username);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, name, DateTime.Now, DateTime.Now.AddYears(1), isPersistant, data);

            string cookieData = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(subdomain + "cuser", cookieData)
            {
                HttpOnly = true,
                Expires = ticket.Expiration
            };

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public ClientUser GetMemberData()
        {
            ClientUser memdata = null;
            string subdomain = "N/A";
            string sub = HttpContext.Current.Request.Url.DnsSafeHost.GetSubdomain();
            if (sub != null)
            {
                if (!sub.Equals("www") && !sub.Equals("127.0"))
                {
                    subdomain = sub + ".";// + HelpingClass.GetDomainOnly();
                }
            }

            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[subdomain + "cuser"];
                if (cookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                    //memdata = new JavaScriptSerializer().Deserialize(ticket.UserData, typeof(ClientUser)) as ClientUser;
                    memdata = new ClientUser() { Username = ticket.UserData };
                }
            }
            catch (Exception ex)
            {
            }

            return memdata;
        }

    }
}