using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using BLL = LaboratoryBusiness.BLL;
using BusinessPOCO = LaboratoryBusiness.POCO;
using Repositories = LaboratoryBusiness.Repositories;

namespace LaboratorySystem
{
    public class NotificationManager
    {
        public static bool Fire(string Title,string Description,string ClickLink,int EmployeeID,int CreatedBy
            , string Icon = "i-Speach-Bubble-6 text-primary mr-1", string decoTitle = "", string decoDescription = "", string decoResponseType = "Simple")
        {
            try
            {
                Repositories.Admin.INotificationRepository notfication = new BLL.Admin.NotificationRepository();

                BusinessPOCO.Admin.Notification notf = new BusinessPOCO.Admin.Notification();
                notf.Description = Description;
                notf.Title = Title;
                notf.Icon = Icon;
                notf.ClickLink = ClickLink;
                notf.CreatedDatetime = DateTime.Now;
                notf.Isviewed = false;
                notf.EmployeeID = EmployeeID;
                notf.CreatedBy = CreatedBy;

                notfication.Insert(notf);

                notfication.Save();


                var notfreturn = new
                {
                    Description = notf.Description,
                    Title = notf.Title,
                    Icon = notf.Icon,
                    ClickLink = notf.ClickLink,
                    CreatedDatetime = notf.CreatedDatetime,
                    Isviewed = notf.Isviewed,
                    EmployeeID = notf.EmployeeID,
                    CreatedBy = notf.CreatedBy,
                    TimeAgo = HelpingClass.Timeago(notf.CreatedDatetime.Value, DateTime.Now)
                };

                string json = new JavaScriptSerializer().Serialize(new { ResponseType = decoResponseType, title = decoTitle, description = decoDescription, notification = notfreturn });
                RealTimeBroadcaster.BroadCast("s-" + EmployeeID.ToString(), json);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool FireOnClient(string Title, string Description, string ClickLink, int EmployeeID, int CreatedBy,DBInitializer db
            , string Icon = "i-Speach-Bubble-6 text-primary mr-1", string decoTitle = "", string decoDescription = "", string decoResponseType = "Simple")
        {
            try
            {
                Repositories.User.INotificationRepository notfication = db.NotificationRepository();

                BusinessPOCO.User.Cl_Notification notf = new BusinessPOCO.User.Cl_Notification();
                notf.Description = Description;
                notf.Title = Title;
                notf.Icon = Icon;
                notf.ClickLink = ClickLink;
                notf.CreatedDatetime = DateTime.Now;
                notf.Isviewed = false;
                notf.EmployeeID = EmployeeID;
                notf.CreatedBy = CreatedBy;

                notfication.Insert(notf);

                notfication.Save();


                var notfreturn = new
                {
                    Description = notf.Description,
                    Title = notf.Title,
                    Icon = notf.Icon,
                    ClickLink = notf.ClickLink,
                    CreatedDatetime = notf.CreatedDatetime,
                    Isviewed = notf.Isviewed,
                    EmployeeID = notf.EmployeeID,
                    CreatedBy = notf.CreatedBy,
                    TimeAgo = HelpingClass.Timeago(notf.CreatedDatetime.Value, DateTime.Now)
                };

                string json = new JavaScriptSerializer().Serialize(new { ResponseType = decoResponseType, title = decoTitle, description = decoDescription, notification = notfreturn });
                RealTimeBroadcaster.BroadCast("c-" + EmployeeID.ToString(), json);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}