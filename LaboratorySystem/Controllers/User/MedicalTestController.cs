using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repositories = LaboratoryBusiness.Repositories;
using BLL = LaboratoryBusiness.BLL;
using BusinessPOCO = LaboratoryBusiness.POCO;
using System.IO;
using LaboratorySystem.Models;
using System.Dynamic;

namespace LaboratorySystem.Controllers.User
{
    [RoutePrefix("User/MedicalTest")]
    public class MedicalTestController : RedirectController
    {
        // GET: MedicalTest
        [Route]
        [ClientAuthorizeMember]
        public ActionResult Index(string IsEdit, string EditId)
        {
            if (EditId != null && IsEdit == null)
            {
                ViewBag.IsView = true;
            }
            else if (EditId != null && IsEdit != null)
            {
                if (IsEdit.Equals("0"))
                {
                    ViewBag.IsView = true;
                }
                else
                {
                    ViewBag.IsView = false;
                }
            }
            else
            {
                ViewBag.IsView = false;
            }

            ViewBag.currentdomaindb = this.currentdomaindb;
            ViewBag.subdomainurl = this.subdomainurl;
            return View("~/Views/User/MedicalTest/Index.cshtml");
        }

        [Route("GetPatientDetail")]
        [HttpPost]
        public JsonResult GetPatientDetail(int patientid)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IPatientDetailRepository patientdetail = this.currentdomaindb.PatientDetailRepository();


