using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repositories = LaboratoryBusiness.Repositories;
using BLL = LaboratoryBusiness.BLL;
using BusinessPOCO = LaboratoryBusiness.POCO;

namespace LaboratorySystem.Controllers.User
{

    [RoutePrefix("User/OpinionRequest")]
    public class OpinionRequestController : RedirectController
    {
        // GET: OpinionRequest


        [HttpPost]
        [Route("CreateNew")]
        public JsonResult CreateNew(string patientdetails, string sampleanalysisdetails, string opinionneededdescription)
        {
            try
            {
                Repositories.User.IOpinionRequestRepository opinionrequest = this.currentdomaindb.OpinionRequestRepository();
                Repositories.User.IOpinionRequestStatus opinionrequeststatus = this.currentdomaindb.OpinionRequestStatus();
                Repositories.User.IAdminDetailRepository admindetail = this.currentdomaindb.AdminDetailRepository();
                Repositories.Admin.IOpinionRequestRepository adminopinionrequest = new BLL.Admin.OpinionRequestRepository();
                Repositories.Admin.IRoleMappingRepository adminrolemapping = new BLL.Admin.RoleMappingRepository();
                Repositories.Admin.IRoleRepository adminrole = new BLL.Admin.RoleRepository();
                Repositories.Admin.ISystemUserRepository adminsystemuser = new BLL.Admin.SystemUserRepository();

                DateTime createddate = DateTime.Now;
                BusinessPOCO.User.Cl_OpinionRequestStatus requeststatus = opinionrequeststatus.GetAll().Where(x => x.Name.Equals("New")).FirstOrDefault();

                BusinessPOCO.User.Cl_OpinionRequest opinionrequestobj = new BusinessPOCO.User.Cl_OpinionRequest();
                opinionrequestobj.PatientDetails = patientdetails.Trim();
                opinionrequestobj.SampleAnalysisDetails = sampleanalysisdetails.Trim();
                opinionrequestobj.RequestCreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                opinionrequestobj.RequestCreatedDate = createddate;
                opinionrequestobj.OpinionNeededDescription = opinionneededdescription.Trim();
                opinionrequestobj.SystemOpinionRequestID = 0;
                opinionrequestobj.StatusID = requeststatus.OpinionRequestStatusID;


                opinionrequest.Insert(opinionrequestobj);

                opinionrequest.Save();

                BusinessPOCO.User.Cl_AdminDetail ad_admindetailobj = admindetail.GetAll().FirstOrDefault();

                BusinessPOCO.Admin.OpinionRequest ad_opinionrequestobj = new BusinessPOCO.Admin.OpinionRequest();
                ad_opinionrequestobj.PatientDetails = patientdetails.Trim();
                ad_opinionrequestobj.SampleAnalysisDetails = sampleanalysisdetails.Trim();
                ad_opinionrequestobj.RequestCreatedBy = (MySession.GetClientSession(this.subdomainurl).ClientUserID)*-1;
                ad_opinionrequestobj.RequestCreatedDate = createddate;
                ad_opinionrequestobj.OpinionNeededDescription = opinionneededdescription.Trim();
                ad_opinionrequestobj.ClientID = ad_admindetailobj.SystemClientID;
                ad_opinionrequestobj.ClientOpinionRequestID = opinionrequestobj.OpinionRequestID;
                ad_opinionrequestobj.StatusID = requeststatus.OpinionRequestStatusID;

                adminopinionrequest.Insert(ad_opinionrequestobj);
                adminopinionrequest.Save();

                opinionrequestobj.SystemOpinionRequestID = ad_opinionrequestobj.OpinionRequestID;
                opinionrequest.Update(opinionrequestobj);
                opinionrequest.Save();

                var users = (from rl in adminrole.GetAll()
                             join rlm in adminrolemapping.GetAll() on rl.RoleID equals rlm.RoleID
                             join cu in adminsystemuser.GetAll() on rlm.UserID equals cu.SystemUserID
                             where rl.RoleName.Equals("Superadmin")
                             select cu).ToList();
                foreach (var user in users)
                {
                    NotificationManager.Fire("2nd Opinion Request",
                        "Request ID # " + ad_opinionrequestobj.OpinionRequestID.ToString(),
                        "/Admin/OpinionRequest/NewOpinion", user.SystemUserID, ((MySession.GetClientSession(this.subdomainurl).ClientUserID) * -1),
                        "i-Speach-Bubble-6 text-primary mr-1", "Opinion Request", "Request ID # " + ad_opinionrequestobj.OpinionRequestID.ToString(), "toastr-sucsess");
                }

                return WebJSResponse.ResponseSWAL(SwalEnum.success, "Created Successfully !", "Opinion request has been created successfully<br>", new { });


            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [HttpPost]
        [Route("GetList")]
        public JsonResult GetList(string status)
        {
            try
            {
                Repositories.User.IOpinionRequestRepository opinionrequest = this.currentdomaindb.OpinionRequestRepository();
                Repositories.User.IOpinionRequestStatus opinionrequeststatus = this.currentdomaindb.OpinionRequestStatus();
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();

                object returnlist = null;
                if (status.Equals("New/In-Progress"))
                {
                    returnlist = (from opn in opinionrequest.GetAll()
                                  join opnst in opinionrequeststatus.GetAll() on opn.StatusID equals opnst.OpinionRequestStatusID
                                  join clus in clientuser.GetAll() on opn.RequestCreatedBy equals clus.ClientUserID
                                  select new
                                  {
                                      opn.OpinionRequestID,
                                      opn.PatientDetails,
                                      opn.SampleAnalysisDetails,
                                      opn.OpinionNeededDescription,
                                      opn.StatusID,
                                      IsPublish = opn.IsPublish.HasValue ? (opn.IsPublish.Value ? "Yes" : "No") : "No",
                                      RequestBy=clus.FirstName+" "+clus.LastName,
                                      StatusCustom = opnst.Name,

                                  }).Where(x => (x.StatusCustom == "New" || x.StatusCustom == "In-Progress")).OrderByDescending(x => x.OpinionRequestID).ToList();
                }
                else
                {

                    returnlist = (from opn in opinionrequest.GetAll()
                                  join opnst in opinionrequeststatus.GetAll() on opn.StatusID equals opnst.OpinionRequestStatusID
                                  join clus in clientuser.GetAll() on opn.RequestCreatedBy equals clus.ClientUserID
                                  select new
                                  {
                                      opn.OpinionRequestID,
                                      opn.PatientDetails,
                                      opn.SampleAnalysisDetails,
                                      opn.OpinionNeededDescription,
                                      opn.StatusID,
                                      IsPublish = opn.IsPublish.HasValue?(opn.IsPublish.Value?"Yes":"No"):"No",
                                      RequestBy = clus.FirstName + " " + clus.LastName,
                                      StatusCustom = opnst.Name,

                                  }).Where(x => ((x.StatusCustom == "Completed" && x.IsPublish=="Yes") || x.StatusCustom == "Cancelled")).OrderByDescending(x => x.OpinionRequestID).ToList();
                }




                return WebJSResponse.ResponseSimple(new { opinionrequestjson = returnlist });




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
                Repositories.User.IOpinionRequestRepository opinionrequest = this.currentdomaindb.OpinionRequestRepository();
                Repositories.User.IOpinionRequestStatus opinionrequeststatus = this.currentdomaindb.OpinionRequestStatus();
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();

                object returnlist = null;
                object newAction = null;
                object progressAction = null;
                var data = opinionrequest.GetByID(opinionrequestid);
                if (data != null)
                {
                    BusinessPOCO.User.Cl_ClientUser cu = clientuser.GetByID(data.RequestCreatedBy.Value);
                    returnlist = new
                    {
                        data.OpinionRequestID,
                        data.PatientDetails,
                        data.SampleAnalysisDetails,
                        data.OpinionNeededDescription,
                        data.CommentForRequester,
                        StatusName = opinionrequeststatus.GetByID(data.StatusID.Value).Name,
                        RequestCreatedDateCustom = data.RequestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                        RequestCreatedByName = cu.FirstName + " " + cu.LastName,
                        data.IsPublish,
                        data.OpinionBy,
                        data.ConsultationOpinion
                    };

                    BusinessPOCO.Admin.SystemUser su = new BusinessPOCO.Admin.SystemUser();
                    if (data.NewActionBy.HasValue)
                    {
                        su = systemuser.GetByID(data.NewActionBy.Value*-1);
                        newAction = new
                        {
                            NewActionByName = su.FirstName + " " + su.LastName,
                            NewActionDateCustom = data.NewActionDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                            NewActionComments = data.NewActionComments,
                            NewActionStatusName = opinionrequeststatus.GetByID(data.NewActionStatusID.Value).Name
                        };
                    }

                    if (data.PendingActionBy.HasValue)
                    {
                        su = systemuser.GetByID(data.NewActionBy.Value * -1);
                        progressAction = new
                        {
                            ProgressActionByName = su.FirstName + " " + su.LastName,
                            ProgressActionDateCustom = data.PendingActionDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                            ProgressActionComments = data.PendingActionComments,
                            ProgressActionStatusName = opinionrequeststatus.GetByID(data.StatusID.Value).Name
                        };
                    }

                }

                return WebJSResponse.ResponseSimple(new { opinionrequestobjjson = returnlist, newactionjson = newAction, progressactionjson = progressAction });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("Completed")]
        [ClientAuthorizeMember]
        [HttpGet]
        public ActionResult Completed()
        {
            return View("~/Views/User/OpinionRequest/Completed.cshtml");
        }

        [Route("New")]
        [ClientAuthorizeMember]
        [HttpGet]

        public ActionResult New()
        {
            return View("~/Views/User/OpinionRequest/New.cshtml");
        }

        [Route("Pending")]
        [ClientAuthorizeMember]
        [HttpGet]
        public ActionResult Pending()
        {
            return View("~/Views/User/OpinionRequest/Pending.cshtml");
        }
    }

}