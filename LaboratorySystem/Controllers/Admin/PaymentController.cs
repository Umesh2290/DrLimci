using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repositories = LaboratoryBusiness.Repositories;
using BLL = LaboratoryBusiness.BLL;
using BusinessPOCO = LaboratoryBusiness.POCO;
using System.Text;

namespace LaboratorySystem.Controllers.Admin
{
    [RoutePrefix("Admin/Payment")]
    public class PaymentController : RedirectController
    {

        [Route("GetClientLicensePayments")]
        [HttpPost]
        public JsonResult GetClientLicensePayments()
        {
            try
            {
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();
            Repositories.Admin.ISystemUserTypeRepository systemsusertype = new BLL.Admin.SystemUserTypeRepository();
            Repositories.Admin.IClientRepository client = new BLL.Admin.ClientRepository();

            int clientdetailtypeid = systemsusertype.GetAll().Where(x => x.TypeName.Equals("Client")).FirstOrDefault().SystemUserTypeID;

            var clientlist = (from c in client.GetAll()
                             join su in systemuser.GetAll() on c.ClientDetailID equals su.DetailID
                             where su.DetailType.Value==clientdetailtypeid && c.InvoicePDFLink!=null
                             select new {c.ClientDetailID,ClientDetailIDEncrypted=Convert.ToBase64String(Encoding.UTF8.GetBytes(c.ClientDetailID.ToString()))
                             ,
                                         c.CompanyName,
                                         c.CreatedBy,
                                         c.CreatedDate,
                                         CreatedDateFormatted=c.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                                         c.InvoiceID,
                                         c.InvoicePDFLink,
                                         status = su.IsActive.HasValue ? (su.IsActive.Value == true ? "Active" : "Inactive") : "Inactive",
                                         c.TotalLicenseCost.Value,
                                         FormattedCost = c.PriceUnit + " " + c.TotalLicenseCost.Value.ToString()
                             ,su.SystemUserID}).ToList();

                return WebJSResponse.ResponseSimple(new { clientpaymentjson = clientlist });
            }
            catch(Exception ex) 
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            } 
        }

        [Route("DownloadMultipleInvoice")]
        [HttpGet]

        public ActionResult DownloadMultipleInvoice(string qr)
        {
            try
            {
                Repositories.Admin.IClientRepository client = new BLL.Admin.ClientRepository();
                List<FileManager> invoicepdfs = new List<FileManager>();

                string[] stringids = qr.Split(',');
                int clientid = 0;

                BusinessPOCO.Admin.Client clientobj;
                foreach (string id in stringids)
                {
                    clientid = Convert.ToInt32(Encoding.UTF8.GetString(Convert.FromBase64String(id)));
                    clientobj = client.GetByID(clientid);
                    invoicepdfs.Add(new FileManager(clientobj.InvoicePDFLink, "Invoice_" + clientobj.CompanyName.Trim().Replace(" ","")));

                }

                FileManager zipfile = ZipManager.ListOfFilesToZipFile("MultipleInvoices", invoicepdfs);

                return File(zipfile.FileBytes, "application/zip", zipfile.Name + zipfile.Extension);
            }
            catch (Exception ex)
            {
                return HttpNotFound("Somthing went wrong while trying to download" + ex.Message);
            }

        }

