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
    [RoutePrefix("User/Home")]
    public class HomeController : RedirectController
    {
        // GET: Home
        [Route]
        // GET: Home
        [ClientAuthorizeMember]
        public ActionResult Index()
        {


            string sub = Request.Url.DnsSafeHost.GetSubdomain();
            if (sub != null)
            {

                ViewBag.RoleIDForDashboard = MySession.GetClientSession(this.subdomainurl).CurrentRole.RoleID;
                
            }
           
            return View("~/Views/User/Home/Index.cshtml");
        }
        [HttpGet]
        [Route("PatientRegistration")]
        public ActionResult PatientRegistration(string status)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IClientUserTypeRepository clientusertype = this.currentdomaindb.ClientUserTypeRepository();
                Repositories.User.IPatientDetailRepository patientdetail = this.currentdomaindb.PatientDetailRepository();


                int clientuserdetailtypeid = clientusertype.GetAll().Where(x => x.TypeName.Equals("Patient")).FirstOrDefault().ClientUserTypeID;

                object returnList = null;
                status = "All";
                if (status == "All")
                {
                    returnList = (from cl in clientuser.GetAll()
                                  join pd in patientdetail.GetAll() on cl.DetailID equals pd.PatientDetailID
                                  where cl.DetailType.Value == clientuserdetailtypeid
                                  select new
                                  {
                                      cl.ClientUserID,
                                      cl.Username,
                                      Status = cl.IsActive.HasValue ? (cl.IsActive.Value ? "Active" : "Inactive") : "Inactive",
                                      FullName = cl.FirstName + " " + pd.MiddleName + " " + cl.LastName,
                                      pd.Sex,
                                      cl.IsBlock,
                                  
                                      CreatedDate = pd.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt")
                                  }).ToList().GroupBy(e => e.CreatedDate); 
                }
                else if (status == "Active")
                {
                    returnList = (from cl in clientuser.GetAll()
                                  join pd in patientdetail.GetAll() on cl.DetailID equals pd.PatientDetailID
                                  where cl.DetailType.Value == clientuserdetailtypeid
                                  select new
                                  {
                                      cl.ClientUserID,
                                      cl.Username,
                                      Status = cl.IsActive.HasValue ? (cl.IsActive.Value ? "Active" : "Inactive") : "Inactive",
                                      FullName = cl.FirstName + " " + pd.MiddleName + " " + cl.LastName,
                                      pd.Sex,
                                      cl.IsBlock
                                  }).Where(x => x.Status.Equals("Active")).ToList();
                }
                else
                {
                    returnList = (from cl in clientuser.GetAll()
                                  join pd in patientdetail.GetAll() on cl.DetailID equals pd.PatientDetailID
                                  where cl.DetailType.Value == clientuserdetailtypeid
                                  select new
                                  {
                                      cl.ClientUserID,
                                      cl.Username,
                                      Status = cl.IsActive.HasValue ? (cl.IsActive.Value ? "Active" : "Inactive") : "Inactive",
                                      FullName = cl.FirstName + " " + pd.MiddleName + " " + cl.LastName,
                                      pd.Sex,
                                      cl.IsBlock
                                  }).Where(x => x.Status.Equals("Inactive")).ToList();
                }



                return WebJSResponse.ResponseSimple(new { patientjson = returnList });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }
        [HttpGet]
        [Route("MedicalTestRegistration")]
        public ActionResult MedicalTestRegistration(string status)
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
                    if (MySession.GetClientSession(this.subdomainurl).CurrentRole.RoleName.Equals("Receptionist"))
                    {
                        teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("On Receptionist Plate")).FirstOrDefault().TestStatusID;
                    }
                    else if (MySession.GetClientSession(this.subdomainurl).CurrentRole.RoleName.Equals("Lab Technician"))
                    {
                        teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("On Lab Technician Plate")).FirstOrDefault().TestStatusID;
                    }
                    else if (MySession.GetClientSession(this.subdomainurl).CurrentRole.RoleName.Equals("Clinical Laboratory Scientist"))
                    {
                        teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("On Clinical Lab Scientist Plate")).FirstOrDefault().TestStatusID;
                    }
                    else if (MySession.GetClientSession(this.subdomainurl).CurrentRole.RoleName.Equals("Clinical Laboratory Consultant"))
                    {
                        teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("On Clinical Lab Doctor Plate")).FirstOrDefault().TestStatusID;
                    }
                }
                else if (status.Equals("Completed"))
                {
                    teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("Completed")).FirstOrDefault().TestStatusID;
                }

                var result = (from ts in testrep.GetAll()
                              join ptus in clientuser.GetAll() on ts.PatientUserID equals ptus.ClientUserID
                              join pt in patientdetailrepo.GetAll() on ptus.DetailID equals pt.PatientDetailID
                              //where (ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == teststatusid
                              select new
                              {
                                  ts.TestID,
                                  ts.TestName,
                                  PatientName = (ptus.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + ptus.LastName),
                                  Status = (ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == 5 ? "Completed" : "Pending",
                                  TestCreatedDateCustom = ts.TestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                                  IsPublish = ts.IsPublish.HasValue ? ts.IsPublish.Value ? "Yes" : "No" : "No"
                              }).ToList().GroupBy(e=>e.TestCreatedDateCustom);

                return WebJSResponse.ResponseSimple(new { testjson = result });

            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }
        [HttpGet]
        [Route("SampleCollectionRequest")]
        public ActionResult SampleCollectionRequest(string status)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IPatientDetailRepository patientdetailrepo = this.currentdomaindb.PatientDetailRepository();
                Repositories.User.ITestRepository testrep = this.currentdomaindb.TestRepository();
                Repositories.User.ITestStatusRepositories teststatusrep = this.currentdomaindb.TestStatusRepositories();

                var samplecollection = testrep.GetAll().ToList().GroupBy(e=>e.TestCreatedDate);



                return WebJSResponse.ResponseSimple(new { samplecollection = samplecollection });





            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }
        [HttpGet]
        [Route("TestReportRequests")]
        public ActionResult TestReportRequests(string status)
        {
            try
            {

                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.ITestRepository testrep = this.currentdomaindb.TestRepository();
                Repositories.User.ITestStatusRepositories teststatusrep = this.currentdomaindb.TestStatusRepositories();
                Repositories.User.ITestInvestigationRepository testinvestigation = this.currentdomaindb.TestInvestigationRepository();
                Repositories.User.IRoleRepository role = this.currentdomaindb.RoleRepository();
                Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();

                var testinvestigationdata = testinvestigation.GetAll().ToList().GroupBy(e=>e.CreatedDate);



                return WebJSResponse.ResponseSimple(new { testinvestigation = testinvestigationdata });





            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }
        [HttpGet]
        [Route("ExtraWorkRequests")]
        public ActionResult ExtraWorkRequests(string status)
        {
            try
            {

                Repositories.User.ITestRepository testrep = this.currentdomaindb.TestRepository();
                Repositories.User.IExtraWorkRequestReposotory extrawork = this.currentdomaindb.ExtraWorkRequestRepository();
                Repositories.User.IExtraWorkRequestStatusRepository extrastatus = this.currentdomaindb.ExtraWorkRequestStatusRepository();

                var extraworkdata = extrawork.GetAll().ToList().GroupBy(e=>e.RequestCreatedDate);



                return WebJSResponse.ResponseSimple(new { extraworkdata = extraworkdata });





            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }
        [HttpGet]
        [Route("Inventory")]
        public ActionResult Inventory(string status)
        {
            try
            {
                Repositories.User.IInventory inventory = this.currentdomaindb.InventoryRepository();
                Repositories.User.IInventoryStatusTypeRepository inventorystatustype = this.currentdomaindb.InventoryStatusTypeRepository();

                object returnlist = null; object returnlist2 = null;
                returnlist = (from inv in inventory.GetAll()
                              join invst in inventorystatustype.GetAll() on inv.InventoryStatusID equals invst.InventoryStatusTypeID
                              select new
                              {
                                  inv.ItemName,
                                  inv.OrderedHistory,
                                  inv.QuantityLeft,
                                  inv.QuantityOrdered,
                                  inv.UpdatedBy,
                                  inv.UpdatedDate,
                                  inv.InventoryStatusID,
                                  inv.InventoryID,
                                  ExpiryDate = inv.ExpiryDate.Value.ToString("MM/dd/yyyy"),
                                  InventoryStatusCustom = invst.StatusName,
                                  inv.Description,
                                  inv.CreatedDate,
                                  inv.CreatedBy,
                                  inv.Code
                              }).Where(x => (x.QuantityLeft == "0")).OrderByDescending(x => x.InventoryID).ToList().Count;
                returnlist2 = (from inv in inventory.GetAll()
                              join invst in inventorystatustype.GetAll() on inv.InventoryStatusID equals invst.InventoryStatusTypeID
                              select new
                              {
                                  inv.ItemName,
                                  inv.OrderedHistory,
                                  inv.QuantityLeft,
                                  inv.QuantityOrdered,
                                  inv.UpdatedBy,
                                  inv.UpdatedDate,
                                  inv.InventoryStatusID,
                                  inv.InventoryID,
                                  ExpiryDate = inv.ExpiryDate.Value.ToString("MM/dd/yyyy"),
                                  InventoryStatusCustom = invst.StatusName,
                                  inv.Description,
                                  inv.CreatedDate,
                                  inv.CreatedBy,
                                  inv.Code
                              }).Where(x => (x.QuantityLeft != "0")).OrderByDescending(x => x.InventoryID).ToList().Count;


                //return WebJSResponse.ResponseSimple(new { inventoryjson = returnlist });

                return WebJSResponse.ResponseSimple(new { inventoryjson = returnlist, returnlist2 });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }
        [HttpGet]
        [Route("InventoryRequests")]
        public ActionResult InventoryRequests(string status)
        {
            try
            {
                Repositories.User.IInventoryRequestRepository inventoryrequest = this.currentdomaindb.InventoryRequestRepository();
                Repositories.User.IInventoryRequestStatusRepository inventoryrequeststatus = this.currentdomaindb.InventoryRequestStatusRepository();


                int open = (from inv in inventoryrequest.GetAll()
                                 join invst in inventoryrequeststatus.GetAll() on inv.StatusID equals invst.InventoryRequestStatusID
                                 select new
                                 {
                                     inv.InventoryRequestID,
                                     inv.ItemName,
                                     inv.Description,
                                     inv.Quantity,
                                     inv.StatusID,
                                     StatusCustom = invst.StatusName
                                 }).Where(x => (x.StatusCustom == "Open" || x.StatusCustom == "Progress")).Count();
                int completed = (from inv in inventoryrequest.GetAll()
                                 join invst in inventoryrequeststatus.GetAll() on inv.StatusID equals invst.InventoryRequestStatusID
                                 select new
                                 {
                                     inv.InventoryRequestID,
                                     inv.ItemName,
                                     inv.Description,
                                     inv.Quantity,
                                     inv.StatusID,
                                     StatusCustom = invst.StatusName
                                 }).Where(x => (x.StatusCustom == "Rejected" || x.StatusCustom == "Completed")).Count(); ;

                int[] data = new int[2];
                data[0] = open;
                data[1] = completed;





                return WebJSResponse.ResponseSimple(new { inventoryrequestjson = data });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }
        [HttpGet]
        [Route("SecondOpinionRequest")]
        public ActionResult SecondOpinionRequest(string status)
        {
            try
            {
                Repositories.User.IOpinionRequestRepository opinionrequest = this.currentdomaindb.OpinionRequestRepository();
                Repositories.User.IOpinionRequestStatus opinionrequeststatus = this.currentdomaindb.OpinionRequestStatus();
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();


                var createdDates = (from cl in opinionrequest.GetAll()
                                    select new
                                    {
                                        CreatedDate = cl.RequestCreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                                    }).ToArray();

                status="New/In-Progress";

                var newRequest = (from cl in opinionrequest.GetAll().
                               Where(x => x.StatusID.Equals(1))
                                  select new
                                  {
                                      CreatedDate = cl.RequestCreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                                  }).ToList().GroupBy(e => e.CreatedDate);
                var pendingRequest = (from cl in opinionrequest.GetAll().
                           Where(x => x.StatusID.Equals(2))
                                  select new
                                  {
                                      CreatedDate = cl.RequestCreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                                  }).ToList().GroupBy(e => e.CreatedDate);
                var closedRequest = (from cl in opinionrequest.GetAll().
                           Where(x => x.StatusID.Equals(3))
                                  select new
                                  {
                                      CreatedDate = cl.RequestCreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                                  }).ToList().GroupBy(e => e.CreatedDate);





                return WebJSResponse.ResponseSimple(new { newRequest, pendingRequest, closedRequest, createdDates });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        } [HttpGet]
        [Route("ReportsForPatient")]
        public ActionResult ReportsForPatient(string status)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IPatientDetailRepository patientdetailrepo = this.currentdomaindb.PatientDetailRepository();
                Repositories.User.ITestRepository testrep = this.currentdomaindb.TestRepository();
                Repositories.User.ITestStatusRepositories teststatusrep = this.currentdomaindb.TestStatusRepositories();

                int teststatusidOpen = 0;
                int teststatusidCompleted = 0;

              
                    teststatusidOpen = teststatusrep.GetAll().Where(x => x.StatusName.Equals("Completed")).FirstOrDefault().TestStatusID;
                    teststatusidCompleted = teststatusrep.GetAll().Where(x => x.StatusName.Equals("Completed")).FirstOrDefault().TestStatusID;




                    object result1 = new { };
                    object result2 = new { };
  result1 = (from ts in testrep.GetAll()
                              join ptus in clientuser.GetAll() on ts.PatientUserID equals ptus.ClientUserID
                              join pt in patientdetailrepo.GetAll() on ptus.DetailID equals pt.PatientDetailID
                              join dst in teststatusrep.GetAll() on ts.TestStatusID equals dst.TestStatusID
            where (((ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == teststatusidOpen) || teststatusidOpen == -1)
                              select new
                              {
                                  ts.TestID,
                                  ts.TestName,
                                  PatientName = (ptus.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + ptus.LastName),
                                  Status = (ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == 5 ? "Completed" : "Pending",
                                  TestCreatedDateCustom = ts.TestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                                  IsPublish = ts.IsPublish.HasValue ? ts.IsPublish.Value ? "Yes" : "No" : "No",
                                  DetailStatus = dst.StatusName

                              }).ToList().Count;
               
                    result2 = (from ts in testrep.GetAll()
                              join ptus in clientuser.GetAll() on ts.PatientUserID equals ptus.ClientUserID
                              join pt in patientdetailrepo.GetAll() on ptus.DetailID equals pt.PatientDetailID
                              join dst in teststatusrep.GetAll() on ts.TestStatusID equals dst.TestStatusID
                              where (((ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) != teststatusidOpen))
                              select new
                              {
                                  ts.TestID,
                                  ts.TestName,
                                  PatientName = (ptus.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + ptus.LastName),
                                  Status = (ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == 5 ? "Completed" : "Pending",
                                  TestCreatedDateCustom = ts.TestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                                  IsPublish = ts.IsPublish.HasValue ? ts.IsPublish.Value ? "Yes" : "No" : "No",
                                  DetailStatus = dst.StatusName

                              }).ToList().Count;


                    return WebJSResponse.ResponseSimple(new { testjson = result1, result2 });


            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }
    }
}