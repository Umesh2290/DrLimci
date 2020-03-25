using LaboratorySystem.SessionRelatedClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaboratorySystem.Controllers.Main
{
    public class MainController : Controller
    {
        // GET: Main
        [NonSubDomain]
        public ActionResult Index()
        {
            if (!HelpingClass.ValidateConnection()) 
            {
              throw new Exception("LabSystemDB_Connection does not establish!");
            }
            return View("~/Views/Main/Index.cshtml");
        }
        [NonSubDomain]
        public ActionResult Services()
        {
            return View("~/Views/Main/Services.cshtml");
        }
        [NonSubDomain]
        public ActionResult Pricing()
        {
            return View("~/Views/Main/Pricing.cshtml");
        }
        [NonSubDomain]
        public ActionResult Demo()
        {
            return View("~/Views/Main/Demo.cshtml");
        }
        [NonSubDomain]
        public ActionResult ContactUs()
        {
            //if (!HelpingClass.ValidateConnection())
            //{
            //    throw new Exception("LabSystemDB_Connection does not establish!");
            //}
            return View("~/Views/Main/ContactUs.cshtml");
        }

        [HttpPost]
        [NonSubDomain]
        public JsonResult ContactUsNewRequest(String Name, String Email, String ContactNo, String Subject, String Description,int TypeID)
        {
            try
            {
                LaboratoryBusiness.Repositories.Admin.ISalesAndContactQueryRepository sacqp = new LaboratoryBusiness.BLL.Admin.SalesAndContactQueryRepository();
                LaboratoryBusiness.Repositories.Admin.IRoleRepository role = new LaboratoryBusiness.BLL.Admin.RoleRepository();
                LaboratoryBusiness.Repositories.Admin.IRoleMappingRepository rolemapping = new LaboratoryBusiness.BLL.Admin.RoleMappingRepository();
                LaboratoryBusiness.Repositories.Admin.ISystemUserRepository systemuser = new LaboratoryBusiness.BLL.Admin.SystemUserRepository();

                string link = string.Empty;
                string inquirytype = string.Empty;

                LaboratoryBusiness.POCO.Admin.SalesAndContactQuery sacq = new LaboratoryBusiness.POCO.Admin.SalesAndContactQuery();
                sacq.Email = Email;
                sacq.ContactNo = ContactNo;
                sacq.Name = Name;
                sacq.Subject = Subject;
                sacq.Description = Description;
                sacq.StatusID = 1;
                sacq.TypeID = TypeID;
                sacq.RequestCreatedDate = DateTime.Now;

                sacqp.Insert(sacq);
                sacqp.Save();

                if (TypeID == 1)
                {
                    link = "/Admin/SalesAndContactRequest/NewSales";
                    inquirytype = "Sales Inquiry";
                }
                else
                {
                    link = "/Admin/SalesAndContactRequest/NewContact";
                    inquirytype = "Contact Inquiry";
                }

                var superadmins = (from su in systemuser.GetAll()
                                       join rlm in rolemapping.GetAll() on su.SystemUserID equals rlm.UserID
                                       join rl in role.GetAll() on rlm.RoleID.Value equals rl.RoleID
                                   where rl.RoleName.Equals("Superadmin") || rl.RoleName.Equals("Admin")
                                   select su).Distinct().ToList();

                foreach (var sad in superadmins)
                {
                    NotificationManager.Fire("Inquiry Request", "Click here to view request", link, sad.SystemUserID, -1,
                        "i-Speach-Bubble-6 text-primary mr-1", "New Inquiry", inquirytype, "toastr-sucsess");
                }

                

                return WebJSResponse.ResponseSWAL(SwalEnum.success, "Inquiry Submitted !", "Inquiry has been submitted successfully<br>", new { });

            }
            catch (Exception ex)
            {

                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }

        }


    }
}