        [Route("GetClientDetail")]
        [HttpPost]
        public JsonResult GetClientDetail(int clientdetailid)
        {
            try
            {
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.ISystemUserTypeRepository systemsusertype = new BLL.Admin.SystemUserTypeRepository();
                Repositories.Admin.IClientParameterRepository clientparameter = new BLL.Admin.ClientParameterRepository();
                Repositories.Admin.IProviderParamterRepository providerparameter = new BLL.Admin.ProviderParamterRepository();
                Repositories.Admin.IClientRepository client = new BLL.Admin.ClientRepository();
                Repositories.Admin.IPlanRepository plan = new BLL.Admin.PlanRepository();
                Repositories.Admin.IStorageandDBProviderRepository standdbprovider = new BLL.Admin.StorageandDBProviderRepository();

                int clientdetailtypeid = systemsusertype.GetAll().Where(x => x.TypeName.Equals("Client")).FirstOrDefault().SystemUserTypeID;

                var clientobj = client.GetByID(clientdetailid);
                var systemuserobj = systemuser.GetAll().Where(x => x.DetailType.Value == clientdetailtypeid && x.DetailID == clientdetailid).FirstOrDefault();
                var clientparameters = (from cp in clientparameter.GetAll()
                                        join pp in providerparameter.GetAll() on cp.ParameterID equals pp.ParameterID
                                        where cp.ClientDetailID == clientdetailid
                                        select new { pp.ParameterID, pp.ParameterName, cp.ParameterValue }).ToList();
                var customformatted = new { CreatedDateFormatted = clientobj.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                                            FormattedCost = clientobj.PriceUnit + " " + clientobj.TotalLicenseCost.Value.ToString(),
                                            status = systemuserobj.IsActive.HasValue ? (systemuserobj.IsActive.Value == true ? "Active" : "Inactive") : "Inactive",
                };

                var planobj = plan.GetByID(clientobj.PlanID.Value);
                var standdbproviderobj = standdbprovider.GetByID(clientobj.STandDBprovdiderID.Value);

                return WebJSResponse.ResponseSimple(new { clientobjjson = clientobj,
                                                          systemuserobjjson = systemuserobj,
                                                          clientparametersjson = clientparameters,
                                                          customformattedjson = customformatted,
                                                          planobjjson = planobj,
                                                          standdbproviderobjjson = standdbproviderobj
                });
            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }



        [HttpPost]
        [Route("GetConsultantDoctorPayments")]
        public JsonResult GetConsultantDoctorPayments()
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
                                  join suc in systemuserrepo.GetAll() on or.AssignedTo equals suc.SystemUserID
                                  where (ors.Name.Equals("Completed")) && or.AssignedTo.HasValue
                                  select new
                                  {
                                      OpinionRequestID = or.OpinionRequestID.ToString() + " - " + or.ClientOpinionRequestID.Value.ToString(),
                                      CompanyName = cl.CompanyName,
                                      or.PatientDetails,
                                      StatusCustom = ors.Name,
                                      IsPublish = or.IsPublish.HasValue ? (or.IsPublish.Value ? "Yes" : "No") : "No",
                                      OpinionRequestIDOrg = or.OpinionRequestID,
                                      Consultant = suc.FirstName+" "+(suc.MiddleName!=null?suc.MiddleName:"")+" "+suc.LastName,
                                      or.PaymentReceiptPdfLink,
                                      Amount=or.Payment.HasValue?or.Payment.Value.ToString():"0.00"

                                  }).Where(x=>x.IsPublish.Equals("Yes")).ToList();


