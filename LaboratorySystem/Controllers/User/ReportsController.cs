using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repositories = LaboratoryBusiness.Repositories;
using BLL = LaboratoryBusiness.BLL;
using BusinessPOCO = LaboratoryBusiness.POCO;
using LaboratorySystem.Models;
using System.Dynamic;

namespace LaboratorySystem.Controllers.User
{
    [RoutePrefix("User/Reports")]
    public class ReportsController : RedirectController
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
                object result = new {};

                if (!status.Equals("Open"))
                {
                    result = (from ts in testrep.GetAll()
                              join ptus in clientuser.GetAll() on ts.PatientUserID equals ptus.ClientUserID
                              join pt in patientdetailrepo.GetAll() on ptus.DetailID equals pt.PatientDetailID
                              join dst in teststatusrep.GetAll() on ts.TestStatusID equals dst.TestStatusID
                              where (((ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == teststatusid) || teststatusid == -1)
                              select new
                              {
                                  ts.TestID,
                                  ts.TestName,
                                  PatientName = (ptus.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + ptus.LastName),
                                  Status = (ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == 5 ? "Completed" : "Pending",
                                  TestCreatedDateCustom = ts.TestCreatedDate.Value.ToString("dd/MM/yyyy HH:mm tt"),
                                  IsPublish = ts.IsPublish.HasValue ? ts.IsPublish.Value ? "Yes" : "No" : "No",
                                  City=pt.City,
                                  DetailStatus = dst.StatusName

                              }).ToList();
                }
                else
                {
                    result = (from ts in testrep.GetAll()
                              join ptus in clientuser.GetAll() on ts.PatientUserID equals ptus.ClientUserID
                              join pt in patientdetailrepo.GetAll() on ptus.DetailID equals pt.PatientDetailID
                              join dst in teststatusrep.GetAll() on ts.TestStatusID equals dst.TestStatusID
                              where (((ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) != teststatusid))
                              select new
                              {
                                  ts.TestID,
                                  ts.TestName,
                                  PatientName = (ptus.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + ptus.LastName),
                                  Status = (ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == 5 ? "Completed" : "Pending",
                                  TestCreatedDateCustom = ts.TestCreatedDate.Value.ToString("dd/MM/yyyy HH:mm tt"),
                                  IsPublish = ts.IsPublish.HasValue ? ts.IsPublish.Value ? "Yes" : "No" : "No",
                                  DetailStatus = dst.StatusName,
                                  City = pt.City,

                              }).ToList();
                }

                return WebJSResponse.ResponseSimple(new { testjson = result });


            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        // GET: Reports
         [Route("AllBookedTests")]
         [ClientAuthorizeMember]
        public ActionResult AllBookedTests()
        {
            return View("~/Views/User/Reports/AllBookedTests.cshtml");
        }
        [Route("CompletedTests")]
        [ClientAuthorizeMember]
       // [SystemAuthorizeMember]
        [HttpGet]
        public ActionResult CompletedTests()
        {
            ViewBag.currentdomaindb = this.currentdomaindb;
            ViewBag.subdomainurl = this.subdomainurl;
            return View("~/Views/User/Reports/CompletedTests.cshtml");
        }
        [Route("PendingTests")]
        [ClientAuthorizeMember]
      //  [SystemAuthorizeMember]
        [HttpGet]

        public ActionResult PendingTests()
        {
            return View("~/Views/User/Reports/PendingTests.cshtml");
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




                        return PartialView("~/Views/User/Reports/ViewReport.cshtml", mymodel);



                        // return WebJSResponse.ResponseSimple(new { testobjjson = testobj, samplecollectionobjjson = samplecollectionobj, investigationobjjson = investigationobj, conclusionobjjson = conclusionobj, supplementreportlistjson = supplementreportlist });
                    }


                    return PartialView("~/Views/User/Reports/ViewReport.cshtml", data);

                }

                return PartialView("~/Views/User/Reports/ViewReport.cshtml", data);
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