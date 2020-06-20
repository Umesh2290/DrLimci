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
    [RoutePrefix("User/MyReports")]
    public class MyReportsController : RedirectController
    {
        [Route("Getlist")]
        [HttpPost]
        public JsonResult Getlist(string status)
        {

            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IPatientDetailRepository patientdetailrepo = this.currentdomaindb.PatientDetailRepository();
                Repositories.User.ITestRepository testrep = this.currentdomaindb.TestRepository();
                Repositories.User.ITestStatusRepositories teststatusrep = this.currentdomaindb.TestStatusRepositories();

                int teststatusid = 0;

                if (status.Equals("Open"))
                {

                    teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("Completed")).FirstOrDefault().TestStatusID;
                }
                else if (status.Equals("Completed"))
                {
                    teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("Completed")).FirstOrDefault().TestStatusID;
                }

                else if (status.Equals("All"))
                {
                    teststatusid = -1;
                }
                object result = new { };

                if (!status.Equals("Open"))
                {
                    result = (from ts in testrep.GetAll()
                              join ptus in clientuser.GetAll() on ts.PatientUserID equals ptus.ClientUserID
                              join pt in patientdetailrepo.GetAll() on ptus.DetailID equals pt.PatientDetailID
                              join dst in teststatusrep.GetAll() on ts.TestStatusID equals dst.TestStatusID
                              where (((ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == teststatusid) || teststatusid == -1) && ts.PatientUserID.Value == MySession.GetClientSession(this.subdomainurl).ClientUserID
                              select new
                              {
                                  ts.TestID,
                                  ts.TestName,
                                  PatientName = (ptus.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + ptus.LastName),
                                  Status = (ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == 5 ? "Completed" : "Pending",
                                  TestCreatedDateCustom = ts.TestCreatedDate.Value.ToString("dd/MM/yyyy HH:mm tt"),
                                  IsPublish = ts.IsPublish.HasValue ? ts.IsPublish.Value ? "Yes" : "No" : "No",
                                  DetailStatus = dst.StatusName

                              }).ToList();
                }
                else
                {
                    result = (from ts in testrep.GetAll()
                              join ptus in clientuser.GetAll() on ts.PatientUserID equals ptus.ClientUserID
                              join pt in patientdetailrepo.GetAll() on ptus.DetailID equals pt.PatientDetailID
                              join dst in teststatusrep.GetAll() on ts.TestStatusID equals dst.TestStatusID
                              where (((ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) != teststatusid)) && ts.PatientUserID.Value == MySession.GetClientSession(this.subdomainurl).ClientUserID
                              select new
                              {
                                  ts.TestID,
                                  ts.TestName,
                                  PatientName = (ptus.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + ptus.LastName),
                                  Status = (ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == 5 ? "Completed" : "Pending",
                                  TestCreatedDateCustom = ts.TestCreatedDate.Value.ToString("dd/MM/yyyy HH:mm tt"),
                                  IsPublish = ts.IsPublish.HasValue ? ts.IsPublish.Value ? "Yes" : "No" : "No",
                                  DetailStatus = dst.StatusName

                              }).ToList();
                }

                return WebJSResponse.ResponseSimple(new { testjson = result });


            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        // GET: MyReports
        [Route("CompletedReports")]
         [ClientAuthorizeMember]
        public ActionResult CompletedReports()
        {
            return View("~/Views/User/MyReports/CompletedReports.cshtml");
        }
        [Route("PendingReports")]
        [ClientAuthorizeMember]
       // [SystemAuthorizeMember]
        [HttpGet]
        public ActionResult PendingReports()
        {
            return View("~/Views/User/MyReports/PendingReports.cshtml");
        }
    
        }
    }
