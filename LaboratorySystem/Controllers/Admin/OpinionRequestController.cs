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
    [RoutePrefix("Admin/OpinionRequest")]
    public class OpinionRequestController : Controller
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
                                  where or.StatusID.Value == statusid && !or.AssignedTo.HasValue
                                  select new { OpinionRequestID=or.OpinionRequestID.ToString()+" - "+or.ClientOpinionRequestID.Value.ToString(),
                                               ClientName = cl.CompanyName,
                                               or.PatientDetails,
                                               StatusCustom = "New",
                                               IsPublish = or.IsPublish.HasValue ? (or.IsPublish.Value ? "Yes" : "No") : "No",
                                               OpinionRequestIDOrg = or.OpinionRequestID
                                  }).ToList();
                }

                else if (status.Equals("Assigned"))
                {
                    returnList = (from or in opinionrequestrepo.GetAll()
                                  join ors in opinionrequeststatusrepo.GetAll() on or.StatusID equals ors.OpinionRequestStatusID
                                  join su in systemuserrepo.GetAll() on or.ClientID equals su.SystemUserID
                                  join cl in clientrepo.GetAll() on su.DetailID equals cl.ClientDetailID
                                  where (ors.Name.Equals("New") || ors.Name.Equals("In-Progress")) && or.AssignedTo.HasValue
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

                else if (status.Equals("Completed"))
                {
                    returnList = (from or in opinionrequestrepo.GetAll()
                                  join ors in opinionrequeststatusrepo.GetAll() on or.StatusID equals ors.OpinionRequestStatusID
                                  join su in systemuserrepo.GetAll() on or.ClientID equals su.SystemUserID
                                  join cl in clientrepo.GetAll() on su.DetailID equals cl.ClientDetailID
                                  where (ors.Name.Equals("Completed")) && or.AssignedTo.HasValue
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

                else if (status.Equals("Cancelled"))
                {
                    returnList = (from or in opinionrequestrepo.GetAll()
                                  join ors in opinionrequeststatusrepo.GetAll() on or.StatusID equals ors.OpinionRequestStatusID
                                  join su in systemuserrepo.GetAll() on or.ClientID equals su.SystemUserID
                                  join cl in clientrepo.GetAll() on su.DetailID equals cl.ClientDetailID
                                  where (ors.Name.Equals("Cancelled")) && !or.AssignedTo.HasValue
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
                        RequestCreatedByName = cu.FirstName+" "+cu.LastName,
                        RequestCreatedDateCustom = data.RequestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                        IsPublish = data.IsPublish.HasValue ? data.IsPublish.Value?"Yes":"No" : "No"

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
                            AssignedToName = su.FirstName+" "+su.LastName,
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
        public JsonResult UpdateStatus(int opinionrequestid, string comments, int statusid,string commentforrequester,string commentforconsultant,int assignedtoconsultant) 
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

                if (statusid != -100)
                {
                    opinionrequestobj.StatusID = statusid;
                    opinionrequestobj.NewActionBy = MySession.SystemSession.SystemUserID;
                    opinionrequestobj.NewActionStatusID = statusid;
                    opinionrequestobj.NewActionDate = DateTime.Now;
                    opinionrequestobj.NewActionComments = comments;
                    opinionrequestobj.CommentForRequester = commentforrequester.Trim().Equals("") ? null : commentforrequester.Trim();

                    Repositories.User.IOpinionRequestRepository clopinionrerepo = db.OpinionRequestRepository();
                    var clopinion = clopinionrerepo.GetByID(opinionrequestobj.ClientOpinionRequestID.Value);
                    clopinion.NewActionBy = MySession.SystemSession.SystemUserID*-1;
                    clopinion.NewActionStatusID = statusid;
                    clopinion.NewActionDate = DateTime.Now;
                    clopinion.NewActionComments = comments;
                    clopinion.StatusID = statusid;
                    clopinion.CommentForRequester = commentforrequester.Trim().Equals("") ? null : commentforrequester.Trim();

                    clopinionrerepo.Update(clopinion);
                    clopinionrerepo.Save();


                }
                else
                {
                    opinionrequestobj.AssignedTo = assignedtoconsultant;
                    opinionrequestobj.CommentForRequester = commentforrequester.Trim().Equals("") ? null : commentforrequester.Trim();
                    opinionrequestobj.CommentForConsultant = commentforconsultant.Trim().Equals("") ? null : commentforconsultant.Trim();
                }

                opinionrequestrepo.Update(opinionrequestobj);
                opinionrequestrepo.Save();


                if (statusid != -100)
                {
                    
                    Repositories.User.IClientUserRepository curepo = db.ClientUserRepository();
                    Repositories.User.IRoleMappingRepository rolemapping = db.RoleMappingRepository();
                    Repositories.User.IRoleRepository role = db.RoleRepository();
                    var cu = curepo.GetByID((opinionrequestobj.RequestCreatedBy.Value * -1));

                    var _labcons_users = (from rlm in rolemapping.GetAll()
                                          join rl in role.GetAll() on rlm.RoleID equals rl.RoleID
                                          where rl.RoleName.Equals("Clinical Laboratory Consultant")
                                          select rlm).ToList();

                    foreach (var labcons in _labcons_users)
                    {
                        NotificationManager.FireOnClient(("Request ID #" + opinionrequestobj.OpinionRequestID.ToString("000")), "Opinion Request Cancelled", "/User/OpinionRequest/Completed",
                            labcons.UserID.Value, MySession.SystemSession.SystemUserID * -1, db,
                           "i-Speach-Bubble-6 text-primary mr-1", "Opinion Request Cancelled", ("Request ID #" + opinionrequestobj.OpinionRequestID.ToString("000")), "toastr-error");

                        if (!commentforrequester.Trim().Equals(""))
                        {
                            NotificationManager.FireOnClient(("Request ID #" + opinionrequestobj.OpinionRequestID.ToString("000")), "Opinion Request Comment From System Admin", "/User/OpinionRequest/Completed",
                            labcons.UserID.Value, MySession.SystemSession.SystemUserID * -1, db,
                           "i-Speach-Bubble-6 text-primary mr-1", "Opinion Request Comment From System Admin", ("Request ID #" + opinionrequestobj.OpinionRequestID.ToString("000")), "toastr-sucsess");
                        }
                    }

                    
                }

                else
                {
                    if (!commentforrequester.Trim().Equals(""))
                    {
                        Repositories.User.IClientUserRepository curepo = db.ClientUserRepository();
                        Repositories.User.IRoleMappingRepository rolemapping = db.RoleMappingRepository();
                        Repositories.User.IRoleRepository role = db.RoleRepository();
                        var cu = curepo.GetByID((opinionrequestobj.RequestCreatedBy.Value * -1));

                        var _labcons_users = (from rlm in rolemapping.GetAll()
                                              join rl in role.GetAll() on rlm.RoleID equals rl.RoleID
                                              where rl.RoleName.Equals("Clinical Laboratory Consultant")
                                              select rlm).ToList();

                        foreach (var labcons in _labcons_users)
                        {

                            NotificationManager.FireOnClient(("Request ID #" + opinionrequestobj.OpinionRequestID.ToString("000")), "Opinion Request Comment From System Admin", "/User/OpinionRequest/New",
                                labcons.UserID.Value, MySession.SystemSession.SystemUserID * -1, db,
                               "i-Speach-Bubble-6 text-primary mr-1", "Opinion Request Comment From System Admin", ("Request ID #" + opinionrequestobj.OpinionRequestID.ToString("000")), "toastr-sucsess");
                        }
                    }

                    NotificationManager.Fire(("Request ID #" + opinionrequestobj.OpinionRequestID.ToString("000")), "Opinion Request Assigned", "/Admin/ConsultantRequest/NewRequest",
                        assignedtoconsultant, MySession.SystemSession.SystemUserID, "i-Speach-Bubble-6 text-primary mr-1", "Opinion Request Assigned", ("Request ID #" + opinionrequestobj.OpinionRequestID.ToString("000")), "toastr-sucsess");

                    NotificationManager.Fire(("Request ID #" + opinionrequestobj.OpinionRequestID.ToString("000")), "Opinion Request Comment From System Admin", "/Admin/ConsultantRequest/NewRequest",
                        assignedtoconsultant, MySession.SystemSession.SystemUserID, "i-Speach-Bubble-6 text-primary mr-1", "Opinion Request Comment From System Admin", ("Request ID #" + opinionrequestobj.OpinionRequestID.ToString("000")), "toastr-sucsess");
                }


                return WebJSResponse.ResponseSWAL(SwalEnum.success, "Saved Successfully !", "Request has been saved successfully<br>", new { opinionrequestobjjson = opinionrequestobj });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("NewOpinion")]
        [SystemAuthorizeMember]
        public ActionResult NewOpinion()
        {
            return View("~/Views/Admin/OpinionRequest/NewOpinion.cshtml");
        }


        [Route("AssignedOpinion")]
        [SystemAuthorizeMember]
        public ActionResult AssignedOpinion()
        {
            return View("~/Views/Admin/OpinionRequest/AssignedOpinion.cshtml");
        }

        [Route("CompletedOpinion")]
        [SystemAuthorizeMember]
        public ActionResult CompletedOpinion()
        {
            return View("~/Views/Admin/OpinionRequest/CompletedOpinion.cshtml");
        }


        [Route("CancelledOpinion")]
        [SystemAuthorizeMember]
        public ActionResult CancelledOpinion()
        {
            return View("~/Views/Admin/OpinionRequest/CancelledOpinion.cshtml");
        }
    }
}