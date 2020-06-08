using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repositories = LaboratoryBusiness.Repositories;
using BLL = LaboratoryBusiness.BLL;
using BusinessPOCO = LaboratoryBusiness.POCO;

namespace LaboratorySystem
{
    public class PdfCreator
    {
        public static FileManager CreatePatientPDF(int patientuserid,string subdomainurl,DBInitializer db, out string link)
        {
            try
            {
                LaboratoryBusiness.Repositories.User.IClientUserRepository cluser = db.ClientUserRepository();
                LaboratoryBusiness.Repositories.User.IPatientDetailRepository patientdetail = db.PatientDetailRepository();
                Repositories.User.IAdminDetailRepository adminrepo = db.AdminDetailRepository();

                LaboratoryBusiness.POCO.User.Cl_ClientUser cuser = cluser.GetByID(patientuserid);
                LaboratoryBusiness.POCO.User.Cl_PatientDetail pdetail = patientdetail.GetByID(cuser.DetailID.Value);

                Repositories.Admin.ISystemUserRepository sysuserrepo = new LaboratoryBusiness.BLL.Admin.SystemUserRepository();
                Repositories.Admin.IClientRepository clientrepo = new LaboratoryBusiness.BLL.Admin.ClientRepository();

                int systemclientuserid = adminrepo.GetAll().FirstOrDefault().SystemClientID.Value;
                var clientdetail = sysuserrepo.GetByID(systemclientuserid);
                int clientdetailid = clientdetail.DetailID.Value;
                var clientobj = clientrepo.GetByID(clientdetailid);

                Dictionary<string, string> dic = new Dictionary<string, string>();
                //dic.Add("@patientID@", cuser.ClientUserID.ToString("000"));

                dic.Add("@ClientName@", clientobj.CompanyName);
                dic.Add("@ClientAddress@", clientdetail.Address);
                dic.Add("@ClientEmail@", clientdetail.Email);

                dic.Add("@firstname@", cuser.FirstName);
                dic.Add("@middlename@", pdetail.MiddleName);
                dic.Add("@lastname@", cuser.LastName);
                dic.Add("@email@", cuser.Email);
                dic.Add("@mobileno@", cuser.MobileNo);
                dic.Add("@username@", cuser.Username);
                dic.Add("@age@", pdetail.Age);
                dic.Add("@gender@", pdetail.Sex);
                dic.Add("@address@", cuser.Address);
                //dic.Add("@status@", cuser.IsActive.HasValue ? cuser.IsActive.Value ? "Active" : "False" : "False");
                dic.Add("@streetname@", pdetail.Streetname);
                dic.Add("@city@", pdetail.City);

                dic.Add("@alternateaddress@", pdetail.AlternateAddress);
                dic.Add("@alternatephoneno@", pdetail.AlternatePhoneNo);
                dic.Add("@referingdoctor@", pdetail.ReferingDoctor);
                dic.Add("@referinghospital@", pdetail.ReferingHospital);
                dic.Add("@paymentmode@", pdetail.PaymentMode);
                dic.Add("@payment@", pdetail.Payment.ToString());
                //dic.Add("@createddate@", pdetail.CreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"));

                string pdfhtmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\Patient\Patient.html", dic);

                FileManager pd = PdfManager.HtmlToPdf("PatientDetail_" + cuser.ClientUserID.ToString("000") + "_" + pdetail.CreatedDate.Value.ToString("MMddyyyyHHmmtt"), pdfhtmltemplate);
                Client cl = new Client(subdomainurl);
                FileInitializer fl = new FileInitializer(cl);
                string pdfuploadedpath;
                fl.UploadFile(pd, "\\PatientFiles", out pdfuploadedpath);
                link = pdfuploadedpath;
                return pd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static FileManager CreatePatientPaymentPDF(int patientuserid, string subdomainurl, DBInitializer db, out string link)
        {
            try
            {
                LaboratoryBusiness.Repositories.User.IClientUserRepository cluser = db.ClientUserRepository();
                LaboratoryBusiness.Repositories.User.IPatientDetailRepository patientdetail = db.PatientDetailRepository();
                Repositories.User.IAdminDetailRepository adminrepo = db.AdminDetailRepository();

                LaboratoryBusiness.POCO.User.Cl_ClientUser cuser = cluser.GetByID(patientuserid);
                LaboratoryBusiness.POCO.User.Cl_PatientDetail pdetail = patientdetail.GetByID(cuser.DetailID.Value);

                Repositories.Admin.ISystemUserRepository sysuserrepo = new LaboratoryBusiness.BLL.Admin.SystemUserRepository();
                Repositories.Admin.IClientRepository clientrepo = new LaboratoryBusiness.BLL.Admin.ClientRepository();

                int systemclientuserid = adminrepo.GetAll().FirstOrDefault().SystemClientID.Value;
                var clientdetail = sysuserrepo.GetByID(systemclientuserid);
                int clientdetailid = clientdetail.DetailID.Value;
                var clientobj = clientrepo.GetByID(clientdetailid);

                Dictionary<string, string> dic = new Dictionary<string, string>();
                //dic.Add("@patientid@", cuser.ClientUserID.ToString("000"));
                //dic.Add("@name@", cuser.FirstName + " " + pdetail.MiddleName + " " + cuser.LastName);
                //dic.Add("@address@", cuser.Address);
                //dic.Add("@streetname@", pdetail.Streetname);
                //dic.Add("@city@", pdetail.City);

                //dic.Add("@amount@", pdetail.Payment.ToString());
                //dic.Add("@date@", pdetail.CreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"));

                dic.Add("@ClientLabName@", clientobj.CompanyName);
                dic.Add("@ClientLabAdress@", clientdetail.Address);
                dic.Add("@Phone@", clientdetail.MobileNo);
                dic.Add("@ClientEmail@", clientdetail.Email);
                dic.Add("@PatientID@", cuser.ClientUserID.ToString("0000"));
                dic.Add("@username@", cuser.Username);
                dic.Add("@invoiceno@", pdetail.PatientDetailID.ToString("0000"));
                dic.Add("@invoicedate@", pdetail.CreatedDate.Value.ToString("dd MMM yyyy HH:mm tt"));
                dic.Add("@PatientName@", cuser.FirstName + " " + pdetail.MiddleName + " " + cuser.LastName);
                dic.Add("@PatientAddress@", cuser.Address);
                dic.Add("@PatientMobile@", cuser.MobileNo);
                dic.Add("@patientemail@", cuser.Email);
                dic.Add("@Amount@", pdetail.Payment.ToString());
                dic.Add("@TotalAmount@", pdetail.Payment.ToString());

                string pdfhtmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\Patient\PatientPayment.html", dic);

                FileManager pd = PdfManager.HtmlToPdf("PatientReceipt_" + cuser.ClientUserID.ToString("000") + "_" + pdetail.CreatedDate.Value.ToString("MMddyyyyHHmmtt"), pdfhtmltemplate);
                Client cl = new Client(subdomainurl);
                FileInitializer fl = new FileInitializer(cl);
                string pdfuploadedpath;
                fl.UploadFile(pd, "\\PatientFiles\\Receipt", out pdfuploadedpath);
                link = pdfuploadedpath;
                return pd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static FileManager CreateInvoicePDF(Models.InvoiceModel invoice, string subdomainurl, DBInitializer db, out string link)
        {
            try
            {


                Dictionary<string, string> dic = new Dictionary<string, string>();
                //Dictionary<string, string> dicP = new Dictionary<string, string>();
                dic.Add("@invoicedate@", invoice.InvoiceDate.ToString("dd/MM/yyyy"));
                dic.Add("@labname@", invoice.LabName);

                dic.Add("@total@", invoice.Total);
                dic.Add("@vat@", invoice.Vat);
                dic.Add("@vatcost@", invoice.VatCost);

                dic.Add("@hospitalname@", invoice.HospitalName);
                dic.Add("@hospitaladdress@", invoice.HospitalAddress);
                dic.Add("@duedate@", invoice.DueDate.ToString("dd/MM/yyyy"));
                string paitientnewrow = string.Empty;
                int sno = 1;
                foreach (var p in invoice.invoiceTestDatas)
                {
                    paitientnewrow += @"<tr>
                                        <td>" + sno + @"</td>
                                        <td>" + p.SampleCode + @"</td>
                                        <td>" + p.PaitientName + @"</td>
                                        <td>" + p.TestName + @"</td>
                                        <td>" + p.Cost + @"</td>
                                        </tr>";
                    sno++;
                    //dicP.Add("@samplecode@", p.SampleCode);
                    //dicP.Add("@paitientname@", p.PaitientName);
                    //dicP.Add("@testname@", p.TestName);
                    //dicP.Add("@cost@", p.Cost);
                    
                }
                dic.Add("@paitientnewrow@", paitientnewrow);//Html
                //dic.Append(dicP);
                dic.Add("@InvoiceNumber@", invoice.InvoiceID);
                dic.Add("@Subtotal@", invoice.SubTotal);
                //dic.Add("@Phone@", clientdetail.MobileNo);
                //dic.Add("@ClientEmail@", clientdetail.Email);
                //dic.Add("@PatientID@", cuser.ClientUserID.ToString("0000"));
                //dic.Add("@username@", cuser.Username);
                //dic.Add("@invoiceno@", pdetail.PatientDetailID.ToString("0000"));
                //dic.Add("@invoicedate@", pdetail.CreatedDate.Value.ToString("dd MMM yyyy HH:mm tt"));
                //dic.Add("@PatientName@", cuser.FirstName + " " + pdetail.MiddleName + " " + cuser.LastName);
                //dic.Add("@PatientAddress@", cuser.Address);
                //dic.Add("@PatientMobile@", cuser.MobileNo);
                //dic.Add("@patientemail@", cuser.Email);
                //dic.Add("@Amount@", pdetail.Payment.ToString());
                //dic.Add("@TotalAmount@", pdetail.Payment.ToString());

                string pdfhtmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\Invoice\Invoice.html", dic);

                FileManager pd = PdfManager.HtmlToPdf("Invoice" +invoice.InvoiceID + invoice.InvoiceDate.ToString("MMddyyyyHHmmtt"), pdfhtmltemplate);
                Client cl = new Client(subdomainurl);
                FileInitializer fl = new FileInitializer(cl);
                string pdfuploadedpath;
                fl.UploadFile(pd, "\\InvoiceReciept", out pdfuploadedpath);
                link = pdfuploadedpath;
                return pd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static FileManager CreateEmployeePDF(int employeeuserid, string subdomainurl, DBInitializer db, out string link)
        {
            try
            {
                LaboratoryBusiness.Repositories.User.IClientUserRepository cluser = db.ClientUserRepository();
                LaboratoryBusiness.Repositories.User.IClientUserDetailRepository clientuserdetail = db.ClientUserDetailRepository();
                LaboratoryBusiness.Repositories.User.IRoleRepository role = db.RoleRepository();
                LaboratoryBusiness.Repositories.User.IRoleMappingRepository rolemapping = db.RoleMappingRepository();

                LaboratoryBusiness.POCO.User.Cl_ClientUser cuser = cluser.GetByID(employeeuserid);
                LaboratoryBusiness.POCO.User.Cl_ClientUserDetail cudetail = clientuserdetail.GetByID(cuser.DetailID.Value);
            
                Repositories.User.IAdminDetailRepository adminrepo = db.AdminDetailRepository();

                Repositories.Admin.ISystemUserRepository sysuserrepo = new LaboratoryBusiness.BLL.Admin.SystemUserRepository();
                Repositories.Admin.IClientRepository clientrepo = new LaboratoryBusiness.BLL.Admin.ClientRepository();

                int systemclientuserid = adminrepo.GetAll().FirstOrDefault().SystemClientID.Value;
                var clientdetail = sysuserrepo.GetByID(systemclientuserid);
                int clientdetailid = clientdetail.DetailID.Value;
                var clientobj = clientrepo.GetByID(clientdetailid);

                string roles = string.Join(",", ((from rlm in rolemapping.GetAll()
                                                  join rl in role.GetAll() on rlm.RoleID equals rl.RoleID
                                                  where rlm.UserID == cuser.ClientUserID
                                                  select rl.RoleName).ToList()));

                string defaultrole =  ((from rlm in rolemapping.GetAll()
                                                  join rl in role.GetAll() on rlm.RoleID equals rl.RoleID
                                        where rlm.UserID == cuser.ClientUserID && (rlm.IsDefault.HasValue? rlm.IsDefault.Value == true:false)
                                                  select rl.RoleName).FirstOrDefault());

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("@employeeID@", cuser.ClientUserID.ToString("000"));
                dic.Add("@name@", cuser.FirstName + " " + cuser.LastName); dic.Add("@fs@", cuser.FirstName); dic.Add("@ls@", cuser.LastName);
                dic.Add("{ClientName}", clientobj.CompanyName);
                dic.Add("{ClientAddress}", clientdetail.Address);
                dic.Add("{Client Email}", clientdetail.Email);
                dic.Add("@username@", cuser.Username);
                dic.Add("@email@", cuser.Email);
                dic.Add("@mobileno@", cuser.MobileNo);
                dic.Add("@address@", cuser.Address);
                dic.Add("@status@", cuser.IsActive.HasValue ? cuser.IsActive.Value ? "Active" : "False" : "False");
                dic.Add("@createddate@", cudetail.CreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"));

                dic.Add("@joiningdate@", cudetail.JoiningDate.Value.ToString("MM/dd/yyyy HH:mm tt"));
                dic.Add("@terminationdate@", (cudetail.TerminationDate.HasValue?cudetail.TerminationDate.Value.ToString("MM/dd/yyyy HH:mm tt"):"Not Available"));
                dic.Add("@terminationreason@", (cudetail.TerminationReason==null ? "Not Available" : cudetail.TerminationReason));
                dic.Add("@qualifications@", cudetail.Qualifications);
                dic.Add("@typeofcollection@", (cudetail.TypeOfCollection == null ? "Not Available" : cudetail.TypeOfCollection));
                dic.Add("@license@", (cudetail.License == null ? "Not Available" : cudetail.License));
                dic.Add("@employeementtype@", (cudetail.IsFulltime.HasValue ? (cudetail.IsFulltime.Value ? "Full-Time" : "Part-Time") : "Not Available"));
                dic.Add("@roles@", roles);
                dic.Add("@defaultrole@", defaultrole);


                string pdfhtmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\Employee\Employee.html", dic);

                FileManager pd = PdfManager.HtmlToPdf("EmployeeDetail_" + cuser.ClientUserID.ToString("000") + "_" + cudetail.CreatedDate.Value.ToString("MMddyyyyHHmmtt"), pdfhtmltemplate);
                Client cl = new Client(subdomainurl);
                FileInitializer fl = new FileInitializer(cl);
                string pdfuploadedpath;
                fl.UploadFile(pd, "\\EmployeeFiles", out pdfuploadedpath);
                link = pdfuploadedpath;
                return pd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static FileManager CreateTestPDF(int testid, string subdomainurl, DBInitializer db, out string link)
        {
            dynamic testobj = null;
            dynamic samplecollectionobj = null;
            dynamic investigationobj = null;
            dynamic conclusionobj = null;

            try
            {
                Repositories.User.IClientUserRepository clientuser = db.ClientUserRepository();
                Repositories.User.IPatientDetailRepository patientdetailrepo = db.PatientDetailRepository();
                Repositories.User.ITestRepository testrep = db.TestRepository();
                Repositories.User.ITestStatusRepositories teststatusrep = db.TestStatusRepositories();
                Repositories.User.ITestAttachmentRepository testattachmentrepo = db.TestAttachmentRepository();
                Repositories.User.ITestAttachmentTypeRepository testattachmenttyperepo = db.TestAttachmentTypeRepository();
                Repositories.User.ITestInvestigationRepository testinvestigationrepo = db.TestInvestigationRepository();
                Repositories.User.ITestConclusionRepository testconclusionrepo = db.TestConclusionRepository();
                Repositories.User.ITestReportTypeRepository testreporttyperepo = db.TestReportTypeRepository();
                Repositories.User.IExtraWorkRequestReposotory extraworkrequestrepo = db.ExtraWorkRequestRepository();
                Repositories.User.IExtraWorkAttachmentRepository extraworkattachmentrepo = db.ExtraWorkAttachmentRepository();
                Repositories.User.IExtraWorkRequestStatusRepository extraworkstatusrepo = db.ExtraWorkRequestStatusRepository();



                if (testid > 0)
                {

                    var testinnerobj = testrep.GetByID(testid);


                    if (testinnerobj != null)
                    {
                        int attachmenttypeid = 0;
                        var patientobj = (from cl in clientuser.GetAll()
                                          join pd in patientdetailrepo.GetAll() on cl.DetailID equals pd.PatientDetailID
                                          where cl.ClientUserID == testinnerobj.PatientUserID.Value
                                          select new { cl, pd }).FirstOrDefault();
                        var createdby = clientuser.GetByID(testinnerobj.TestCreatedBy.Value);
                        string createdbycustom = createdby.FirstName + " " + createdby.LastName;
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
                            testinnerobj.IsPublish
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

                        Dictionary<string, string> dic = new Dictionary<string, string>();

                        if (testobj != null)
                        {
                            dic.Add("@patientid@", testobj.PatientID.ToString());
                            dic.Add("@city@", testobj.City.ToString());
                            dic.Add("@testcreatedby@", testobj.TestCreatedByCustom.ToString());
                            dic.Add("@patientname@", testobj.PatientName.ToString());
                            dic.Add("@mobileno@", testobj.MobileNo.ToString());
                            dic.Add("@testcreateddate@", testobj.TestCreatedDateCustom.ToString());
                            dic.Add("@age@", testobj.Age.ToString());
                            dic.Add("@gender@", testobj.Gender.ToString());
                            dic.Add("@testid@", testobj.TestID.ToString());
                            dic.Add("@complainthistory@", testobj.ComplaintHistory.ToString());
                            dic.Add("@testname@", testobj.TestName.ToString());
                            dic.Add("@sampledescription@", testobj.Description.ToString());
                            dic.Add("@issamplerequired@", testobj.IsSampleRequired.ToString());
                        }
                        else
                        {
                            dic.Add("@patientid@", "");
                            dic.Add("@city@", "");
                            dic.Add("@testcreatedby@", "");
                            dic.Add("@patientname@", "");
                            dic.Add("@mobileno@", "");
                            dic.Add("@testcreateddate@", "");
                            dic.Add("@age@", "");
                            dic.Add("@gender@", "");
                            dic.Add("@testid@", "");
                            dic.Add("@complainthistory@", "");
                            dic.Add("@testname@", "");
                            dic.Add("@sampledescription@", "");
                            dic.Add("@issamplerequired@", "");
                        }

                        if (samplecollectionobj != null)
                        {
                            dic.Add("@issamplecollected@", samplecollectionobj.IsSampleCollected.ToString());
                            dic.Add("@samplelabel@", samplecollectionobj.SampleLabel.ToString());
                            dic.Add("@samplecollecteddate@", samplecollectionobj.SampleCollectedDateCustom.ToString());
                            dic.Add("@samplecode@", samplecollectionobj.SampleCode.ToString());
                            dic.Add("@samplecollectedby@", samplecollectionobj.SampleCollectedBy.ToString());
                            dic.Add("@issampletype@", samplecollectionobj.SampleType.ToString());
                            
                        }
                        else
                        {
                            dic.Add("@issamplecollected@", "");
                            dic.Add("@samplelabel@", "");
                            dic.Add("@samplecollecteddate@", "");
                            dic.Add("@samplecode@", "");
                            dic.Add("@samplecollectedby@", "");
                            dic.Add("@issampletype@", "");
                        }

                        if (investigationobj != null)
                        {
                            dic.Add("@investigationby@", investigationobj.AnalysisBy.ToString());
                            dic.Add("@investigationdate@", investigationobj.AnalysisDateCustom.ToString());

                            string investigationsrow = string.Empty;
                            string extraworkrequests = string.Empty;

                            if (investigationobj.InvestigationList != null)
                            {
                                foreach(var inv in ((List<BusinessPOCO.User.Cl_TestInvestigation>)investigationobj.InvestigationList)) 
                                {
                                    investigationsrow += @"<tr>
                                        <td>" + inv.InvestigationName + @"</td>
                                        <td>" + inv.Value + @"</td>
                                        <td>" + inv.Range + @"</td>
                                        <td>" + inv.Value + @"</td>
                                        </tr>";
                                }
                                dic.Add("@investigationsrow@", investigationsrow);//Html
                            }
                            else
                            {
                                dic.Add("@investigationsrow@", investigationsrow);//Html
                            }

                            if (investigationobj.ExtraWorkRequestList != null)
                            {
                                foreach (var ext in ((List<ExtraWorkRequest>)investigationobj.ExtraWorkRequestList))
                                {
                                    extraworkrequests += @"<h3 class=""border-bottom border-gray pb-2"" style=""margin-left: 20px;"">Extra Work Request # "+ext.ExtraWorkID+@"</h3>

                                    <div class=""row"" style=""margin-left:0"">
                                        <div class=""col-md-4 col-6"">
                                            <div class=""mb-4"">
                                                <p class=""text-primary mb-1""><i class=""text-16 mr-1""></i>H & E Levels</p>
                                                <span>" + ext.H_ELevels + @"</span>
                                            </div>
                                            <div class=""mb-4"">
                                                <p class=""text-primary mb-1""><i class=""text-16 mr-1""></i>Special Stains</p>
                                                <span>" +ext.SpecialStains+@"</span>
                                            </div>

                                        </div>
                                        <div class=""col-md-4 col-6"">
                                            <div class=""mb-4"">
                                                <p class=""text-primary mb-1""><i class=""text-16 mr-1""></i>ImmunoHisto Chemistry</p>
                                                <span>"+ext.ImmunoHistoChemistry+@"</span>
                                            </div>
                                            <div class=""mb-4"">
                                                <p class=""text-primary mb-1""><i class=""text-16 mr-1""></i>Others</p>
                                                <span>"+ext.Others+@"</span>
                                            </div>
                                        </div>
                                        <div class=""col-md-4 col-6"">
                                            <div class=""mb-4"">
                                                <p class=""text-primary mb-1""><i class=""text-16 mr-1""></i>Completed By</p>
                                                <span>"+ext.CompletedBy+@"</span>
                                            </div>
                                            <div class=""mb-4"">
                                                <p class=""text-primary mb-1""><i class=""text-16 mr-1""></i>Completed Date</p>
                                                <span>"+ext.CompletedDateCustom+@"</span>
                                            </div>
                                        </div>
                                    </div><div class=""row"" style=""margin-left:0px;"">
                                        <div class=""col-lg-12"">

                                            <div class=""table-borderless"">
                                                <table class=""table"">
                                                    <thead>
                                                        <tr>
                                                            <th scope=""col"" style=""color:#70657b"">Investigation Name</th>
                                                            <th scope=""col"" style=""color:#70657b"">Values</th>
                                                            <th scope=""col"" style=""color:#70657b"">Range</th>
                                                            <th scope=""col"" style=""color:#70657b"">Result</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>";

                                    if (ext.Investigations != null)
                                    {
                                        foreach (var extinv in ext.Investigations)
                                        {
                                            extraworkrequests += @"<tr>
                                        <td>" + extinv.InvestigationName + @"</td>
                                        <td>" + extinv.Value + @"</td>
                                        <td>" + extinv.Range + @"</td>
                                        <td>" + extinv.Value + @"</td>
                                        </tr>";
                                        }
                                    }

                                    extraworkrequests += @"</tbody>
                                                </table>
                                            </div>

                                        </div>
                                    </div>";
                                }
                                dic.Add("@extraworkrequests@", extraworkrequests);//Html
                            }
                            else
                            {
                                dic.Add("@extraworkrequests@", extraworkrequests);//Html
                            }
                            
                        }
                        else
                        {
                            dic.Add("@investigationby@", "");
                            dic.Add("@investigationdate@", "");
                            dic.Add("@investigationsrow@", "");//Html
                            dic.Add("@extraworkrequests@", "");//Html
                        }

                        if (conclusionobj != null)
                        {
                            dic.Add("@reporttype_conclusion@", conclusionobj.TestReportType.ToString());
                            dic.Add("@specimendetails@", conclusionobj.SpecimenDetails.ToString());
                            dic.Add("@conclusion@", conclusionobj.Conclusion.ToString());
                            dic.Add("@conclusionby@", conclusionobj.ConclusionBy.ToString());
                            dic.Add("@clinicaldetails@", conclusionobj.ClinicalDetails.ToString());
                            string html_macroscopy = string.Empty;
                            string html_snomedcoding = string.Empty;
                            string html_microscopy = string.Empty;
                            string html_sampledescription = string.Empty;
                            string html_report = string.Empty;

                            if (conclusionobj.Macroscopy != null)
                            {
                                html_macroscopy = @"<div class=""mb-4"" id=""macroscopy_con_vw"">
                            <p class=""text-primary mb-1""><i class=""text-16 mr-1""></i>Macroscopy</p>
                            <span id=""macroscopy_con_vw_lbl"">" + conclusionobj.Macroscopy.ToString() + @"</span>
                        </div>";
                            }
                            else
                            {
                                html_macroscopy = "";
                            }

                            if (conclusionobj.SnomedCoding != null)
                            {
                                html_snomedcoding = @"<div class=""mb-4"" id=""snomedcoding_con_vw"">
                            <p class=""text-primary mb-1""><i class=""text-16 mr-1""></i>SNOMED Coding</p>
                            <span id=""snomedcoding_con_vw_lbl"">"+conclusionobj.SnomedCoding.ToString()+@"</span>
                        </div>";
                            }
                            else
                            {
                                html_snomedcoding = "";
                            }

                            dic.Add("@conclusiondate@", conclusionobj.ConclusionDateCustom.ToString());

                            if (conclusionobj.Microscopy != null)
                            {
                                html_microscopy = @"<div class=""mb-4"" id=""microscopy_con_vw"">
                            <p class=""text-primary mb-1""><i class=""text-16 mr-1""></i>Microscopy</p>
                            <span id=""microscopy_con_vw_lbl"">" + conclusionobj.Microscopy.ToString() + @"</span>
                        </div>";
                            }
                            else
                            {
                                html_microscopy = "";
                            }

                            if (conclusionobj.SampleDescription != null)
                            {
                                html_sampledescription = @"<div class=""mb-4"" id=""sampledescription_con_vw"">
                            <p class=""text-primary mb-1""><i class=""text-16 mr-1""></i>Sample Description</p>
                            <span id=""sampledescription_con_vw_lbl"">" + conclusionobj.SampleDescription.ToString() + @"</span>
                        </div>";
                            }
                            else
                            {
                                html_sampledescription = "";
                            }

                            if (conclusionobj.Report != null)
                            {
                                html_report = @"<div class=""mb-4"" id=""report_con_vw"">
                            <p class=""text-primary mb-1""><i class=""text-16 mr-1""></i>Report</p>
                            <span id=""report_con_vw_lbl"">" + conclusionobj.Report.ToString() + @"</span>
                        </div>";
                            }
                            else
                            {
                                html_report = "";
                            }

                            dic.Add("@macroscopy@", html_macroscopy);//Html
                            dic.Add("@snomedcoding@", html_snomedcoding);//Html
                            dic.Add("@microscopy@", html_microscopy);//Html
                            dic.Add("@sampledescription_conclusion@", html_sampledescription);//Html
                            dic.Add("@report_conclusion@", html_report);//Html
                        }
                        else
                        {
                            dic.Add("@reporttype_conclusion@", "");
                            dic.Add("@specimendetails@", "");
                            dic.Add("@conclusion@", "");
                            dic.Add("@conclusionby@", "");
                            dic.Add("@clinicaldetails@", "");
                            dic.Add("@macroscopy@", "");//Html
                            dic.Add("@snomedcoding@", "");//Html
                            dic.Add("@conclusiondate@", "");
                            dic.Add("@microscopy@", "");//Html
                            dic.Add("@sampledescription_conclusion@", "");//Html
                            dic.Add("@report_conclusion@", "");//Html
                        }

                        dic.Add("@bootstrapcsslink@", HelpingClass.GetDefaultDirectory() + "\\Content\\EmailTemplates\\Content\\bootstrap.min.css");
                        dic.Add("@jqueryjslink@", HelpingClass.GetDefaultDirectory() + "\\Content\\EmailTemplates\\Content\\jquery.min.js");
                        dic.Add("@bootstrapjslink@", HelpingClass.GetDefaultDirectory() + "\\Content\\EmailTemplates\\Content\\bootstrap.min.js");

                        string pdfhtmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\TestPdfTemplate.html", dic);

                        FileManager pdf = PdfManager.HtmlToPdf("TestDetail_" + testobj.TestID.ToString("000") + "_" + DateTime.Now.ToString("MMddyyyyHHmmtt"), pdfhtmltemplate);
                        Client subclient = new Client(subdomainurl);
                        FileInitializer fl = new FileInitializer(subclient);
                        string pdfuploadedpath;
                        fl.UploadFile(pdf, "\\TestFiles", out pdfuploadedpath);
                        link = pdfuploadedpath;
                        return pdf;

                    }

                    else
                    {
                        throw new Exception("There is no test associated to this test id.");
                        
                    }


                }

                throw new Exception("Kindly provide correct test id.");


            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong from our side!please try later" + ex.Message);
            }
        }
    }
}