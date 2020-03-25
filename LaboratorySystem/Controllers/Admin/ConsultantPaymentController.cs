using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repositories = LaboratoryBusiness.Repositories;
using BLL = LaboratoryBusiness.BLL;
using BusinessPOCO = LaboratoryBusiness.POCO;

namespace LaboratorySystem.Controllers.Admin
{
    [RoutePrefix("Admin/ConsultantPayment")]
    public class ConsultantPaymentController : Controller
    {
        
        [Route("MyPayments")]
        [SystemAuthorizeMember]
        public ActionResult MyPayments()
        {
            return View("~/Views/Admin/ConsultantPayment/MyPayments.cshtml");
        }

        [HttpPost]
        [Route("GetList")]
        public JsonResult GetList()
        {
            try
            {
                Repositories.Admin.IOpinionRequestRepository opinionrequestrepo = new BLL.Admin.OpinionRequestRepository();
                Repositories.Admin.IOpinionRequestStatusRepository opinionrequeststatusrepo = new BLL.Admin.OpinionRequestStatusRepository();
                Repositories.Admin.ISystemUserRepository systemuserrepo = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.IClientRepository clientrepo = new BLL.Admin.ClientRepository();

                object returnList = new { };
               

                    returnList = (from or in opinionrequestrepo.GetAll()
                                  join ors in opinionrequeststatusrepo.GetAll() on or.StatusID equals ors.OpinionRequestStatusID
                                  join su in systemuserrepo.GetAll() on or.ClientID equals su.SystemUserID
                                  join cl in clientrepo.GetAll() on su.DetailID equals cl.ClientDetailID
                                  where (ors.Name.Equals("Completed")) && (or.AssignedTo.HasValue ? or.AssignedTo.Value : 0)==MySession.SystemSession.SystemUserID
                                  select new
                                  {
                                      OpinionRequestID = or.OpinionRequestID.ToString() + " - " + or.ClientOpinionRequestID.Value.ToString(),
                                      ClientName = cl.CompanyName,
                                      or.PatientDetails,
                                      StatusCustom = ors.Name,
                                      IsPublish = or.IsPublish.HasValue ? (or.IsPublish.Value ? "Yes" : "No") : "No",
                                      OpinionRequestIDOrg = or.OpinionRequestID,
                                      Payment=or.Payment.HasValue?or.Payment.Value.ToString():"0.00",
                                      or.PaymentReceiptPdfLink,
                                      CompletionDate = or.PendingActionDate.HasValue?or.PendingActionDate.Value.ToString("MM/dd/yyyy HH:mm tt"):or.NewActionDate.Value.ToString("MM/dd/yyyy HH:mm tt")
                                  }).ToList();


                    return WebJSResponse.ResponseSimple(new { consultantpaymentjson = returnList });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }
    }
}