using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repositories = LaboratoryBusiness.Repositories;
using BLL = LaboratoryBusiness.BLL;
using BusinessPOCO = LaboratoryBusiness.POCO;
using System.IO;

namespace LaboratorySystem.Controllers.User
{
    [RoutePrefix("User/ExtraWorkRequest")]
    public class ExtraWorkRequestController : RedirectController
    {

        [Route("GetTestDetail")]
        [HttpPost]
        public JsonResult GetTestDetail(int testid)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IPatientDetailRepository patientdetail = this.currentdomaindb.PatientDetailRepository();
                Repositories.User.ITestRepository testrepo = this.currentdomaindb.TestRepository();


                if (testid > 0)
                {

                    var test = testrepo.GetByID(testid);
                    var clientuserobj = clientuser.GetByID((test.PatientUserID.HasValue ? test.PatientUserID.Value : 0));
                    var patientdetailobj = patientdetail.GetAll().Where(x => x.PatientDetailID == clientuserobj.DetailID.Value).FirstOrDefault();
                    if (test != null && clientuserobj != null && patientdetailobj !=null)
                    {
                        var testobj = new
                        {
                            TestID = test.TestID,
                            TestName = test.TestName,
                            PatientName = clientuserobj.FirstName + " " + (patientdetailobj.MiddleName == null ? "" : patientdetailobj.MiddleName) + " " + clientuserobj.LastName,
                            IsSampleCollected = test.IsSampleCollected.HasValue?(test.IsSampleCollected.Value?"Yes":"No"):"No",
                            ComplaintHistory = test.ComplaintHistory==null?"":test.ComplaintHistory,
                            TestCreatedDate = test.TestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt")
                        };
                        return WebJSResponse.ResponseSimple(new { testjson = testobj });
                    }

                    else
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "No test found !", "There is no test associated to this test id.", new { });
                    }


                }

                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Incorrect test-id !", "Kindly provide correct test id.", new { });


            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [HttpPost]
        [Route("UploadFile")]
        public JsonResult UploadFile(int ExtraWorkID)
        {
            try
            {
                LaboratoryBusiness.Repositories.User.IExtraWorkAttachmentRepository extraworkattachment = this.currentdomaindb.ExtraWorkAttachmentRepository();
                LaboratoryBusiness.POCO.User.Cl_ExtraWorkAttachment extraworkattachmentobj = new BusinessPOCO.User.Cl_ExtraWorkAttachment();

                Client cl = new Client(this.subdomainurl);
                FileInitializer fl = new FileInitializer(cl);
                FileManager fm = new FileManager();
                string path = string.Empty;

                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                    //string filename = Path.GetFileName(Request.Files[i].FileName);  

                    HttpPostedFileBase file = files[i];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }


                    fm = new FileManager();
                    fm.FileBytes = FileManager.InputStreamToByteArray(file.InputStream);
                    fm.MimeType = file.ContentType;
                    fm.Extension = Path.GetExtension(file.FileName);
                    fm.Name = Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.ToString("ddMMyyyyhhmmss");

                    path = string.Empty;

                    fl.UploadFile(fm, "\\TestReportFiles", out path);

                    path = HelpingClass.LocalUploadPathToRelativeWebPath(path);

                    extraworkattachmentobj = new BusinessPOCO.User.Cl_ExtraWorkAttachment();
                    extraworkattachmentobj.Link = path;
                    extraworkattachmentobj.Extension = fm.Extension;
                    extraworkattachmentobj.Name = Path.GetFileNameWithoutExtension(file.FileName);
                    extraworkattachmentobj.CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                    extraworkattachmentobj.CreatedDate = DateTime.Now;
                    extraworkattachmentobj.ExtraWorkID = ExtraWorkID;

                    extraworkattachment.Insert(extraworkattachmentobj);
                    extraworkattachment.Save();

                }
                // Returns message that successfully uploaded  
                    return WebJSResponse.ResponseSimple(new { ResponseType = "swal-success", Title = "Requested Successfully !", Description = "Request has been saved successfully<br>." });

            }
            catch (Exception ex)
            {

                return WebJSResponse.ResponseSimple(new { ResponseType = "swal-warning", Title = "Requested Successfully !", Description = "Request has been saved successfully<br>but something went wrong while uploading files." });
            }
        }

        [Route("AddExtraWorkRequest")]
        [HttpPost]
        public JsonResult AddExtraWorkRequest(string helevels, string specialstains, string immunohistochemistry, string others, int testid)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IExtraWorkRequestReposotory extraworkrep = this.currentdomaindb.ExtraWorkRequestRepository();
                Repositories.User.IExtraWorkRequestStatusRepository extraworkstatusrep = this.currentdomaindb.ExtraWorkRequestStatusRepository();
                Repositories.User.IRoleRepository role = this.currentdomaindb.RoleRepository();
                Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();

                int extraworkstatusid = extraworkstatusrep.GetAll().Where(x => x.StatusName.Equals("New")).FirstOrDefault().WorkRequestStatusID;

                BusinessPOCO.User.Cl_ExtraWorkRequest extrawork = new BusinessPOCO.User.Cl_ExtraWorkRequest();
                extrawork.TestID = testid;
                extrawork.H_ELevels = helevels.Trim();
                extrawork.SpecialStains = specialstains.Trim();
                extrawork.ImmunoHistoChemistry = immunohistochemistry.Trim();
                extrawork.Others = others.Trim();
                extrawork.StatusID = extraworkstatusid;
                extrawork.RequestCreatedDate = DateTime.Now;
                extrawork.RequestCreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;


                extraworkrep.Insert(extrawork);
                extraworkrep.Save();

                var _labsci_users = (from rlm in rolemapping.GetAll()
                                      join rl in role.GetAll() on rlm.RoleID equals rl.RoleID
                                     where rl.RoleName.Equals("Clinical Laboratory Scientist")
                                      select rlm).ToList();

                foreach (var labsci in _labsci_users)
                {
                    NotificationManager.FireOnClient(("Request ID #" + extrawork.ExtraWorkID.ToString("000")), "Extra Work Request", "/User/ExtraWorkRequest/Open",
                        labsci.UserID.Value, MySession.GetClientSession(this.subdomainurl).ClientUserID, this.currentdomaindb,
                       "i-Speach-Bubble-6 text-primary mr-1", "New Extra Work Request", ("Request ID #" + extrawork.ExtraWorkID.ToString("000")), "toastr-sucsess");
                }

                return WebJSResponse.ResponseSimple(new { extraworkjson = extrawork });


            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("Getlist")]
        [HttpPost]
        public JsonResult Getlist(string status)
        {

            try
            {
                Repositories.User.ITestRepository testrep = this.currentdomaindb.TestRepository();
                Repositories.User.IExtraWorkRequestReposotory extrawork = this.currentdomaindb.ExtraWorkRequestRepository();
                Repositories.User.IExtraWorkRequestStatusRepository extrastatus = this.currentdomaindb.ExtraWorkRequestStatusRepository();
                Repositories.User.IPatientDetailRepository patientrepo = this.currentdomaindb.PatientDetailRepository();
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IHospitalDetail hospitalDetail = this.currentdomaindb.HospitalDetailRepository();
                int hospitalid = MySession.GetClientSession(this.subdomainurl).HospitalDetailID.HasValue? MySession.GetClientSession(this.subdomainurl).HospitalDetailID.Value:0;

                object result = new {};        
        
                if (status.Equals("Open"))
                {
                    result = (hospitalid != 0)?(from ew in extrawork.GetAll()
                              join ews in extrastatus.GetAll() on ew.StatusID equals ews.WorkRequestStatusID
                              join tst in testrep.GetAll() on ew.TestID equals tst.TestID
                              join cl in clientuser.GetAll() on tst.PatientUserID equals cl.ClientUserID
                              join pt in patientrepo.GetAll() on cl.DetailID equals pt.PatientDetailID
                              join hs in hospitalDetail.GetAll() on pt.HospitalID equals hs.HospitalDetailID into hspd
                              from m in hspd.DefaultIfEmpty()
                              where pt.HospitalID == hospitalid && ( ew.StatusID.Value==1 || ew.StatusID.Value==2) 
                              select new { ew.ExtraWorkID, tst.TestName,
                                  PatientName = (cl.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + cl.LastName),
                                  City=pt.City,
                                  HospitalName = (pt.HospitalID != null) ? m.HospitalName : pt.ReferingHospital,
                                  ReportRefrenceNumber = tst.SampleLabel,
                                  RequestCreatedDate = ew.RequestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                              StatusCustom=ews.StatusName}).ToList()
                    : (from ew in extrawork.GetAll()
                       join ews in extrastatus.GetAll() on ew.StatusID equals ews.WorkRequestStatusID
                       join tst in testrep.GetAll() on ew.TestID equals tst.TestID
                       join cl in clientuser.GetAll() on tst.PatientUserID equals cl.ClientUserID
                       join pt in patientrepo.GetAll() on cl.DetailID equals pt.PatientDetailID
                       join hs in hospitalDetail.GetAll() on pt.HospitalID equals hs.HospitalDetailID into hspd
                       from m in hspd.DefaultIfEmpty()
                       where ew.StatusID.Value == 1 || ew.StatusID.Value == 2
                       select new
                       {
                           ew.ExtraWorkID,
                           tst.TestName,
                           PatientName = (cl.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + cl.LastName),
                           City = pt.City,
                           HospitalName = (pt.HospitalID != null) ? m.HospitalName : pt.ReferingHospital,
                           ReportRefrenceNumber = tst.SampleLabel,
                           RequestCreatedDate = ew.RequestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                           StatusCustom = ews.StatusName
                       }).ToList();
                }
                else if (status.Equals("Completed"))
                {
                    result = (from ew in extrawork.GetAll()
                              join ews in extrastatus.GetAll() on ew.StatusID equals ews.WorkRequestStatusID
                              join tst in testrep.GetAll() on ew.TestID equals tst.TestID
                              join cl in clientuser.GetAll() on tst.PatientUserID equals cl.ClientUserID
                              join pt in patientrepo.GetAll() on cl.DetailID equals pt.PatientDetailID
                              join hs in hospitalDetail.GetAll() on pt.HospitalID equals hs.HospitalDetailID into hspd
                              from m in hspd.DefaultIfEmpty()
                              where ew.StatusID.Value == 3
                              select new
                              {
                                  ew.ExtraWorkID,
                                  tst.TestName,
                                  PatientName = (cl.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + cl.LastName),
                                  City = pt.City,
                                  HospitalName= (pt.HospitalID != null) ? m.HospitalName : pt.ReferingHospital,
                                  ReportRefrenceNumber=tst.SampleLabel,
                                  RequestCreatedDate = ew.RequestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                                  StatusCustom = ews.StatusName
                              }).ToList();
                }
                else
                {
                    result = (from ew in extrawork.GetAll()
                              join ews in extrastatus.GetAll() on ew.StatusID equals ews.WorkRequestStatusID
                              join tst in testrep.GetAll() on ew.TestID equals tst.TestID
                              join cl in clientuser.GetAll() on tst.PatientUserID equals cl.ClientUserID
                              join pt in patientrepo.GetAll() on cl.DetailID equals pt.PatientDetailID
                              join hs in hospitalDetail.GetAll() on pt.HospitalID equals hs.HospitalDetailID into hspd
                              from m in hspd.DefaultIfEmpty()
                              where ew.StatusID.Value == 1 || ew.StatusID.Value == 2 || ew.StatusID.Value==3
                              select new
                              {
                                  ew.ExtraWorkID,
                                  tst.TestName,
                                  PatientName = (cl.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + cl.LastName),
                                  City = pt.City,
                                  HospitalName = (pt.HospitalID != null) ? m.HospitalName : pt.ReferingHospital,
                                  ReportRefrenceNumber = tst.SampleLabel,
                                  RequestCreatedDate = ew.RequestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                                  StatusCustom = ews.StatusName
                              }).ToList();
                }


                return WebJSResponse.ResponseSimple(new { extraworkrequestjson = result });


            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [HttpPost]
        [Route("GetDetail")]
        public JsonResult GetDetail(int extraworkrequestid)
        {
            try
            {
                Repositories.User.ITestRepository testrep = this.currentdomaindb.TestRepository();
                Repositories.User.IExtraWorkRequestReposotory extrawork = this.currentdomaindb.ExtraWorkRequestRepository();
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IExtraWorkRequestStatusRepository extrastatus = this.currentdomaindb.ExtraWorkRequestStatusRepository();
                Repositories.User.IPatientDetailRepository patientrepo = this.currentdomaindb.PatientDetailRepository();
                Repositories.User.ITestInvestigationRepository testinvestigationrepo = this.currentdomaindb.TestInvestigationRepository();
                Repositories.User.IExtraWorkAttachmentRepository extraworkattachmentrepo = this.currentdomaindb.ExtraWorkAttachmentRepository();

                object returnlist = null;
                object openAction = null;
                object progressAction = null;
                object investigationlist = null;
                object attachments = null;
                List<BusinessPOCO.User.Cl_ExtraWorkRequestStatus> extrarequeststatuses = null;
                var data = extrawork.GetByID(extraworkrequestid);
                if (data != null)
                {
                    var cu = clientuser.GetByID(data.RequestCreatedBy.Value);
                    var test = testrep.GetByID(data.TestID);
                    var patdetail = clientuser.GetByID(Convert.ToInt32(test.PatientUserID));
                    var pat =patientrepo.GetByID(Convert.ToInt32(patdetail.DetailID));
                    
                   

                    returnlist = new
                    {
                        data.ExtraWorkID,
                        test.TestName,
                        data.H_ELevels,
                        data.SpecialStains,
                        data.ImmunoHistoChemistry,
                        data.Others,
                        PatientName = patdetail.FirstName+" "+patdetail.LastName,
                        StatusName = extrastatus.GetByID(data.StatusID.Value).StatusName,
                        RequestCreatedDateCustom = data.RequestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                        RequestCreatedByName = cu.FirstName + " " + cu.LastName
                    };

                    if (data.NewActionBy.HasValue)
                    {
                        cu = clientuser.GetByID(data.NewActionBy.Value);
                        openAction = new
                        {
                            OpenActionByName = cu.FirstName + " " + cu.LastName,
                            OpenActionDateCustom = data.NewActionDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                            OpenActionComments = data.NewActionComments,
                            OpenActionStatusName = extrastatus.GetByID(data.NewActionStatusID.Value).StatusName
                        };
                    }

                    if (data.PendingActionBy.HasValue)
                    {
                        cu = clientuser.GetByID(data.PendingActionBy.Value);
                        progressAction = new
                        {
                            ProgressActionByName = cu.FirstName + " " + cu.LastName,
                            ProgressActionDateCustom = data.PendingActionDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                            ProgressActionComments = data.PendingActionComments,
                            ProgressActionStatusName = extrastatus.GetByID(data.StatusID.Value).StatusName
                        };
                    }

                    extrarequeststatuses = extrastatus.GetAll().Where(x => !x.StatusName.Equals("New") && !x.StatusName.Equals("New-Deleted") && !x.StatusName.Equals("In-Progress-Deleted")).ToList();
                    if (openAction != null)
                    {
                        extrarequeststatuses.Remove(extrarequeststatuses.Where(x => x.StatusName.Equals("In-Progress")).FirstOrDefault());
                    }

                    investigationlist = testinvestigationrepo.GetAll().Where(x => (x.ExtraWorkID.HasValue ? x.ExtraWorkID : 0) == data.ExtraWorkID).ToList();
                    attachments = extraworkattachmentrepo.GetAll().Where(x => (x.ExtraWorkID.HasValue ? x.ExtraWorkID.Value : 0) == data.ExtraWorkID).ToList();
                }

                return WebJSResponse.ResponseSimple(new { extraworkrequestobjjson = returnlist, openactionjson = openAction, progressactionjson = progressAction, extraworkrequeststatusesjson = extrarequeststatuses
                ,investigationlistjson = investigationlist,attachmentsjson=attachments});




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("UpdateStatus")]
        [HttpPost]
        public JsonResult UpdateStatus(int extraworkrequestid, string comments, int statusid,List<ViewModels.TestInvestigation> testinvestigations)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IExtraWorkRequestReposotory extrawokrequest = this.currentdomaindb.ExtraWorkRequestRepository();
                Repositories.User.IExtraWorkRequestStatusRepository extraworkrequeststatus = this.currentdomaindb.ExtraWorkRequestStatusRepository();
                Repositories.User.ITestInvestigationRepository testinvestigation = this.currentdomaindb.TestInvestigationRepository();
                Repositories.User.IRoleRepository role = this.currentdomaindb.RoleRepository();
                Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();

                var extraworkrequestobj = extrawokrequest.GetByID(extraworkrequestid);

                string statusname = string.Empty;
                if (statusid != -100)
                {
                    statusname = extraworkrequeststatus.GetByID(statusid).StatusName;
                    if (statusname.Equals("Completed"))
                    {
                        foreach (var inv in testinvestigations)
                        {
                            testinvestigation.Insert(new BusinessPOCO.User.Cl_TestInvestigation()
                            {
                                InvestigationName = inv.InvestigationName,
                                Value = inv.InvestigationValues,
                                Range = inv.InvestigationRange,
                                InvestigationResult = inv.InvestigationResult,
                                TestID = extraworkrequestobj.TestID,
                                ExtraWorkID = extraworkrequestobj.ExtraWorkID,
                                CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID,
                                CreatedDate = DateTime.Now
                            });
                        }

                        if (testinvestigations.Count > 0)
                        {
                            testinvestigation.Save();
                        }
                    }
                }
                else
                {
                    statusname = "Deleted";
                    BusinessPOCO.User.Cl_ExtraWorkRequestStatus reqstatus = null;
                    if (extraworkrequestobj.NewActionBy.HasValue)
                    {
                        reqstatus = extraworkrequeststatus.GetAll().Where(x => x.StatusName.Equals("In-Progress-Deleted")).FirstOrDefault();
                        statusid = reqstatus.WorkRequestStatusID;
                    }
                    else
                    {
                        reqstatus = extraworkrequeststatus.GetAll().Where(x => x.StatusName.Equals("New-Deleted")).FirstOrDefault();
                        statusid = reqstatus.WorkRequestStatusID;
                    }
                }


                extraworkrequestobj.StatusID = statusid;
                if (extraworkrequestobj.NewActionBy.HasValue)
                {

                    extraworkrequestobj.PendingActionBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                    extraworkrequestobj.PendingActionComments = comments;
                    extraworkrequestobj.PendingActionDate = DateTime.Now;
                }

                else
                {
                    extraworkrequestobj.NewActionBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                    extraworkrequestobj.NewActionComments = comments;
                    extraworkrequestobj.NewActionDate = DateTime.Now;
                    extraworkrequestobj.NewActionStatusID = statusid;
                }

                extrawokrequest.Update(extraworkrequestobj);

                extrawokrequest.Save();

                if (statusname.Equals("Completed"))
                {
                    var _labcons_users = (from rlm in rolemapping.GetAll()
                                          join rl in role.GetAll() on rlm.RoleID equals rl.RoleID
                                          where rl.RoleName.Equals("Clinical Laboratory Consultant")
                                          select rlm).ToList();

                    foreach (var labcons in _labcons_users)
                    {
                        NotificationManager.FireOnClient(("Request ID #" + extraworkrequestobj.ExtraWorkID.ToString("000")), "Extra Work Request Completed", "/User/ExtraWorkRequest/Completed",
                            labcons.UserID.Value, MySession.GetClientSession(this.subdomainurl).ClientUserID, this.currentdomaindb,
                           "i-Speach-Bubble-6 text-primary mr-1", "Extra Work Request Completed", ("Request ID #" + extraworkrequestobj.ExtraWorkID.ToString("000")), "toastr-sucsess");
                    }
                }

                if (statusname.Contains("Deleted"))
                {
                    return WebJSResponse.ResponseSimple(new { extraworkrequestjson = extraworkrequestobj,IsDeleted=true });
                }
                else
                {
                    return WebJSResponse.ResponseSimple(new { extraworkrequestjson = extraworkrequestobj, IsDeleted = false });
                }

            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        // GET: ExtraWorkRequest
        [Route("Completed")]
        [ClientAuthorizeMember]
        public ActionResult Completed()
        {
            return View("~/Views/User/ExtraWorkRequest/Completed.cshtml");
        }
        [Route("New")]
        [ClientAuthorizeMember]
        //  [SystemAuthorizeMember]
        [HttpGet]
        public ActionResult New()
        {
            ViewBag.currentdomaindb = this.currentdomaindb;
            ViewBag.subdomainurl = this.subdomainurl;
            return View("~/Views/User/ExtraWorkRequest/New.cshtml");
        }
        [Route("Open")]
        [ClientAuthorizeMember]
        //  [SystemAuthorizeMember]
        [HttpGet]
        public ActionResult Open()
        {
            return View("~/Views/User/ExtraWorkRequest/Open.cshtml");
        }
        [Route("Search")]
        [ClientAuthorizeMember]
        //  [SystemAuthorizeMember]
        [HttpGet]
        public ActionResult Search()
        {
            return View("~/Views/User/ExtraWorkRequest/Search.cshtml");
        }
    }
}