                    return WebJSResponse.ResponseSimple(new { consultantpaymentjson = returnList });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [HttpPost]
        [Route("GetConsultantOpinionDetail")]
        public JsonResult GetConsultantOpinionDetail(int opinionrequestid)
        {
            try
            {
                Repositories.Admin.IOpinionRequestRepository opinionrequestrepo = new BLL.Admin.OpinionRequestRepository();
                Repositories.Admin.IOpinionRequestStatusRepository opinionrequeststatusrepo = new BLL.Admin.OpinionRequestStatusRepository();
                Repositories.Admin.ISystemUserRepository systemuserrepo = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.IClientRepository clientrepo = new BLL.Admin.ClientRepository();

                object returnlist = null;
                object openAction = null;
                object progressAction = null;
                object assignedInfo = null;

                var data = opinionrequestrepo.GetByID(opinionrequestid);

                if (data != null)
                {
                    BusinessPOCO.Admin.SystemUser su = systemuserrepo.GetByID(data.ClientID.Value);
                    Client cl = new Client(su.DetailID.Value);
                    DBInitializer db = new DBInitializer(cl);
                    Repositories.User.IClientUserRepository curepo = db.ClientUserRepository();
                    var cu = curepo.GetByID((data.RequestCreatedBy.Value * -1));

                    returnlist = new
                    {
                        OpinionRequestIDCustom = data.OpinionRequestID.ToString() + " - " + data.ClientOpinionRequestID.Value.ToString(),
                        data.OpinionRequestID,
                        PatientDetails = data.PatientDetails,
                        SampleAnalysisDetails = data.SampleAnalysisDetails,
                        OpinionNeededDescription = data.OpinionNeededDescription,
                        data.ClientOpinionRequestID,
                        data.ClientID,
                        ClientName = cl.CompanyName,
                        StatusCustom = opinionrequeststatusrepo.GetByID(data.StatusID.Value).Name,
                        data.StatusID,
                        data.CommentForRequester,
                        data.CommentForConsultant,
                        data.Payment,
                        data.PaymentReceiptPdfLink,
                        RequestCreatedByName = cu.FirstName + " " + cu.LastName,
                        RequestCreatedDateCustom = data.RequestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                        IsPublish = data.IsPublish.HasValue ? data.IsPublish.Value ? "Yes" : "No" : "No"

                    };

                    if (data.NewActionBy.HasValue)
                    {
                        su = systemuserrepo.GetByID(data.NewActionBy.Value);
                        openAction = new
                        {
                            OpenActionByName = su.FirstName + " " + su.LastName,
                            OpenActionDateCustom = data.NewActionDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                            OpenActionComments = data.NewActionComments,
                            OpenActionStatusName = opinionrequeststatusrepo.GetByID(data.NewActionStatusID.Value).Name
                        };
                    }

                    if (data.PendingActionBy.HasValue)
                    {
                        su = systemuserrepo.GetByID(data.PendingActionBy.Value);
                        progressAction = new
                        {
                            ProgressActionByName = su.FirstName + " " + su.LastName,
                            ProgressActionDateCustom = data.PendingActionDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                            ProgressActionComments = data.PendingActionComments,
                            ProgressActionStatusName = opinionrequeststatusrepo.GetByID(data.StatusID.Value).Name
                        };
                    }

                    if (data.AssignedTo.HasValue)
                    {
                        su = systemuserrepo.GetByID(data.AssignedTo.Value);
                        assignedInfo = new
                        {
                            data.AssignedTo,
                            AssignedToName = su.FirstName + " " + su.LastName,
                            data.ConsultantOpinion

                        };

                    }
                }



                return WebJSResponse.ResponseSimple(new { opinionrequestobjjson = returnlist, openactionjson = openAction, progressactionjson = progressAction, assignedInfojson = assignedInfo });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("DownloadMultipleInvoiceConsultant")]
        [HttpGet]

        public ActionResult DownloadMultipleInvoiceConsultant(string qr)
        {
            try
            {
                Repositories.Admin.IOpinionRequestRepository opinionrequestrepo = new BLL.Admin.OpinionRequestRepository();
                List<FileManager> paymentpdfs = new List<FileManager>();

                string[] stringids = qr.Split(',');
                int requestid = 0;

                BusinessPOCO.Admin.OpinionRequest opinionrequestobj;
                foreach (string id in stringids)
                {
                    requestid = Convert.ToInt32(id);
                    opinionrequestobj = opinionrequestrepo.GetByID(requestid);
                    paymentpdfs.Add(new FileManager(opinionrequestobj.PaymentReceiptPdfLink, "ConsultantReceipt_" + requestid.ToString() + "_" + (opinionrequestobj.AssignedTo.HasValue ? opinionrequestobj.AssignedTo.Value.ToString():"0")));

                }

                FileManager zipfile = ZipManager.ListOfFilesToZipFile("ConsultantReceiptPdfs", paymentpdfs);

                return File(zipfile.FileBytes, "application/zip", zipfile.Name + zipfile.Extension);
            }
            catch (Exception ex)
            {
                return HttpNotFound("Somthing went wrong while trying to download" + ex.Message);
            }

        }

        // GET: Payment
        [Route("ClientLicensePayments")]
        [SystemAuthorizeMember]
        public ActionResult ClientLicensePayments()
        {

            return View("~/Views/Admin/Payment/ClientLicensePayments.cshtml");
        }

        [Route("ConsultantDoctorPayments")]
        [SystemAuthorizeMember]
        public ActionResult ConsultantDoctorPayments()
        {
            return View("~/Views/Admin/Payment/ConsultantDoctorPayments.cshtml");
        } 
    }
}