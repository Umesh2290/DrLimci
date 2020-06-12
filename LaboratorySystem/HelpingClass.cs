using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace LaboratorySystem
{
    public static class HelpingClass
    {
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static bool ValidateConnection()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["LabSystemDB_Connection"].ConnectionString))
                {
                    con.Open();
                    return true;
                }
           }
           catch (Exception ex)
           {
               return false;
           }
        } 

        public static System.Configuration.Configuration GetErrorConf()
        {
            return WebConfigurationManager.OpenWebConfiguration(@"~/Web.Config");
        }

        public static string Timeago(DateTime from, DateTime to)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(to.Ticks - from.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string GetSubdomain(this string url, string domain = null)
        {
            var subdomain = url;

            if (subdomain != null)
            {
                if (domain == null)
                {
                    // Since we were not provided with a known domain, assume that second-to-last period divides the subdomain from the domain.
                    var nodes = url.Split('.');
                    var lastNodeIndex = nodes.Length - 1;
                    if (lastNodeIndex > 0)
                        domain = nodes[lastNodeIndex - 1] + "." + nodes[lastNodeIndex];
                }

                // Verify that what we think is the domain is truly the ending of the hostname... otherwise we're hooped.
                if (!subdomain.EndsWith(domain))
                    throw new ArgumentException("Site was not loaded from the expected domain");

                //// Quash the domain portion, which should leave us with the subdomain and a trailing dot IF there is a subdomain.
                subdomain = subdomain.Replace(domain, "");
                //// Check if we have anything left.  If we don't, there was no subdomain, the request was directly to the root domain:
                if (string.IsNullOrWhiteSpace(subdomain))
                    return null;

                //// Quash any trailing periods
                subdomain = subdomain.TrimEnd(new[] { '.' });
            }

            return subdomain;
        }
        public static DateTime ConvertToDate(DateTime? a)
        {
            if (a.HasValue)
            {
                return a.Value.Date;
            }

            else
            {
                return DateTime.Now.Date;
            }
        }

        public static IEnumerable<Dictionary<string, object>> Serialize(DbDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(SerializeRow(cols, reader));

            return results;
        }
        private static Dictionary<string, object> SerializeRow(IEnumerable<string> cols,
                                                        DbDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }

        public static void Clear(string key, HttpContext bb)
        {
            var httpContext = new HttpContextWrapper(bb);
            var _response = httpContext.Response;

            HttpCookie cookie = new HttpCookie(key)
            {
                Expires = DateTime.Now.AddDays(-1) // or any other time in the past
            };
            _response.Cookies.Set(cookie);
        }

        public static string GetDefaultDirectory()
        {
            return WebConfigurationManager.AppSettings["DefaultDirectory"];
        }

        public static string CurrentWebUrl()
        {
            return WebConfigurationManager.AppSettings["CurrentUrl"];
        }

        public static string GetProtocol()
        {
            return WebConfigurationManager.AppSettings["Protocol"];
        }

        public static string GetDomainOnly()
        {
            string currentweburl = string.Empty;
            string domainonly = string.Empty;

            try
            {
                currentweburl = HelpingClass.CurrentWebUrl();
                string[] parts = currentweburl.Split(new string[] { "www." }, StringSplitOptions.None);
                domainonly = parts[1].Replace("/", "");
            }
            catch
            {
                currentweburl = string.Empty;
                domainonly = string.Empty;
            }

            return domainonly;
        }

        public static string MainCompanyName()
        {
            return WebConfigurationManager.AppSettings["MainCompanyName"];
        }

        public static string SpacetoSpaceCode(string str)
        {
            return str.Replace(" ", "&nbsp;");
        }

        public static string LocalUploadPathToRelativeWebPath(string path) 
        {
            return path.Replace(GetDefaultDirectory(), "").Replace("\\","/");
        }

    }

    public static class MyEnumExtensions
    {
        public static string ToDescriptionString(this ToastrEnum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToDescriptionString(this SwalEnum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}