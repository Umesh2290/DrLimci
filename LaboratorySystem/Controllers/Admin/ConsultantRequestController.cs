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
    [RoutePrefix("Admin/ConsultantRequest")]
    public class ConsultantRequestController : Controller
    {
        [HttpPost]
        [Route("GetList")]
        public JsonResult GetList(string status)
        {
            try
            {
                Repositories.Admin.IOpinionRequestRepository opinionrequestrepo = new BLL.Admin.OpinionRequestRepository();
                Repositories.Admin.IOpinionRequestStatusRepository opinionrequeststatusrepo = new BLL.Admin.OpinionRequestStatusRepository();
                Repositories.Admin.ISystemUserRepository systemuserrepo = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.IClientRepository clientrepo = new BLL.Admin.ClientRepository();

                object returnList = new { };
                if (status.Equals("New"))
                {
                    int statusid = opinionrequeststatusrepo.GetAll().Where(x => x.Name.Equals("New")).FirstOrDefault().OpinionRequestStatusID;
                    returnList = (from or in opinionrequestrepo.GetAll()
                                  join ors in opinionrequeststatusrepo.GetAll() on or.StatusID equals ors.OpinionRequestStatusID
                                  join su in systemuserrepo.GetAll() on or.ClientID equals su.SystemUserID
                                  join cl in clientrepo.GetAll() on su.DetailID equals cl.ClientDetailID
                                  where or.StatusID.Value == statusid && (or.AssignedTo.HasValue ? or.AssignedTo.Value : 0)==MySession.SystemSession.SystemUserID
                                  select new
                                  {
                                      OpinionRequestID = or.OpinionRequestID.ToString() + " - " + or.ClientOpinionRequestID.Value.ToString(),
                                      ClientName = cl.CompanyName,
                                      or.PatientDetails,
                                      StatusCustom = "New",
                                      IsPublish = or.IsPublish.HasValue?(or.IsPublish.Value?"Yes":"No"):"No",
                                      OpinionRequestIDOrg = or.OpinionRequestID
                                  }).ToList();
                }

                else if (status.Equals("Pending"))
                {
                    returnList = (from or in opinionrequestrepo.GetAll()
                                  join ors in opinionrequeststatusrepo.GetAll() on or.StatusID equals ors.OpinionRequestStatusID
                                  join su in systemuserrepo.GetAll() on or.ClientID equals su.SystemUserID
                                  join cl in clientrepo.GetAll() on su.DetailID equals cl.ClientDetailID
                                  where (((ors.Name.Equals("In-Progress")) || (ors.Name.Equals("Completed") && or.IsPublish.HasValue ? !or.IsPublish.Value : true)) && (or.AssignedTo.HasValue ? or.AssignedTo.Value : 0) == MySession.SystemSession.SystemUserID)
                                  select new
                                  {
                                      OpinionRequestID = or.OpinionRequestID.ToString() + " - " + or.ClientOpinionRequestID.Value.ToString(),
                                      ClientName = cl.CompanyName,
                                      or.PatientDetails,
                                      StatusCustom = ors.Name,
                                      IsPublish = or.IsPublish.HasValue?(or.IsPublish.Value?"Yes":"No"):"No",
                                      OpinionRequestIDOrg = or.OpinionRequestID
                                  }).Where(x => x.StatusCustom.Equals("In-Progress") || x.StatusCustom.Equals("Completed")).ToList();
                }

                else if (status.Equals("Completed"))
                {
                    returnList = (from or in opinionrequestrepo.GetAll()
                                  join ors in opinionrequeststatusrepo.GetAll() on or.StatusID equals ors.OpinionRequestStatusID
                                  join su in systemuserrepo.GetAll() on or.ClientID equals su.SystemUserID
                                  join cl in clientrepo.GetAll() on su.DetailID equals cl.ClientDetailID
                                  where (ors.Name.Equals("Completed")) && (or.AssignedTo.HasValue ? or.AssignedTo.Value : 0) == MySession.SystemSession.SystemUserID
                                  select new
                                  {
                                      OpinionRequestID = or.OpinionRequestID.ToString() + " - " + or.ClientOpinionRequestID.Value.ToString(),
                                      ClientName = cl.CompanyName,
                                      or.PatientDetails,
                                      StatusCustom = ors.Name,
                                      IsPublish = or.IsPublish.HasValue ? (or.IsPublish.Value ? "Yes" : "No") : "No",
                                      OpinionRequestIDOrg = or.OpinionRequestID
                                  }).ToList();
                }





                return WebJSResponse.ResponseSimple(new { opinionrequestjson = returnList });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [HttpPost]
        [Route("GetDetail")]
        public JsonResult GetDetail(int opinionrequestid)
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

        [HttpPost]
        [Route("UpdateStatus")]
        public JsonResult UpdateStatus(int opinionrequestid, string comments, int statusid,string payment,string opinion,bool ispublish)
        {
            try
            {
                Repositories.Admin.IOpinionRequestRepository opinionrequestrepo = new BLL.Admin.OpinionRequestRepository();
                Repositories.Admin.IOpinionRequestStatusRepository opinionrequeststatusrepo = new BLL.Admin.OpinionRequestStatusRepository();
                Repositories.Admin.ISystemUserRepository systemuserrepo = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.IClientRepository clientrepo = new BLL.Admin.ClientRepository();

                var opinionrequestobj = opinionrequestrepo.GetByID(opinionrequestid);

                BusinessPOCO.Admin.SystemUser su = systemuserrepo.GetByID(opinionrequestobj.ClientID.Value);
                Client cl = new Client(su.DetailID.Value);
                DBInitializer db = new DBInitializer(cl);

                string statusname = opinionrequeststatusrepo.GetByID(statusid).Name;

                if (statusname.Equals("Completed"))
                {
                    if (opinionrequestobj.NewActionBy.HasValue)
                    {

                        opinionrequestobj.StatusID = statusid;
                        opinionrequestobj.PendingActionBy = MySession.SystemSession.SystemUserID;
                        opinionrequestobj.PendingActionDate = DateTime.Now;
                        opinionrequestobj.PendingActionComments = comments;
                        opinionrequestobj.ConsultantOpinion = opinion;
                        opinionrequestobj.Payment = Convert.ToDecimal(payment);
                        opinionrequestobj.IsPublish = ispublish;

                        opinionrequestrepo.Update(opinionrequestobj);
                        opinionrequestrepo.Save();

                        if (ispublish)
                        {
                            Repositories.User.IOpinionRequestRepository clopinionrerepo = db.OpinionRequestRepository();
                            var clopinion = clopinionrerepo.GetByID(opinionrequestobj.ClientOpinionRequestID.Value);
                            clopinion.PendingActionBy = MySession.SystemSession.SystemUserID * -1;
                            clopinion.PendingActionDate = DateTime.Now;
                            clopinion.PendingActionComments = comments;
                            clopinion.StatusID = statusid;
                            clopinion.ConsultationOpinion = opinion;
                            clopinion.OpinionBy = MySession.SystemSession.FirstName + " " + MySession.SystemSession.LastName;
                            clopinion.IsPublish = true;

                            clopinionrerepo.Update(clopinion);
                            clopinionrerepo.Save();

                            Repositories.User.IClientUserRepository curepo = db.ClientUserRepository();
                            Repositories.User.IRoleMappingRepository rolemapping = db.RoleMappingRepository();
                            Repositories.User.IRoleRepository role = db.RoleRepository();

                            var _labcons_users = (from rlm in rolemapping.GetAll()
                                                  join rl in role.GetAll() on rlm.RoleID equals rl.RoleID
                                                  where rl.RoleName.Equals("Clinical Laboratory Consultant")
                                                  select rlm).ToList();

                            foreach (var labcons in _labcons_users)
                            {
                                NotificationManager.FireOnClient(("Request ID #" + opinionrequestobj.OpinionRequestID.ToString("000")), "Opinion Request Completed", "/User/OpinionRequest/Completed",
                                    labcons.UserID.Value, MySession.SystemSession.SystemUserID * -1, db,
                                   "i-Speach-Bubble-6 text-primary mr-1", "Opinion Request Completed", ("Request ID #" + opinionrequestobj.OpinionRequestID.ToString("000")), "toastr-sucsess");
                            }


                        }
                    }
                    else
                    {
                        opinionrequestobj.StatusID = statusid;
                        opinionrequestobj.NewActionBy = MySession.SystemSession.SystemUserID;
                        opinionrequestobj.NewActionStatusID = statusid;
                        opinionrequestobj.NewActionDate = DateTime.Now;
                        opinionrequestobj.NewActionComments = comments;
                        opinionrequestobj.ConsultantOpinion = opinion;
                        opinionrequestobj.Payment = Convert.ToDecimal(payment);
                        opinionrequestobj.IsPublish = ispublish;

                        opinionrequestrepo.Update(opinionrequestobj);
                        opinionrequestrepo.Save();

                        if (ispublish)
                        {
                            Repositories.User.IOpinionRequestRepository clopinionrerepo = db.OpinionRequestRepository();
                            var clopinion = clopinionrerepo.GetByID(opinionrequestobj.ClientOpinionRequestID.Value);
                            clopinion.NewActionBy = MySession.SystemSession.SystemUserID * -1;
                            clopinion.NewActionDate = DateTime.Now;
                            clopinion.NewActionComments = comments;
                            clopinion.NewActionStatusID = statusid;
                            clopinion.StatusID = statusid;
                            clopinion.ConsultationOpinion = opinion;
                            clopinion.OpinionBy = MySession.SystemSession.FirstName + " " + MySession.SystemSession.LastName;
                            clopinion.IsPublish = true;

                            clopinionrerepo.Update(clopinion);
                            clopinionrerepo.Save();

                            Repositories.User.IClientUserRepository curepo = db.ClientUserRepository();
                            Repositories.User.IRoleMappingRepository rolemapping = db.RoleMappingRepository();
                            Repositories.User.IRoleRepository role = db.RoleRepository();

                            var _labcons_users = (from rlm in rolemapping.GetAll()
                                                  join rl in role.GetAll() on rlm.RoleID equals rl.RoleID
                                                  where rl.RoleName.Equals("Clinical Laboratory Consultant")
                                                  select rlm).ToList();

                            foreach (var labcons in _labcons_users)
                            {
                                NotificationManager.FireOnClient(("Request ID #" + opinionrequestobj.OpinionRequestID.ToString("000")), "Opinion Request Completed", "/User/OpinionRequest/Completed",
                                    labcons.UserID.Value, MySession.SystemSession.SystemUserID * -1, db,
                                   "i-Speach-Bubble-6 text-primary mr-1", "Opinion Request Completed", ("Request ID #" + opinionrequestobj.OpinionRequestID.ToString("000")), "toastr-sucsess");
                            }
                        }
                    }
                    

                    


                }
                else
                {
                    opinionrequestobj.StatusID = statusid;
                    opinionrequestobj.NewActionBy = MySession.SystemSession.SystemUserID;
                    opinionrequestobj.NewActionStatusID = statusid;
                    opinionrequestobj.NewActionDate = DateTime.Now;
                    opinionrequestobj.NewActionComments = comments;

                    opinionrequestrepo.Update(opinionrequestobj);
                    opinionrequestrepo.Save();

                    Repositories.User.IOpinionRequestRepository clopinionrerepo = db.OpinionRequestRepository();
                    var clopinion = clopinionrerepo.GetByID(opinionrequestobj.ClientOpinionRequestID.Value);
                    clopinion.NewActionBy = MySession.SystemSession.SystemUserID * -1;
                    clopinion.NewActionDate = DateTime.Now;
                    clopinion.NewActionComments = comments;
                    clopinion.NewActionStatusID = statusid;
                    clopinion.StatusID = statusid;

                    clopinionrerepo.Update(clopinion);
                    clopinionrerepo.Save();
                }

                bool isreceiptcreated = false;
                if (ispublish && statusname.Equals("Completed") && opinionrequestobj!=null)
                {
                    try
                    {

                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("@bootstrapcsslink@", HelpingClass.GetDefaultDirectory() + "\\Content\\EmailTemplates\\Content\\bootstrap.min.css");
                        dic.Add("@jqueryjslink@", HelpingClass.GetDefaultDirectory() + "\\Content\\EmailTemplates\\Content\\jquery.min.js");
                        dic.Add("@bootstrapjslink@", HelpingClass.GetDefaultDirectory() + "\\Content\\EmailTemplates\\Content\\bootstrap.min.js");

                        dic.Add("@opinionrequestid@", opinionrequestobj.OpinionRequestID.ToString());
                        dic.Add("@date@", opinionrequestobj.PendingActionDate.HasValue ? opinionrequestobj.PendingActionDate.Value.ToString("MM/dd/yyyy HH:mm tt") : opinionrequestobj.NewActionDate.Value.ToString("MM/dd/yyyy HH:mm tt"));
                        dic.Add("@amount@", opinionrequestobj.Payment.HasValue ? opinionrequestobj.Payment.Value.ToString() : "0.00");

                        dic.Add("@consultantid@", MySession.SystemSession.SystemUserID.ToString("000"));
                        dic.Add("@name@", MySession.SystemSession.FirstName + " " + (MySession.SystemSession.MiddleName != null ? MySession.SystemSession.MiddleName : "")+" "+MySession.SystemSession.LastName);
                        dic.Add("@address@", MySession.SystemSession.Address != null ? MySession.SystemSession.Address : "");
                        dic.Add("@mobileno@", MySession.SystemSession.MobileNo != null ? MySession.SystemSession.MobileNo : "");

                        string pdfhtmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\ConsultantReceipt.html", dic);

                        FileManager pd = PdfManager.HtmlToPdf("ConsultantReceipt_"+opinionrequestobj.OpinionRequestID.ToString()+"_"+MySession.SystemSession.SystemUserID.ToString(), pdfhtmltemplate);
                        string pdffilelocalsavedpath;
                        FileManager.ByteArrayToFile(pd, HelpingClass.GetDefaultDirectory() + "\\ClientsData\\" + cl.CompanyName.Trim().Replace(" ", "")+"\\ConsultantReceiptFiles", out pdffilelocalsavedpath);



                        opinionrequestobj.PaymentReceiptPdfLink = HelpingClass.LocalUploadPathToRelativeWebPath(pdffilelocalsavedpath);

                        opinionrequestrepo.Update(opinionrequestobj);
                        opinionrequestrepo.Save();

                        isreceiptcreated = true;
                    }
                    catch (Exception ex)
                    {
                        isreceiptcreated = false;
                    }
                }
                else
                {
                    isreceiptcreated = true;
                }

                if (isreceiptcreated)
                {
                    return WebJSResponse.ResponseSWAL(SwalEnum.success, "Saved Successfully !", "Request has been saved successfully<br>", new { opinionrequestobjjson = opinionrequestobj });
                }
                else
                {
                    return WebJSResponse.ResponseSWAL(SwalEnum.warning, "Saved Successfully !", "Request has been saved successfully<br>Something went wrong while created pdf receipt !", new { opinionrequestobjjson = opinionrequestobj });
                }




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("NewRequest")]
        [SystemAuthorizeMember]
        public ActionResult NewRequest()
        {
            return View("~/Views/Admin/ConsultantRequest/NewRequest.cshtml");
        }

        [Route("PendingRequest")]
        [SystemAuthorizeMember]
        public ActionResult PendingRequest()
        {
            return View("~/Views/Admin/ConsultantRequest/PendingRequest.cshtml");
        }

        [Route("CompletedRequest")]
        [SystemAuthorizeMember]
        public ActionResult CompletedRequest()
        {
            return View("~/Views/Admin/ConsultantRequest/CompletedRequest.cshtml");
        }
    }
}