                if (patientid > 0)
                {

                    var clientuserobj = clientuser.GetByID(patientid);
                    var patientdetailobj = patientdetail.GetAll().Where(x => x.PatientDetailID == clientuserobj.DetailID.Value).FirstOrDefault();
                    if (patientdetailobj != null && clientuserobj != null)
                    {
                        var patientuserobj = new
                        {
                            FirstName = clientuserobj.FirstName,
                            LastName = clientuserobj.LastName,
                            FullName = clientuserobj.FirstName + " " + (patientdetailobj.MiddleName == null ? "" : patientdetailobj.MiddleName) + " " + clientuserobj.LastName,
                            MobileNo = clientuserobj.MobileNo,
                            Email = clientuserobj.Email,
                            Address = clientuserobj.Address,
                            IsActive = clientuserobj.IsActive,
                            ClientUserID = clientuserobj.ClientUserID,
                            Username = clientuserobj.Username,
                            MiddleName = patientdetailobj.MiddleName,
                            Payment = patientdetailobj.Payment,
                            PaymentMode = patientdetailobj.PaymentMode,
                            PaymentReceiptPdfLink = patientdetailobj.PaymentReceiptPdfLink,
                            PdfLink = patientdetailobj.PdfLink,
                            ReferingDoctor = patientdetailobj.ReferingDoctor,
                            ReferingHospital = patientdetailobj.ReferingHospital,
                            Sex = patientdetailobj.Sex,
                            Streetname = patientdetailobj.Streetname,
                            City = patientdetailobj.City,
                            AlternatePhoneNo = patientdetailobj.AlternatePhoneNo,
                            AlternateAddress = patientdetailobj.AlternateAddress,
                            Age = patientdetailobj.Age,
                            PatientDetailID = patientdetailobj.PatientDetailID,
                            Status = clientuserobj.IsActive.HasValue ? (clientuserobj.IsActive.Value ? "Active" : "Inactive") : "Inactive",
                            CreatedDateCustom = clientuserobj.CreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                        };
                        return WebJSResponse.ResponseSimple(new { patientuserjson = patientuserobj });
                    }

                    else
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "No patient found !", "There is no patient associated to this patient id.", new { });
                    }


                }

                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Incorrect patient-id !", "Kindly provide correct patient id.", new { });


            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("AddTestInfo")]
        [HttpPost]
        public JsonResult AddTestInfo(string testname, bool issamplerequired, string complainthistory, string description, int patientid)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.ITestRepository testrep = this.currentdomaindb.TestRepository();
                Repositories.User.ITestStatusRepositories teststatusrep = this.currentdomaindb.TestStatusRepositories();
                Repositories.User.IRoleRepository role = this.currentdomaindb.RoleRepository();
                Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();
                Repositories.User.ITestRate testRate = this.currentdomaindb.TestRate();
                var testrate=testRate.GetByID(Convert.ToInt32(testname));
                int teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("On Lab Technician Plate")).FirstOrDefault().TestStatusID;

                BusinessPOCO.User.Cl_Test test = new BusinessPOCO.User.Cl_Test();
                test.TestName = testrate.TestName.ToString();
                test.IsSampleRequired = issamplerequired;
                test.ComplaintHistory = complainthistory;
                test.Description = description;
                test.PatientUserID = patientid;
                test.TestStatusID = teststatusid;
                test.TestCreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                test.TestCreatedDate = DateTime.Now;
                test.Cost = complainthistory;
                testrep.Insert(test);
                testrep.Save();

                var _labtech_users = (from rlm in rolemapping.GetAll()
                                      join rl in role.GetAll() on rlm.RoleID equals rl.RoleID
                                      where rl.RoleName.Equals("Lab Technician")
                                      select rlm).ToList();

                foreach (var labtech in _labtech_users)
                {
                    NotificationManager.FireOnClient(("Test ID #" + test.TestID.ToString("0000")), "Sample Collection Request", "/User/MedicalTest/Open",
                        labtech.UserID.Value, MySession.GetClientSession(this.subdomainurl).ClientUserID, this.currentdomaindb,
                       "i-Speach-Bubble-6 text-primary mr-1", "New Sample Collection Request", ("Test ID #" + test.TestID.ToString("0000")), "toastr-sucsess");
                }
                //return WebJSResponse.ResponseSimple(new { testjson = test });
                return WebJSResponse.ResponseSWAL(SwalEnum.success, "Created Successfully !", "Test has been created successfully<br>", new { testjson = test });


            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("AddSampleCollectionInfo")]
        [HttpPost]
        public JsonResult AddSampleCollectionInfo(bool issamplecollected, string samplelabel, string samplecode, string sampletype, int testid)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.ITestRepository testrep = this.currentdomaindb.TestRepository();
                Repositories.User.ITestStatusRepositories teststatusrep = this.currentdomaindb.TestStatusRepositories();
                Repositories.User.IRoleRepository role = this.currentdomaindb.RoleRepository();
                Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();

                int teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("On Clinical Lab Scientist Plate")).FirstOrDefault().TestStatusID;

                BusinessPOCO.User.Cl_Test test = testrep.GetByID(testid);
                if (test != null)
                {
                    test.IsSampleCollected = issamplecollected;
                    test.SampleLabel = samplelabel;
                    test.SampleCode = samplecode;
                    test.SampleType = sampletype;
                    test.TestStatusID = teststatusid;
                    test.SampleCollectionBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                    test.SampleCollectionDate = DateTime.Now;

                    testrep.Update(test);
                    testrep.Save();

                    var _labsci_users = (from rlm in rolemapping.GetAll()
                                         join rl in role.GetAll() on rlm.RoleID equals rl.RoleID
                                         where rl.RoleName.Equals("Clinical Laboratory Scientist")
                                         select rlm).ToList();

                    foreach (var labsci in _labsci_users)
                    {
                        NotificationManager.FireOnClient(("Test ID #" + test.TestID.ToString("0000")), "Investigation Request", "/User/MedicalTest/Open",
                            labsci.UserID.Value, MySession.GetClientSession(this.subdomainurl).ClientUserID, this.currentdomaindb,
                           "i-Speach-Bubble-6 text-primary mr-1", "New Investigation Request", ("Test ID #" + test.TestID.ToString("0000")), "toastr-sucsess");
                    }

                    return WebJSResponse.ResponseSimple(new { testjson = test });
                }
                else
                {
                    return WebJSResponse.ResponseToastr(ToastrEnum.error, "No test found !", "There is no test associated to this test id.", new { });
                }

            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("AddInvestigationInfo")]
        [HttpPost]
        public JsonResult AddInvestigationInfo(int testid, List<ViewModels.TestInvestigation> testinvestigations, int consultantid)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.ITestRepository testrep = this.currentdomaindb.TestRepository();
                Repositories.User.ITestStatusRepositories teststatusrep = this.currentdomaindb.TestStatusRepositories();
                Repositories.User.ITestInvestigationRepository testinvestigation = this.currentdomaindb.TestInvestigationRepository();
                Repositories.User.IRoleRepository role = this.currentdomaindb.RoleRepository();
                Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();

                int teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("On Clinical Lab Doctor Plate")).FirstOrDefault().TestStatusID;

                BusinessPOCO.User.Cl_Test test = testrep.GetByID(testid);
                if (test != null)
                {
                    test.TestStatusID = teststatusid;
                    test.AnalysisBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                    test.AnalysisDate = DateTime.Now;
                    if (consultantid != 0)
                    {
                        test.ConclusionBy = consultantid;
                    }

                    testrep.Update(test);
                    testrep.Save();

                    foreach (var inv in testinvestigations)
                    {
                        testinvestigation.Insert(new BusinessPOCO.User.Cl_TestInvestigation()
                        {
                            InvestigationName = inv.InvestigationName,
                            Value = inv.InvestigationValues,
                            Range = inv.InvestigationRange,
                            InvestigationResult = inv.InvestigationResult,
                            TestID = testid,
                            CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID,
                            CreatedDate = DateTime.Now
                        });
                    }

                    testinvestigation.Save();

                    var _labdoc_users = (from rlm in rolemapping.GetAll()
                                         join rl in role.GetAll() on rlm.RoleID equals rl.RoleID
                                         where rl.RoleName.Equals("Clinical Laboratory Consultant")
                                         select rlm).ToList();

                    foreach (var labdoc in _labdoc_users)
                    {
                        NotificationManager.FireOnClient(("Test ID #" + test.TestID.ToString("0000")), "Test Conclusion Request", "/User/MedicalTest/Open",
                            labdoc.UserID.Value, MySession.GetClientSession(this.subdomainurl).ClientUserID, this.currentdomaindb,
                           "i-Speach-Bubble-6 text-primary mr-1", "New Test Conclusion Request", ("Test ID #" + test.TestID.ToString("0000")), "toastr-sucsess");
                    }

                    return WebJSResponse.ResponseSimple(new { testjson = test, investigationjson = testinvestigations });
                }
                else
                {
                    return WebJSResponse.ResponseToastr(ToastrEnum.error, "No test found !", "There is no test associated to this test id.", new { });
                }

            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("AddConclusionInfo")]
        [HttpPost]
        public JsonResult AddConclusionInfo(int testid, int conclusionid, int reporttype, string specimendetails, string clinicaldetails, string conclusion
            , string microscopy, string microscopy2, string macroscopy, string snomedcoding, string sampledescription, string report, int ispublish)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.ITestRepository testrep = this.currentdomaindb.TestRepository();
                Repositories.User.ITestStatusRepositories teststatusrep = this.currentdomaindb.TestStatusRepositories();
                Repositories.User.ITestConclusionRepository testconclusion = this.currentdomaindb.TestConclusionRepository();
                Repositories.User.IRoleRepository role = this.currentdomaindb.RoleRepository();
                Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();

                int teststatusid = 0;
                if (ispublish == 0)
                {
                    teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("On Clinical Lab Doctor Plate")).FirstOrDefault().TestStatusID;
                }
                else
                {
                    teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("Completed")).FirstOrDefault().TestStatusID;
                }

                BusinessPOCO.User.Cl_Test test = testrep.GetByID(testid);
                if (test != null)
                {
                    test.TestStatusID = teststatusid;
                    test.ConclusionBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                    test.ConclusionDate = DateTime.Now;
                    if (ispublish != 0)
                    {
                        test.IsPublish = true;
                    }


                    testrep.Update(test);
                    testrep.Save();

                    BusinessPOCO.User.Cl_TestConclusion conclusionobj = testconclusion.GetByID(conclusionid);
                    if (conclusionobj == null)
                    {
                        conclusionobj = new BusinessPOCO.User.Cl_TestConclusion();
                        conclusionobj.TestReportTypeID = reporttype;
                        conclusionobj.TestID = testid;
                        conclusionobj.SpecimenDetails = specimendetails;
                        conclusionobj.ClinicalDetails = clinicaldetails;
                        conclusionobj.Conclusion = conclusion;
                        if (reporttype == 1)
                        {
                            conclusionobj.Microscopy = microscopy;
                        }
                        if (reporttype == 1)
                        {
                            conclusionobj.Macroscopy = macroscopy;
                        }
                        if (reporttype == 1)
                        {
                            conclusionobj.SnomedCoding = snomedcoding;
                        }
                        if (reporttype == 2 || reporttype == 3 || reporttype == 4 || reporttype == 5)
                        {
                            conclusionobj.SampleDescription = sampledescription;
                        }

                        if (reporttype == 2 || reporttype == 3 || reporttype == 4)
                        {
                            conclusionobj.Report = report;
                        }
                        if (reporttype == 5)
                        {
                            conclusionobj.Report = report;

                            conclusionobj.Microscopy = microscopy2;
                        }
                        conclusionobj.CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                        conclusionobj.CreatedDate = DateTime.Now;

                        testconclusion.Insert(conclusionobj);
                        testconclusion.Save();
                    }
                    else
                    {
                        conclusionobj.Microscopy = null;
                        conclusionobj.Microscopy = null;
                        conclusionobj.SnomedCoding = null;
                        conclusionobj.SampleDescription = null;
                        conclusionobj.Report = null;

                        conclusionobj.TestReportTypeID = reporttype;
                        conclusionobj.TestID = testid;
                        conclusionobj.SpecimenDetails = specimendetails;
                        conclusionobj.ClinicalDetails = clinicaldetails;
                        conclusionobj.Conclusion = conclusion;
                        if (reporttype == 1)
                        {
                            conclusionobj.Microscopy = microscopy;
                        }
                        if (reporttype == 1)
                        {
                            conclusionobj.Macroscopy = macroscopy;
                        }
                        if (reporttype == 1)
                        {
                            conclusionobj.SnomedCoding = snomedcoding;
                        }
                        if (reporttype == 2 || reporttype == 3 || reporttype == 4 || reporttype == 5)
                        {
                            conclusionobj.SampleDescription = sampledescription;
                        }

                        if (reporttype == 2 || reporttype == 3 || reporttype == 4)
                        {
                            conclusionobj.Report = report;
                        }
                        if (reporttype == 5)
                        {
                            conclusionobj.Report = report;
                            conclusionobj.Microscopy = microscopy2;
                        }
                        conclusionobj.UpdatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                        conclusionobj.UpdatedDate = DateTime.Now;

                        testconclusion.Update(conclusionobj);
                        testconclusion.Save();
                    }

                    //Report PDF Code
                    if (ispublish == 1)
                    {
                        if (test != null)
                        {
                            try
                            {
                                string pdflink = string.Empty;
                                FileManager fmd = PdfCreator.CreateTestPDF(test.TestID, this.subdomainurl, this.currentdomaindb, out pdflink);
                                test.PdfLink = HelpingClass.LocalUploadPathToRelativeWebPath(pdflink);

                                testrep.Update(test);
                                testrep.Save();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    //Report PDF Code

                    if (test.IsPublish.HasValue ? test.IsPublish.Value : false)
                    {
                        NotificationManager.FireOnClient(("Test ID #" + test.TestID.ToString("0000")), "Test Report Ready", "/User/MyReports/CompletedReports",
                                test.PatientUserID.Value, MySession.GetClientSession(this.subdomainurl).ClientUserID, this.currentdomaindb,
                               "i-Speach-Bubble-6 text-primary mr-1", "Test Report Ready", ("Test ID #" + test.TestID.ToString("0000")), "toastr-sucsess");
                    }

                    return WebJSResponse.ResponseSimple(new { testjson = test, conclusionobjjson = conclusionobj });
                }
                else
                {
                    return WebJSResponse.ResponseToastr(ToastrEnum.error, "No test found !", "There is no test associated to this test id.", new { });
                }

            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }


        [Route("AddSupplementaryInfo")]
        [HttpPost]
        public JsonResult AddSupplementaryInfo(int testid, int supplementid, int reporttype, string specimendetails, string clinicaldetails, string conclusion
            , string microscopy, string microscopy2, string macroscopy, string snomedcoding, string sampledescription, string report, int ispublish)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.ITestRepository testrep = this.currentdomaindb.TestRepository();
                Repositories.User.ITestStatusRepositories teststatusrep = this.currentdomaindb.TestStatusRepositories();
                Repositories.User.ITestSupplementReportRepository testsupplement = this.currentdomaindb.TestSupplementReportRepository();
                Repositories.User.IRoleRepository role = this.currentdomaindb.RoleRepository();
                Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();


                BusinessPOCO.User.Cl_Test test = testrep.GetByID(testid);

                if (test != null)
                {

                    BusinessPOCO.User.Cl_TestSupplementReport supplementobj = testsupplement.GetByID(supplementid);

                    if (supplementobj == null)
                    {
                        supplementobj = new BusinessPOCO.User.Cl_TestSupplementReport();
                        supplementobj.TestReportTypeID = reporttype;
                        supplementobj.TestID = testid;
                        supplementobj.SpecimenDetails = specimendetails;
                        supplementobj.ClinicalDetails = clinicaldetails;
                        supplementobj.SupplementReportConclusion = conclusion;
                        supplementobj.IsPublished = false;
                        if (reporttype == 1)
                        {
                            supplementobj.Microscopy = microscopy;
                        }
                        if (reporttype == 1)
                        {
                            supplementobj.Macroscopy = macroscopy;
                        }
                        if (reporttype == 1)
                        {
                            supplementobj.SnomedCoding = snomedcoding;
                        }
                        if (reporttype == 2 || reporttype == 3 || reporttype == 4 || reporttype == 5)
                        {
                            supplementobj.SampleDescription = sampledescription;
                        }

                        if (reporttype == 2 || reporttype == 3 || reporttype == 4)
                        {
                            supplementobj.Report = report;
                        }
                        if (reporttype == 5)
                        {
                            supplementobj.Microscopy = microscopy2;
                        }
                        supplementobj.CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                        supplementobj.CreatedDate = DateTime.Now;

                        testsupplement.Insert(supplementobj);
                        testsupplement.Save();
                    }

                    else
                    {

                        //supplementobj = new BusinessPOCO.User.Cl_TestSupplementReport();
                        supplementobj.TestReportTypeID = reporttype;
                        supplementobj.TestID = testid;
                        supplementobj.SpecimenDetails = specimendetails;
                        supplementobj.ClinicalDetails = clinicaldetails;
                        supplementobj.SupplementReportConclusion = conclusion;
                        supplementobj.IsPublished = true;
                        if (reporttype == 1)
                        {
                            supplementobj.Microscopy = microscopy;
                        }
                        if (reporttype == 1)
                        {
                            supplementobj.Macroscopy = macroscopy;
                        }
                        if (reporttype == 1)
                        {
                            supplementobj.SnomedCoding = snomedcoding;
                        }
                        if (reporttype == 2 || reporttype == 3 || reporttype == 4 || reporttype == 5)
                        {
                            supplementobj.SampleDescription = sampledescription;
                        }

                        if (reporttype == 2 || reporttype == 3 || reporttype == 4)
                        {
                            supplementobj.Report = report;
                        }
                        if (reporttype == 5)
                        {
                            supplementobj.Microscopy = microscopy2;
                        }
                        supplementobj.UpdatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                        supplementobj.UpdatedDate = DateTime.Now;

                        testsupplement.Update(supplementobj);
                        testsupplement.Save();
                    }


                    //Report PDF Code

                    //Report PDF Code


                    NotificationManager.FireOnClient(("Test ID #" + test.TestID.ToString("0000")), "New Supplement Report", "/User/MyReports/CompletedReports",
                                 test.PatientUserID.Value, MySession.GetClientSession(this.subdomainurl).ClientUserID, this.currentdomaindb,
                                "i-Speach-Bubble-6 text-primary mr-1", "New Supplement Report", ("Test ID #" + test.TestID.ToString("0000")), "toastr-sucsess");

                    return WebJSResponse.ResponseSimple(new { testjson = test, supplementobjjson = supplementobj });
                }
                else
                {
                    return WebJSResponse.ResponseToastr(ToastrEnum.error, "No test found !", "There is no test associated to this test id.", new { });
                }

            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }


        [HttpPost]
        [Route("UploadFile")]
        public JsonResult UploadFile(int TestID, int AttachmentTypeID, int SupplementReportID = 0)
        {
            try
            {
                LaboratoryBusiness.Repositories.User.ITestAttachmentRepository testattachment = this.currentdomaindb.TestAttachmentRepository();
                LaboratoryBusiness.POCO.User.Cl_TestAttachment testattachmentobj = new BusinessPOCO.User.Cl_TestAttachment();

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

                    testattachmentobj = new BusinessPOCO.User.Cl_TestAttachment();
                    testattachmentobj.Link = path;
                    testattachmentobj.Extension = fm.Extension;
                    testattachmentobj.Name = Path.GetFileNameWithoutExtension(file.FileName);
                    testattachmentobj.CreatedBy = SupplementReportID == 0 ? MySession.GetClientSession(this.subdomainurl).ClientUserID : SupplementReportID;
                    testattachmentobj.CreatedDate = DateTime.Now;
                    testattachmentobj.TestID = TestID;
                    testattachmentobj.AttachmentTypeID = AttachmentTypeID;

                    testattachment.Insert(testattachmentobj);
                    testattachment.Save();

                }
                // Returns message that successfully uploaded  
                if (AttachmentTypeID == 1)
                {
                    return WebJSResponse.ResponseSimple(new { ResponseType = "swal-success", Title = "Saved Successfully !", Description = "Sample Collection has been saved successfully<br>." });
                }
                else if (AttachmentTypeID == 2)
                {
                    return WebJSResponse.ResponseSimple(new { ResponseType = "swal-success", Title = "Saved Successfully !", Description = "Investigation Request has been saved successfully<br>." });
                }
                else if (AttachmentTypeID == 3)
                {
                    return WebJSResponse.ResponseSimple(new { ResponseType = "swal-success", Title = "Saved Successfully !", Description = "Report Conclusion has been saved successfully<br>." });
                }

                else if (AttachmentTypeID == 4)
                {
                    return WebJSResponse.ResponseSimple(new { ResponseType = "swal-success", Title = "Saved Successfully !", Description = "Supplement Report has been saved successfully<br>." });
                }
                else if (AttachmentTypeID == 5)
                {
                    return WebJSResponse.ResponseSimple(new { ResponseType = "swal-success", Title = "Saved Successfully !", Description = "Test Report has been saved successfully<br>." });
                }
                else
                {
                    return WebJSResponse.ResponseSimple(new { ResponseType = "swal-success", Title = "Saved Successfully !", Description = "Saved Successfully<br>." });
                }
            }
            catch (Exception ex)
            {

                if (AttachmentTypeID == 1)
                {
                    return WebJSResponse.ResponseSimple(new { ResponseType = "swal-warning", Title = "Saved Successfully !", Description = "Sample Collection has been saved successfully<br>but something went wrong while uploading files." });
                }
                else if (AttachmentTypeID == 2)
                {
                    return WebJSResponse.ResponseSimple(new { ResponseType = "swal-warning", Title = "Saved Successfully !", Description = "Investigation Request has been saved successfully<br>but something went wrong while uploading files." });
                }
                else if (AttachmentTypeID == 3)
                {
                    return WebJSResponse.ResponseSimple(new { ResponseType = "swal-warning", Title = "Saved Successfully !", Description = "Report Conclusion has been saved successfully<br>but something went wrong while uploading files." });
                }
                else if (AttachmentTypeID == 4)
                {
                    return WebJSResponse.ResponseSimple(new { ResponseType = "swal-warning", Title = "Saved Successfully !", Description = "Supplement Report has been saved successfully<br>but something went wrong while uploading files." });
                }
                else if (AttachmentTypeID == 5)
                {
                    return WebJSResponse.ResponseSimple(new { ResponseType = "swal-warning", Title = "Saved Successfully !", Description = "Test Report has been saved successfully<br>but something went wrong while uploading files." });
                }
                else
                {
                    return WebJSResponse.ResponseSimple(new { ResponseType = "swal-warning", Title = "Saved Successfully !", Description = "Saved Successfully<br>but something went wrong while uploading files." });
                }
            }
        }

        [HttpPost]
        [Route("DeleteFile")]
        public JsonResult DeleteFile(int attachmentid)
        {
            try
            {
                Repositories.User.ITestAttachmentRepository testattachment = this.currentdomaindb.TestAttachmentRepository();


                if (attachmentid > 0)
                {
                    testattachment.Delete(attachmentid);
                    testattachment.Save();
                    return WebJSResponse.ResponseToastr(ToastrEnum.success, "Deleted Successfully !", "Attachment has been deleted successfully.", new { });

                }

                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Incorrect attachment-id !", "Kindly provide correct attachment id.", new { });


            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("GetTestDetail")]
        [HttpPost]
        public JsonResult GetTestDetail(int testid)
        {
            object testobj = null;
            object samplecollectionobj = null;
            object investigationobj = null;
            object conclusionobj = null;

            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IPatientDetailRepository patientdetailrepo = this.currentdomaindb.PatientDetailRepository();
                Repositories.User.ITestRepository testrep = this.currentdomaindb.TestRepository();
                Repositories.User.ITestStatusRepositories teststatusrep = this.currentdomaindb.TestStatusRepositories();
                Repositories.User.ITestAttachmentRepository testattachmentrepo = this.currentdomaindb.TestAttachmentRepository();
                Repositories.User.ITestAttachmentTypeRepository testattachmenttyperepo = this.currentdomaindb.TestAttachmentTypeRepository();
                Repositories.User.ITestInvestigationRepository testinvestigationrepo = this.currentdomaindb.TestInvestigationRepository();
                Repositories.User.ITestConclusionRepository testconclusionrepo = this.currentdomaindb.TestConclusionRepository();
                Repositories.User.ITestReportTypeRepository testreporttyperepo = this.currentdomaindb.TestReportTypeRepository();
                Repositories.User.IExtraWorkRequestReposotory extraworkrequestrepo = this.currentdomaindb.ExtraWorkRequestRepository();
                Repositories.User.IExtraWorkAttachmentRepository extraworkattachmentrepo = this.currentdomaindb.ExtraWorkAttachmentRepository();
                Repositories.User.IExtraWorkRequestStatusRepository extraworkstatusrepo = this.currentdomaindb.ExtraWorkRequestStatusRepository();
                Repositories.User.ITestSupplementReportRepository testsupplementrepo = this.currentdomaindb.TestSupplementReportRepository();
                Repositories.User.IHospitalDetail hospitalDetail = this.currentdomaindb.HospitalDetailRepository();
                int hospitalid = MySession.GetClientSession(this.subdomainurl).HospitalDetailID.HasValue ? MySession.GetClientSession(this.subdomainurl).HospitalDetailID.Value : 0;




                if (testid > 0)
                {

                    var testinnerobj = testrep.GetByID(testid);


                    if (testinnerobj != null)
                    {
                        int attachmenttypeid = 0;
                        var patientobj = (hospitalid != 0)?(from cl in clientuser.GetAll()
                                          join pd in patientdetailrepo.GetAll() on cl.DetailID equals pd.PatientDetailID
                                          join hs in hospitalDetail.GetAll() on pd.HospitalID equals hs.HospitalDetailID 
                                          into hspd from m in hspd.DefaultIfEmpty()
                                          where cl.ClientUserID == testinnerobj.PatientUserID.Value &&
                                           pd.HospitalID == hospitalid 
                                          select new { cl, pd,m }).FirstOrDefault()
                                          : (from cl in clientuser.GetAll()
                                             join pd in patientdetailrepo.GetAll() on cl.DetailID equals pd.PatientDetailID
                                             join hs in hospitalDetail.GetAll() on pd.HospitalID equals hs.HospitalDetailID
                                             into hspd from m in hspd.DefaultIfEmpty()
                                             where cl.ClientUserID == testinnerobj.PatientUserID.Value 
                                             select new { cl, pd, m }).FirstOrDefault();                              
                        var createdby = clientuser.GetByID(testinnerobj.TestCreatedBy.Value);
                        string createdbycustom = createdby.FirstName + " " + createdby.LastName;
                        attachmenttypeid = testattachmenttyperepo.GetAll().Where(x => x.Name.Equals("Test Creation")).FirstOrDefault().TestAttachmentTypeID;
                        var attachmentlist_testcreation = testattachmentrepo.GetAll().Where(x => x.AttachmentTypeID.Value == attachmenttypeid &&
                            x.TestID.Value == testinnerobj.TestID).ToList();
                        testobj = new
                        {
                            PatientID = patientobj == null ? 0 : patientobj.cl.ClientUserID,
                            PatientName = patientobj == null ? "" : patientobj.cl.FirstName + " " + (patientobj.pd.MiddleName == null ? "" : patientobj.pd.MiddleName) + patientobj.cl.LastName,
                            City = patientobj == null ? "" : patientobj.pd.City,
                            MobileNo = patientobj == null ? "" : patientobj.cl.MobileNo,
                            Age = patientobj == null ? "" : patientobj.pd.Age,
                            Gender = patientobj == null ? "" : patientobj.pd.Sex,
                            TestStatusID = testinnerobj.TestStatusID.HasValue ? testinnerobj.TestStatusID.Value : 0,
                            TestStatusName = testinnerobj.TestStatusID.HasValue ? teststatusrep.GetByID(testinnerobj.TestStatusID.Value).StatusName : "",
                            TestName = testinnerobj.TestName,
                            IsSampleRequired = testinnerobj.IsSampleRequired.HasValue ? (testinnerobj.IsSampleRequired.Value ? "Yes" : "No") : ("Yes"),
                            ComplaintHistory = testinnerobj.ComplaintHistory,
                            Description = testinnerobj.Description,
                            TestCreatedDateCustom = testinnerobj.TestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                            TestCreatedByCustom = createdbycustom,
                            testinnerobj.TestID,
                            testinnerobj.IsPublish,
                            AttachmentList = attachmentlist_testcreation,
                            HospitalName = (patientobj.pd.HospitalID != null) ? patientobj.m.HospitalName : patientobj.pd.ReferingHospital
                        };

                        if (testinnerobj.SampleCollectionBy.HasValue)
                        {
                            createdby = clientuser.GetByID(testinnerobj.SampleCollectionBy.Value);
                            createdbycustom = createdby.FirstName + " " + createdby.LastName;

                            attachmenttypeid = testattachmenttyperepo.GetAll().Where(x => x.Name.Equals("Sample Collection")).FirstOrDefault().TestAttachmentTypeID;
                            var attachmentlist_samplecollection = testattachmentrepo.GetAll().Where(x => x.AttachmentTypeID.Value == attachmenttypeid &&
                                x.TestID.Value == testinnerobj.TestID).ToList();

                            samplecollectionobj = new
                            {
                                IsSampleCollected = testinnerobj.IsSampleCollected.HasValue ? (testinnerobj.IsSampleCollected.Value ? "Yes" : "No") : ("Yes"),
                                SampleLabel = testinnerobj.SampleLabel,
                                SampleCode = testinnerobj.SampleCode,
                                SampleType = testinnerobj.SampleType,
                                AttachmentList = attachmentlist_samplecollection,
                                SampleCollectedDateCustom = testinnerobj.SampleCollectionDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                                SampleCollectedBy = createdbycustom
                            };
                        }

                        if (testinnerobj.AnalysisBy.HasValue)
                        {
                            createdby = clientuser.GetByID(testinnerobj.AnalysisBy.Value);
                            createdbycustom = createdby.FirstName + " " + createdby.LastName;

                            attachmenttypeid = testattachmenttyperepo.GetAll().Where(x => x.Name.Equals("Analysis Detail")).FirstOrDefault().TestAttachmentTypeID;
                            var attachmentlist_analysis = testattachmentrepo.GetAll().Where(x => x.AttachmentTypeID.Value == attachmenttypeid &&
                                x.TestID.Value == testinnerobj.TestID).ToList();

                            int extraworkstatusid = extraworkstatusrepo.GetAll().Where(x => x.StatusName.Equals("Completed")).FirstOrDefault().WorkRequestStatusID;

                            var investigations = (from inv in testinvestigationrepo.GetAll()
                                                  join ts in testrep.GetAll() on inv.TestID equals ts.TestID
                                                  where inv.ExtraWorkID.HasValue == false && ts.TestID == testinnerobj.TestID
                                                  select inv).ToList();
                            var extrawork = (from ex in extraworkrequestrepo.GetAll()
                                             where ex.TestID == testinnerobj.TestID && ex.StatusID.Value == extraworkstatusid
                                             select ex).ToList();

                            List<ExtraWorkRequest> extraworkrequestlist = new List<ExtraWorkRequest>();
                            ExtraWorkRequest extraworkrequest;
                            foreach (var extra in extrawork)
                            {
                                createdby = clientuser.GetByID(extra.PendingActionBy.HasValue ? extra.PendingActionBy.Value : extra.NewActionBy.Value);
                                createdbycustom = createdby.FirstName + " " + createdby.LastName;

                                extraworkrequest = new ExtraWorkRequest();
                                extraworkrequest.ExtraWorkID = extra.ExtraWorkID;
                                extraworkrequest.TestID = extra.TestID;
                                extraworkrequest.H_ELevels = extra.H_ELevels;
                                extraworkrequest.SpecialStains = extra.SpecialStains;
                                extraworkrequest.ImmunoHistoChemistry = extra.ImmunoHistoChemistry;
                                extraworkrequest.Others = extra.Others;
                                extraworkrequest.StatusID = extra.StatusID;
                                extraworkrequest.RequestCreatedDate = extra.RequestCreatedDate;
                                extraworkrequest.RequestCreatedBy = extra.RequestCreatedBy;
                                extraworkrequest.NewActionDate = extra.NewActionDate;
                                extraworkrequest.NewActionBy = extra.NewActionBy;
                                extraworkrequest.NewActionStatusID = extra.NewActionStatusID;
                                extraworkrequest.NewActionComments = extra.NewActionComments;
                                extraworkrequest.PendingActionDate = extra.PendingActionDate;
                                extraworkrequest.PendingActionBy = extra.PendingActionBy;
                                extraworkrequest.PendingActionComments = extra.PendingActionComments;
                                extraworkrequest.Investigations = testinvestigationrepo.GetAll().Where(x => (x.ExtraWorkID.HasValue ? x.ExtraWorkID.Value : 0) == extra.ExtraWorkID).ToList();
                                extraworkrequest.AttachmentList = extraworkattachmentrepo.GetAll().Where(x => (x.ExtraWorkID.HasValue ? x.ExtraWorkID.Value : 0) == extra.ExtraWorkID).ToList();
                                extraworkrequest.CompletedBy = createdbycustom;
                                extraworkrequest.CompletedDateCustom = extra.PendingActionDate.HasValue ? (extra.PendingActionDate.Value.ToString("MM/dd/yyyy HH:mm tt")) : (extra.NewActionDate.Value.ToString("MM/dd/yyyy HH:mm tt"));

                                extraworkrequestlist.Add(extraworkrequest);
                            }

                            investigationobj = new
                            {
                                InvestigationList = investigations,
                                AttachmentList = attachmentlist_analysis,
                                ExtraWorkRequestList = extraworkrequestlist,
                                AnalysisDateCustom = testinnerobj.AnalysisDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                                AnalysisBy = createdbycustom
                            };

                        }


                        if (testinnerobj.ConclusionBy.HasValue)
                        {
                            createdby = clientuser.GetByID(testinnerobj.ConclusionBy.Value);
                            createdbycustom = createdby.FirstName + " " + createdby.LastName;

                            attachmenttypeid = testattachmenttyperepo.GetAll().Where(x => x.Name.Equals("Report Conclusion")).FirstOrDefault().TestAttachmentTypeID;
                            var attachmentlist_conclusion = testattachmentrepo.GetAll().Where(x => x.AttachmentTypeID.Value == attachmenttypeid &&
                                x.TestID.Value == testinnerobj.TestID).ToList();

                            var conclusioninner = testconclusionrepo.GetAll().Where(x => x.TestID.Value == testinnerobj.TestID).FirstOrDefault();

                            string _reporttype = string.Empty;
                            if (conclusioninner != null)
                            {
                                if (conclusioninner.TestReportTypeID.HasValue)
                                {
                                    var rp_type = testreporttyperepo.GetByID(conclusioninner.TestReportTypeID.Value);
                                    if (rp_type != null)
                                    {
                                        _reporttype = rp_type.Name;
                                    }
                                }


                                conclusionobj = new
                                {
                                    conclusioninner.TestConclusionID,
                                    TestReportTypeID = conclusioninner.TestReportTypeID.HasValue ? conclusioninner.TestReportTypeID.Value : 0,
                                    TestID = conclusioninner.TestID.HasValue ? conclusioninner.TestID.Value : 0,
                                    conclusioninner.SpecimenDetails,
                                    conclusioninner.ClinicalDetails,
                                    conclusioninner.Microscopy,
                                    conclusioninner.Macroscopy,
                                    conclusioninner.Conclusion,
                                    conclusioninner.SnomedCoding,
                                    conclusioninner.SampleDescription,
                                    conclusioninner.Report,
                                    ConclusionBy = createdbycustom,
                                    ConclusionDateCustom = testinnerobj.ConclusionDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                                    TestReportType = _reporttype,
                                    AttachmentList = attachmentlist_conclusion,
                                };
                            }
                        }


                        List<TestSupplementReport> supplementreportlist = new List<TestSupplementReport>();
                        var supplementreports = testsupplementrepo.GetAll().Where(x => (x.TestID.HasValue ? x.TestID.Value : 0) == testid).ToList();
                        if (supplementreports.Count > 0)
                        {
                            attachmenttypeid = testattachmenttyperepo.GetAll().Where(x => x.Name.Equals("Supplementary Report")).FirstOrDefault().TestAttachmentTypeID;
                            TestSupplementReport innersup = null;
                            string _reporttype = string.Empty;
                            BusinessPOCO.User.Cl_TestReportType reporttype = null;
                            foreach (var supp in supplementreports)
                            {
                                innersup = new TestSupplementReport();
                                if (supp.TestReportTypeID.HasValue)
                                {
                                    reporttype = testreporttyperepo.GetByID(supp.TestReportTypeID.Value);
                                    if (reporttype != null)
                                    {
                                        _reporttype = reporttype.Name;
                                        reporttype = null;
                                    }
                                    else
                                    {
                                        _reporttype = "";
                                    }
                                }

                                createdby = clientuser.GetByID(supp.CreatedBy.Value);
                                createdbycustom = createdby.FirstName + " " + createdby.LastName;

                                innersup.TestSupplementReportID = supp.TestSupplementReportID;
                                innersup.TestReportTypeID = supp.TestReportTypeID.HasValue ? supp.TestReportTypeID.Value : 0;
                                innersup.TestID = supp.TestID.HasValue ? supp.TestID.Value : 0;
                                innersup.IsPublished = supp.IsPublished.Value;
                                innersup.SpecimenDetails = supp.SpecimenDetails;
                                innersup.ClinicalDetails = supp.ClinicalDetails;
                                innersup.Microscopy = supp.Microscopy;
                                innersup.Macroscopy = supp.Macroscopy;
                                innersup.SupplementReportConclusion = supp.SupplementReportConclusion;
                                innersup.SnomedCoding = supp.SnomedCoding;
                                innersup.SampleDescription = supp.SampleDescription;
                                innersup.Report = supp.Report;
                                innersup.SupplementBy = createdbycustom;
                                innersup.SupplementDate = supp.CreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt");
                                innersup.TestReportType = _reporttype;
                                innersup.AttachmentList = testattachmentrepo.GetAll().Where(x => x.AttachmentTypeID.Value == attachmenttypeid &&
                                x.TestID.Value == testinnerobj.TestID && (x.CreatedBy.HasValue ? x.CreatedBy.Value : 0) == supp.TestSupplementReportID).ToList();

                                supplementreportlist.Add(innersup);

                            }
                        }


                        return WebJSResponse.ResponseSimple(new { testobjjson = testobj, samplecollectionobjjson = samplecollectionobj, investigationobjjson = investigationobj, conclusionobjjson = conclusionobj, supplementreportlistjson = supplementreportlist });
                    }

                    else
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "No test found !", "There is no test associated to this test id.", new { testobjjson = testobj, samplecollectionobjjson = samplecollectionobj, investigationobjjson = investigationobj, conclusionobjjson = conclusionobj });
                    }


                }

                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Incorrect test-id !", "Kindly provide correct test id.", new { testobjjson = testobj, samplecollectionobjjson = samplecollectionobj, investigationobjjson = investigationobj, conclusionobjjson = conclusionobj });


            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { testobjjson = testobj, samplecollectionobjjson = samplecollectionobj, investigationobjjson = investigationobj, conclusionobjjson = conclusionobj });
            }
        }

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
                int hospitalid = 0;

                if (status.Equals("Open"))
                {
                    if (MySession.GetClientSession(this.subdomainurl).CurrentRole.RoleName.Equals("Receptionist") || MySession.GetClientSession(this.subdomainurl).CurrentRole.RoleName.Equals("Hospital / Clinician"))
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
                    else if (MySession.GetClientSession(this.subdomainurl).CurrentRole.RoleName.Equals("Clinical Laboratory Consultant") || MySession.GetClientSession(this.subdomainurl).CurrentRole.RoleName.Equals("Secretary") || MySession.GetClientSession(this.subdomainurl).CurrentRole.RoleName.Equals("External Consultant"))
                    {
                        teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("On Clinical Lab Doctor Plate")).FirstOrDefault().TestStatusID;
                    }


                }
                else if (status.Equals("Completed"))
                {

                    teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("Completed")).FirstOrDefault().TestStatusID;
                    hospitalid = MySession.GetClientSession(this.subdomainurl).HospitalDetailID.HasValue ? MySession.GetClientSession(this.subdomainurl).HospitalDetailID.Value : 0;
                }

                if (MySession.GetClientSession(this.subdomainurl).CurrentRole.RoleName == "External Consultant")
                {
                    if (status == "Completed")
                    {
                        var result = (from ts in testrep.GetAll()
                                      join ptus in clientuser.GetAll() on ts.PatientUserID equals ptus.ClientUserID
                                      join pt in patientdetailrepo.GetAll() on ptus.DetailID equals pt.PatientDetailID
                                      where (ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == teststatusid
                                      select new
                                      {
                                          ts.TestID,
                                          ts.TestName,
                                          PatientName = (ptus.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + ptus.LastName),
                                          Status = (ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == 5 ? "Completed" : "Pending",
                                          TestCreatedDateCustom = ts.TestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                                          City = pt.City,
                                          IsPublish = ts.IsPublish.HasValue ? ts.IsPublish.Value ? "Yes" : "No" : "No"
                                      }).ToList();
                        return WebJSResponse.ResponseSimple(new { testjson = result });
                    }
                    else
                    {
                        var result = (from ts in testrep.GetAll()
                                      join ptus in clientuser.GetAll() on ts.PatientUserID equals ptus.ClientUserID
                                      join pt in patientdetailrepo.GetAll() on ptus.DetailID equals pt.PatientDetailID
                                      where (ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == teststatusid && ts.ConclusionBy == MySession.GetClientSession(this.subdomainurl).ClientUserID
                                      select new
                                      {
                                          ts.TestID,
                                          ts.TestName,
                                          PatientName = (ptus.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + ptus.LastName),
                                          Status = (ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == 5 ? "Completed" : "Pending",
                                          TestCreatedDateCustom = ts.TestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                                          City = pt.City,
                                          IsPublish = ts.IsPublish.HasValue ? ts.IsPublish.Value ? "Yes" : "No" : "No"
                                      }).ToList();
                        return WebJSResponse.ResponseSimple(new { testjson = result });
                    }
                }

                if (!MySession.GetClientSession(this.subdomainurl).HospitalDetailID.HasValue)
                {

                    var result = (from ts in testrep.GetAll()
                                  join ptus in clientuser.GetAll() on ts.PatientUserID equals ptus.ClientUserID
                                  join pt in patientdetailrepo.GetAll() on ptus.DetailID equals pt.PatientDetailID
                                  where (ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == teststatusid
                                  select new
                                  {
                                      ts.TestID,
                                      ts.TestName,
                                      PatientName = (ptus.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + ptus.LastName),
                                      Status = (ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == 5 ? "Completed" : "Pending",
                                      TestCreatedDateCustom = ts.TestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                                      City = pt.City,
                                      IsPublish = ts.IsPublish.HasValue ? ts.IsPublish.Value ? "Yes" : "No" : "No"
                                  }).ToList();
                    return WebJSResponse.ResponseSimple(new { testjson = result });

                }

                else
                {
                    var result = (from ts in testrep.GetAll()
                                  join ptus in clientuser.GetAll() on ts.PatientUserID equals ptus.ClientUserID
                                  join pt in patientdetailrepo.GetAll() on ptus.DetailID equals pt.PatientDetailID
                                  where pt.HospitalID == hospitalid
                                  where (ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == teststatusid
                                  select new
                                  {
                                      ts.TestID,
                                      ts.TestName,
                                      PatientName = (ptus.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + ptus.LastName),
                                      Status = (ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == 5 ? "Completed" : "Pending",
                                      TestCreatedDateCustom = ts.TestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                                      City = pt.City,
                                      IsPublish = ts.IsPublish.HasValue ? ts.IsPublish.Value ? "Yes" : "No" : "No"
                                  }).ToList();
                    return WebJSResponse.ResponseSimple(new { testjson = result });
                }



            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("Completed")]
        [ClientAuthorizeMember]
        //[SystemAuthorizeMember]
        [HttpGet]
        public ActionResult Completed()
        {
            ViewBag.currentdomaindb = this.currentdomaindb;
            ViewBag.subdomainurl = this.subdomainurl;
            return View("~/Views/User/MedicalTest/Completed.cshtml");
        }
        [Route("Open")]
        [ClientAuthorizeMember]
        // [SystemAuthorizeMember]
        [HttpGet]
        public ActionResult Open()
        {
            return View("~/Views/User/MedicalTest/Open.cshtml");
        }
        [Route("CompletedReport")]
        [ClientAuthorizeMember]
        //[SystemAuthorizeMember]
        [HttpGet]
        public ActionResult CompletedReport()
        {
            ViewBag.currentdomaindb = this.currentdomaindb;
            ViewBag.subdomainurl = this.subdomainurl;
            return View("~/Views/User/MedicalTest/CompletedReport.cshtml");
        }

        [Route("SearchCompletedReport")]
        [ClientAuthorizeMember]
        //[SystemAuthorizeMember]
        [HttpGet]
        public ActionResult SearchCompletedReport()
        {
            ViewBag.currentdomaindb = this.currentdomaindb;
            ViewBag.subdomainurl = this.subdomainurl;
            return View("~/Views/User/MedicalTest/SearchCompletedReport.cshtml");
        }

        [Route("ViewReport")]
        [ClientAuthorizeMember]
        // [SystemAuthorizeMember]
        [HttpPost]
        public ActionResult ViewReport(int Id)
        {

            object testobj = null;
            object conclusionobj = null;
            dynamic mymodel = new ExpandoObject();
            var data = new ReportModel();
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IPatientDetailRepository patientdetailrepo = this.currentdomaindb.PatientDetailRepository();
                Repositories.User.ITestRepository testrep = this.currentdomaindb.TestRepository();
                Repositories.User.ITestStatusRepositories teststatusrep = this.currentdomaindb.TestStatusRepositories();
                Repositories.User.ITestAttachmentRepository testattachmentrepo = this.currentdomaindb.TestAttachmentRepository();
                Repositories.User.ITestAttachmentTypeRepository testattachmenttyperepo = this.currentdomaindb.TestAttachmentTypeRepository();
                Repositories.User.ITestInvestigationRepository testinvestigationrepo = this.currentdomaindb.TestInvestigationRepository();
                Repositories.User.ITestConclusionRepository testconclusionrepo = this.currentdomaindb.TestConclusionRepository();
                Repositories.User.ITestReportTypeRepository testreporttyperepo = this.currentdomaindb.TestReportTypeRepository();
                Repositories.User.IExtraWorkRequestReposotory extraworkrequestrepo = this.currentdomaindb.ExtraWorkRequestRepository();
                Repositories.User.IExtraWorkAttachmentRepository extraworkattachmentrepo = this.currentdomaindb.ExtraWorkAttachmentRepository();
                Repositories.User.IExtraWorkRequestStatusRepository extraworkstatusrepo = this.currentdomaindb.ExtraWorkRequestStatusRepository();
                Repositories.User.ITestSupplementReportRepository testsupplementrepo = this.currentdomaindb.TestSupplementReportRepository();
                Repositories.User.ILabReportConfiguration labReportConfiguration = this.currentdomaindb.LabReportConfigurationRepository();


                if (Id > 0)
                {

                    var testinnerobj = testrep.GetByID(Id);


                    if (testinnerobj != null)
                    {
                        //int attachmenttypeid = 0;
                        var patientobj = (from cl in clientuser.GetAll()
                                          join pd in patientdetailrepo.GetAll() on cl.DetailID equals pd.PatientDetailID
                                          where cl.ClientUserID == testinnerobj.PatientUserID.Value
                                          select new { cl, pd }).FirstOrDefault();
                        var createdby = clientuser.GetByID(testinnerobj.TestCreatedBy.Value);
                        string createdbycustom = createdby.FirstName + " " + createdby.LastName;
                        //testobj = new
                        //{
                        //    PatientID = patientobj == null ? 0 : patientobj.cl.ClientUserID,
                        //    PatientName = patientobj == null ? "" : patientobj.cl.FirstName + " " + (patientobj.pd.MiddleName == null ? "" : patientobj.pd.MiddleName) + patientobj.cl.LastName,
                        //    City = patientobj == null ? "" : patientobj.pd.City,
                        //    MobileNo = patientobj == null ? "" : patientobj.cl.MobileNo,
                        //    Age = patientobj == null ? "" : patientobj.pd.Age,
                        //    Gender = patientobj == null ? "" : patientobj.pd.Sex,
                        //    TestStatusID = testinnerobj.TestStatusID.HasValue ? testinnerobj.TestStatusID.Value : 0,
                        //    TestStatusName = testinnerobj.TestStatusID.HasValue ? teststatusrep.GetByID(testinnerobj.TestStatusID.Value).StatusName : "",
                        //    TestName = testinnerobj.TestName,
                        //    IsSampleRequired = testinnerobj.IsSampleRequired.HasValue ? (testinnerobj.IsSampleRequired.Value ? "Yes" : "No") : ("Yes"),
                        //    ComplaintHistory = testinnerobj.ComplaintHistory,
                        //    Description = testinnerobj.Description,
                        //    TestCreatedDateCustom = testinnerobj.TestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                        //    TestCreatedByCustom = createdbycustom,
                        //    testinnerobj.TestID,
                        //    testinnerobj.IsPublish
                        //};
                        var labconfigobj = (from labobj in labReportConfiguration.GetAll()
                                            select new { labobj }).FirstOrDefault();
                        data.LabName = labconfigobj.labobj.LabName;
                        data.LabAddress = labconfigobj.labobj.LabAddress;
                        data.LabCompanyNumber = labconfigobj.labobj.LabCompanyNumber;
                        data.LabUniqueCode = labconfigobj.labobj.LabUniqueCode;
                        data.LabHeadOfficeAddress = labconfigobj.labobj.LabHeadOfficeAddress;
                        data.labEmail = labconfigobj.labobj.labEmail;

                        data.Name = patientobj.cl.FirstName + " " + (patientobj.pd.MiddleName == null ? "" : patientobj.pd.MiddleName) + patientobj.cl.LastName;
                        data.Gender = patientobj.pd.Sex;
                        data.DOB = patientobj.pd.City;
                        data.Address = patientobj.cl.Address;
                        data.Referring_hospital_name = patientobj.pd.ReferingHospital;
                        data.Referring_lab_no = patientobj.pd.ReferingDoctor;
                        data.Report_number = testinnerobj.TestID.ToString();
                        data.NHS_report_number = testinnerobj.SampleLabel.ToString();// ; "N/A";
                        //data.reported_by = createdbycustom;

                        if (testinnerobj.ConclusionBy.HasValue)
                        {
                            createdby = clientuser.GetByID(testinnerobj.ConclusionBy.Value);
                            createdbycustom = createdby.FirstName + " " + createdby.LastName;

                            //attachmenttypeid = testattachmenttyperepo.GetAll().Where(x => x.Name.Equals("Report Conclusion")).FirstOrDefault().TestAttachmentTypeID;
                            //var attachmentlist_conclusion = testattachmentrepo.GetAll().Where(x => x.AttachmentTypeID.Value == attachmenttypeid &&
                            //x.TestID.Value == testinnerobj.TestID).ToList();

                            var conclusioninner = testconclusionrepo.GetAll().Where(x => x.TestID.Value == testinnerobj.TestID).FirstOrDefault();

                            string _reporttype = string.Empty;
                            if (conclusioninner.TestReportTypeID.HasValue)
                            {
                                var rp_type = testreporttyperepo.GetByID(conclusioninner.TestReportTypeID.Value);
                                if (rp_type != null)
                                {
                                    _reporttype = rp_type.Name;
                                }
                            }

                            conclusionobj = new
                            {
                                conclusioninner.TestConclusionID,
                                TestReportTypeID = conclusioninner.TestReportTypeID.HasValue ? conclusioninner.TestReportTypeID.Value : 0,
                                TestID = conclusioninner.TestID.HasValue ? conclusioninner.TestID.Value : 0,
                                conclusioninner.SpecimenDetails,
                                conclusioninner.ClinicalDetails,
                                conclusioninner.Microscopy,
                                conclusioninner.Macroscopy,
                                conclusioninner.Conclusion,
                                conclusioninner.SnomedCoding,
                                conclusioninner.SampleDescription,
                                conclusioninner.Report,
                                ConclusionBy = createdbycustom,
                                ConclusionDateCustom = testinnerobj.ConclusionDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                                TestReportType = _reporttype
                                //AttachmentList = attachmentlist_conclusion,
                            };
                            data.Specimen_type = conclusioninner.SpecimenDetails;
                            data.Clinical_details = conclusioninner.ClinicalDetails;
                            data.Macroscopy = conclusioninner.Macroscopy;
                            data.Microscopy = conclusioninner.Microscopy;
                            data.Conclusion = conclusioninner.Conclusion;
                            data.reported_by = createdbycustom;
                            data.SampleDescription = conclusioninner.SampleDescription;
                            data.Report = conclusioninner.Report;
                            data.TestReportTypeID = conclusioninner.TestReportTypeID.HasValue ? conclusioninner.TestReportTypeID.Value : 0;
                            mymodel.data = data;
                            List<TestSupplementReport> supplementreportlist = new List<TestSupplementReport>();
                            var supplementreports = testsupplementrepo.GetAll().Where(x => (x.TestID.HasValue ? x.TestID.Value : 0) == Id).ToList();
                            if (supplementreports.Count > 0)
                            {
                                // attachmenttypeid = testattachmenttyperepo.GetAll().Where(x => x.Name.Equals("Supplementary Report")).FirstOrDefault().TestAttachmentTypeID;
                                TestSupplementReport innersup = null;
                                //string _reporttype = string.Empty;
                                BusinessPOCO.User.Cl_TestReportType reporttype = null;
                                foreach (var supp in supplementreports)
                                {
                                    innersup = new TestSupplementReport();
                                    if (supp.TestReportTypeID.HasValue)
                                    {
                                        reporttype = testreporttyperepo.GetByID(supp.TestReportTypeID.Value);
                                        if (reporttype != null)
                                        {
                                            _reporttype = reporttype.Name;
                                            reporttype = null;
                                        }
                                        else
                                        {
                                            _reporttype = "";
                                        }
                                    }

                                    createdby = clientuser.GetByID(supp.CreatedBy.Value);
                                    createdbycustom = createdby.FirstName + " " + createdby.LastName;

                                    innersup.TestSupplementReportID = supp.TestSupplementReportID;
                                    innersup.TestReportTypeID = supp.TestReportTypeID.HasValue ? supp.TestReportTypeID.Value : 0;
                                    innersup.TestID = supp.TestID.HasValue ? supp.TestID.Value : 0;
                                    innersup.SpecimenDetails = supp.SpecimenDetails;
                                    innersup.ClinicalDetails = supp.ClinicalDetails;
                                    innersup.Microscopy = supp.Microscopy;
                                    innersup.Macroscopy = supp.Macroscopy;
                                    innersup.SupplementReportConclusion = supp.SupplementReportConclusion;
                                    innersup.SnomedCoding = supp.SnomedCoding;
                                    innersup.SampleDescription = supp.SampleDescription;
                                    innersup.Report = supp.Report;
                                    innersup.SupplementBy = createdbycustom;
                                    innersup.SupplementDate = supp.CreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt");
                                    innersup.TestReportType = _reporttype;
                                    //innersup.AttachmentList = testattachmentrepo.GetAll().Where(x => x.AttachmentTypeID.Value == attachmenttypeid &&
                                    //  x.TestID.Value == testinnerobj.TestID && (x.CreatedBy.HasValue ? x.CreatedBy.Value : 0) == supp.TestSupplementReportID).ToList();

                                    supplementreportlist.Add(innersup);
                                }
                            }
                            mymodel.supplementreportlist = supplementreportlist;
                        }




                        return PartialView("~/Views/User/MedicalTest/ViewReport.cshtml", mymodel);



                        // return WebJSResponse.ResponseSimple(new { testobjjson = testobj, samplecollectionobjjson = samplecollectionobj, investigationobjjson = investigationobj, conclusionobjjson = conclusionobj, supplementreportlistjson = supplementreportlist });
                    }


                    return PartialView("~/Views/User/MedicalTest/ViewReport.cshtml", data);

                }

                return PartialView("~/Views/User/MedicalTest/ViewReport.cshtml", data);
                //var data = new ReportModel();

                //return PartialView("~/Views/User/MedicalTest/ViewReport.cshtml", data);

                //return WebJSResponse.ResponseToastr(ToastrEnum.error, "Incorrect test-id !", "Kindly provide correct test id.", new { testobjjson = testobj, samplecollectionobjjson = samplecollectionobj, investigationobjjson = investigationobj, conclusionobjjson = conclusionobj });


            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { testobjjson = testobj, conclusionobjjson = conclusionobj });
            }

            //return View("~/Views/User/MedicalTest/ViewReport.cshtml");
        }


    }
}