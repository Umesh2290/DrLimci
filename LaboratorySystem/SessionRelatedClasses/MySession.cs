using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboratorySystem
{
    public static class MySession
    {
        public static SystemUser SystemSession
        {
            get { return (SystemUser)System.Web.HttpContext.Current.Session["KEY_VAR2"]; }
            set { System.Web.HttpContext.Current.Session["KEY_VAR2"] = value; } 
        }

        public static ClientUser GetClientSession(string url)
        {
            return (ClientUser)System.Web.HttpContext.Current.Session["KEY_VAR2" + url];
            
        }

        public static bool SetClientSession(string url, ClientUser value)
        {
            try
            {
                System.Web.HttpContext.Current.Session["KEY_VAR2" + url] = value;
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}