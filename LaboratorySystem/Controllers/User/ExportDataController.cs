using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Repositories = LaboratoryBusiness.Repositories;
using BLL = LaboratoryBusiness.BLL;
using BusinessPOCO = LaboratoryBusiness.POCO;

namespace LaboratorySystem.Controllers.User
{
    [RoutePrefix("User/ExportData")]
    public class ExportDataController :  RedirectController
    {
        [ClientAuthorizeMember]
        [Route("AllPatientRecords")]
        // GET: ExportData
        public ActionResult AllPatientRecords()
        {
            return View("~/Views/User/ExportData/AllPatientRecords.cshtml");
        }

        [ClientAuthorizeMember]
        [Route("AllTestRecords")]
      //  [SystemAuthorizeMember]
        [HttpGet]
        public ActionResult  AllTestRecords()
        {
            return View("~/Views/User/ExportData/AllTestRecords.cshtml");
        }

        [ClientAuthorizeMember]
        [Route("AllPaymentRecords")]
        //  [SystemAuthorizeMember]
        [HttpGet]
        public ActionResult AllPaymentRecords()
        {
            return View("~/Views/User/ExportData/AllPaymentRecords.cshtml");
        }

        [ClientAuthorizeMember]
        [Route("AllEmployeeRecords")]
        //  [SystemAuthorizeMember]
        [HttpGet]
        public ActionResult AllEmployeeRecords()
        {
            return View("~/Views/User/ExportData/AllEmployeeRecords.cshtml");
        }


        [Route("AllPatientRecords")]
        [HttpPost]
        public JsonResult GetAllPatientRecords()
        {
            try
            {
                LaboratoryBusiness.Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                LaboratoryBusiness.Repositories.User.IClientUserTypeRepository clientusertype = this.currentdomaindb.ClientUserTypeRepository();
                LaboratoryBusiness.Repositories.User.IPatientDetailRepository patientdetail = this.currentdomaindb.PatientDetailRepository();


                int clientuserdetailtypeid = clientusertype.GetAll().Where(x => x.TypeName.Equals("Patient")).FirstOrDefault().ClientUserTypeID;

                object patientdetailobj = null;

                patientdetailobj = (from cl in clientuser.GetAll()
                                  join pd in patientdetail.GetAll() on cl.DetailID equals pd.PatientDetailID
                                  where cl.DetailType.Value == clientuserdetailtypeid && pd.PaymentReceiptPdfLink!=null && pd.PdfLink!=null
                                  select new
                                  {
                                      pd.PatientDetailID,
                                      pd.Payment,
                                      pd.PdfLink,
                                      pd.PaymentReceiptPdfLink,
                                      pd.City,
                                      CreatedDate=pd.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm tt"),
                                      cl.ClientUserID,
                                      cl.Username,
                                      cl.FirstName,
                                      cl.LastName,
                                      Status=cl.IsActive.HasValue?(cl.IsActive.Value?"Active":"Inactive"):"Inactive",
                                      FullName = cl.FirstName + " " + pd.MiddleName + " " + cl.LastName,
                                      pd.Sex,
                                      cl.IsBlock
                                  }).ToList();

                    if (patientdetailobj != null)
                {


                    return WebJSResponse.ResponseSimple(new { patientuserjson = patientdetailobj });
                }

                else
                {
                    return WebJSResponse.ResponseToastr(ToastrEnum.error, "No patient found !", "There is no patient associated to this patient id.", new { });
                }

            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("DownloadAllPatientRecords")]
        [HttpGet]

        public ActionResult DownloadAllPatientRecords(string qr)
        {
            try
            {
                LaboratoryBusiness.Repositories.User.IPatientDetailRepository patientdetail = this.currentdomaindb.PatientDetailRepository();
                List<FileManager> patientpdfs = new List<FileManager>();

                string[] stringids = qr.Split(',');
                int patientDetailid = 0;

                LaboratoryBusiness.POCO.User.Cl_PatientDetail patientobj;
                foreach (string id in stringids)
                {
                    patientDetailid = Convert.ToInt32(id);// Convert.ToInt32(Encoding.UTF8.GetString(Convert.FromBase64String(id)));
                    patientobj = patientdetail.GetByID(patientDetailid);
                    patientpdfs.Add(new FileManager(patientobj.PdfLink, "PatientDetails_" + patientDetailid));

                }

                FileManager zipfile = ZipManager.ListOfFilesToZipFile("PatientDetailsPdfs", patientpdfs);

                return File(zipfile.FileBytes, "application/zip", zipfile.Name + zipfile.Extension);
            }
            catch (Exception ex)
            {
                return HttpNotFound("Somthing went wrong while trying to download" + ex.Message);
            }

        }

        [Route("GetDetail")]
        [HttpPost]
        public JsonResult GetDetail(int clientdetailid)
        {
            try
            {
                LaboratoryBusiness.Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                LaboratoryBusiness.Repositories.User.IPatientDetailRepository patientdetail = this.currentdomaindb.PatientDetailRepository();


                if (clientdetailid > 0)
                {

                    var clientuserobj = clientuser.GetByID(clientdetailid);
                    var patientdetailobj = patientdetail.GetAll().Where(x => x.PatientDetailID == clientuserobj.DetailID.Value).FirstOrDefault();
                    if (patientdetailobj != null && clientuserobj != null)
                    {
                        var patientuserobj = new
                        {
                            FirstName = clientuserobj.FirstName,
                            LastName = clientuserobj.LastName,
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
                            CreatedDateCustom = clientuserobj.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm tt"),
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

        [Route("GetAllTestRecords")]
        [HttpPost]
        public JsonResult GetAllTestRecords(string status)
        {

            try
            {
                LaboratoryBusiness.Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                LaboratoryBusiness.Repositories.User.IPatientDetailRepository patientdetailrepo = this.currentdomaindb.PatientDetailRepository();
                LaboratoryBusiness.Repositories.User.ITestRepository testrep = this.currentdomaindb.TestRepository();
                LaboratoryBusiness.Repositories.User.ITestStatusRepositories teststatusrep = this.currentdomaindb.TestStatusRepositories();

                int teststatusid = 5;

                //if (status.Equals("Open"))
                //{
                //    if (MySession.GetClientSession(this.subdomainurl).CurrentRole.RoleName.Equals("Receptionist"))
                //    {
                //        teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("On Receptionist Plate")).FirstOrDefault().TestStatusID;
                //    }
                //    else if (MySession.GetClientSession(this.subdomainurl).CurrentRole.RoleName.Equals("Lab Technician"))
                //    {
                //        teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("On Lab Technician Plate")).FirstOrDefault().TestStatusID;
                //    }
                //    else if (MySession.GetClientSession(this.subdomainurl).CurrentRole.RoleName.Equals("Clinical Laboratory Scientist"))
                //    {
                //        teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("On Clinical Lab Scientist Plate")).FirstOrDefault().TestStatusID;
                //    }
                //    else if (MySession.GetClientSession(this.subdomainurl).CurrentRole.RoleName.Equals("Clinical Laboratory Consultant"))
                //    {
                //        teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("On Clinical Lab Doctor Plate")).FirstOrDefault().TestStatusID;
                //    }
                //}
                //else if (status.Equals("Completed"))
                //{
                //    teststatusid = teststatusrep.GetAll().Where(x => x.StatusName.Equals("Completed")).FirstOrDefault().TestStatusID;
                //}

                var result = (from ts in testrep.GetAll()
                              join ptus in clientuser.GetAll() on ts.PatientUserID equals ptus.ClientUserID
                              join pt in patientdetailrepo.GetAll() on ptus.DetailID equals pt.PatientDetailID
                              where (ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == teststatusid && ts.PdfLink!=null
                              select new
                              {
                                  ts.TestID,
                                  ts.TestName,
                                  PatientName = (ptus.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + ptus.LastName),
                                  Status = (ts.TestStatusID.HasValue ? ts.TestStatusID.Value : 0) == 5 ? "Completed" : "Pending",
                                  TestCreatedDateCustom = ts.TestCreatedDate.Value.ToString("dd/MM/yyyy HH:mm tt"),
                                  IsPublish = ts.IsPublish.HasValue ? ts.IsPublish.Value ? "Yes" : "No" : "No",
                                  ts.PdfLink,
                                  City=pt.City
                              }).ToList();

                return WebJSResponse.ResponseSimple(new { testjson = result });


            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("DownloadAllTestRecords")]
        [HttpGet]

        public ActionResult DownloadAllTestRecords(string qr)
        {
            try
            {
                LaboratoryBusiness.Repositories.User.ITestRepository testrep = this.currentdomaindb.TestRepository();
                List<FileManager> testreppdfs = new List<FileManager>();

                string[] stringids = qr.Split(',');
                int testid = 0;

                LaboratoryBusiness.POCO.User.Cl_Test testtobj;
                foreach (string id in stringids)
                {
                    testid = Convert.ToInt32(id);// Convert.ToInt32(Encoding.UTF8.GetString(Convert.FromBase64String(id)));
                    testtobj = testrep.GetByID(testid);
                    testreppdfs.Add(new FileManager(testtobj.PdfLink, "Tests_" + testid));

                }

                FileManager zipfile = ZipManager.ListOfFilesToZipFile("Tests_DetailsPdfs", testreppdfs);

                return File(zipfile.FileBytes, "application/zip", zipfile.Name + zipfile.Extension);
            }
            catch (Exception ex)
            {
                return HttpNotFound("Somthing went wrong while trying to download" + ex.Message);
            }

        }

        [Route("DownloadAllPaymentRecords")]
        [HttpGet]

        public ActionResult DownloadAllPaymentRecords(string qr)
        {
            try
            {
                LaboratoryBusiness.Repositories.User.IPatientDetailRepository patientdetail = this.currentdomaindb.PatientDetailRepository();
                List<FileManager> patientpaymentpdfs = new List<FileManager>();

                string[] stringids = qr.Split(',');
                int patientDetailid = 0;

                LaboratoryBusiness.POCO.User.Cl_PatientDetail patientobj;
                foreach (string id in stringids)
                {
                    patientDetailid = Convert.ToInt32(id);// Convert.ToInt32(Encoding.UTF8.GetString(Convert.FromBase64String(id)));
                    patientobj = patientdetail.GetByID(patientDetailid);
                    patientpaymentpdfs.Add(new FileManager(patientobj.PaymentReceiptPdfLink, "PatientPaymentDetails_" + patientDetailid));

                }

                FileManager zipfile = ZipManager.ListOfFilesToZipFile("PatientPaymentDetailsPdfs", patientpaymentpdfs);

                return File(zipfile.FileBytes, "application/zip", zipfile.Name + zipfile.Extension);
            }
            catch (Exception ex)
            {
                return HttpNotFound("Somthing went wrong while trying to download" + ex.Message);
            }

        }


        [HttpPost]
        [Route("GetAllEmployeeRecords")]
        public JsonResult GetAllEmployeeRecords(bool status)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IClientUserTypeRepository clientusertype = this.currentdomaindb.ClientUserTypeRepository();
                Repositories.User.IClientUserDetailRepository clientuserdetail = this.currentdomaindb.ClientUserDetailRepository();
                Repositories.User.IRoleRepository role = this.currentdomaindb.RoleRepository();
                Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();

                var roles = role.GetAll().ToList();
                var assignedroles = rolemapping.GetAll().ToList();

                int clientuserdetailtypeid_patient = clientusertype.GetAll().Where(x => x.TypeName.Equals("Patient")).FirstOrDefault().ClientUserTypeID;
                int clientuserdetailtypeid_admin = clientusertype.GetAll().Where(x => x.TypeName.Equals("Client Admin")).FirstOrDefault().ClientUserTypeID;

                object returnList = null;
                returnList = (from s in clientuser.GetAll()
                              join cud in clientuserdetail.GetAll() on s.DetailID equals cud.UserDetailID
                              select new
                              {
                                  s.ClientUserID,
                                  s.FirstName,
                                  s.LastName,
                                  FullName = s.FirstName + " " + s.LastName,
                                  s.MobileNo,
                                  s.ProfilePic,
                                  s.Username,
                                  s.IsActive,
                                  s.Email,
                                  s.Address,
                                  s.DetailID,
                                  DetailTypeID = s.DetailType,
                                  s.CreatedBy,
                                  s.CreatedDate,
                                  s.UpdatedBy,
                                  s.UpdatedDate,
                                  s.IsBlock,
                                  cud.PdfLink,
                                  Roles = string.Join(",", ((from ar in assignedroles
                                                             join r in roles on ar.RoleID equals r.RoleID
                                                             where ar.UserID == s.ClientUserID
                                                             select new { Roles = (r.RoleName + " " + (ar.IsDefault.Value ? "Default" : "")) }))),
                                  status = s.IsActive.HasValue ? (s.IsActive.Value == true ? "Active" : "Inactive") : "Inactive",
                              }).Where(x => x.PdfLink!=null && !(x.DetailTypeID.Value == clientuserdetailtypeid_patient || x.DetailTypeID.Value == clientuserdetailtypeid_admin)).OrderByDescending(x => x.ClientUserID).ToList();


                return WebJSResponse.ResponseSimple(new { employeejson = returnList });
            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [HttpPost]
        [Route("GetEmployeeDetail")]
        public JsonResult GetEmployeeDetail(int employeeid)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IClientUserDetailRepository clientuserdetail = this.currentdomaindb.ClientUserDetailRepository();
                Repositories.User.IClientUserAttachmentDetailRepository clientuserattachmentsdetail = this.currentdomaindb.ClientUserAttachmentDetailRepository();
                Repositories.User.IClientUserTypeRepository clientusertype = this.currentdomaindb.ClientUserTypeRepository();
                Repositories.User.IRoleRepository role = this.currentdomaindb.RoleRepository();
                Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();

                if (employeeid > 0)
                {
                    object assignedroles = null;
                    object attachments = null;
                    var clientuserobj = clientuser.GetByID(employeeid);
                    var clientuserdetailobj = clientuserdetail.GetAll().Where(x => x.UserDetailID == clientuserobj.DetailID.Value).FirstOrDefault();
                    if (clientuserobj != null && clientuserdetailobj != null)
                    {
                        var roles = role.GetAll().ToList();
                        assignedroles = (from rm in rolemapping.GetAll()
                                         join r in roles on rm.RoleID equals r.RoleID
                                         where rm.UserID == clientuserobj.ClientUserID
                                         select new { r.RoleName, IsDefault = (rm.IsDefault.Value ? "Yes" : "No") }).ToList();

                        attachments = (from at in clientuserattachmentsdetail.GetAll()
                                       where at.UserDetailID.Value == clientuserdetailobj.UserDetailID
                                       select at).ToList();

                        var clientuserobjj = new
                        {
                            FirstName = clientuserobj.FirstName,
                            LastName = clientuserobj.LastName,
                            MobileNo = clientuserobj.MobileNo,
                            Email = clientuserobj.Email,
                            Address = clientuserobj.Address,
                            IsActive = clientuserobj.IsActive,
                            ClientUserID = clientuserobj.ClientUserID,
                            Username = clientuserobj.Username,
                            JoiningDateCustom = clientuserdetailobj.JoiningDate.Value.ToString("dd/MM/yyyy"),
                            TerminationDateCustom = clientuserdetailobj.TerminationDate.HasValue ? clientuserdetailobj.TerminationDate.Value.ToString("dd/MM/yyyy") : null,
                            TerminationReason = clientuserdetailobj.TerminationReason,
                            Qualifications = clientuserdetailobj.Qualifications,
                            TypeOfCollection = clientuserdetailobj.TypeOfCollection,
                            License = clientuserdetailobj.License,
                            EmployementType = clientuserdetailobj.IsFulltime.HasValue ? clientuserdetailobj.IsFulltime.Value ? "Full-time" : "Part-time" : null,
                            PdfLink = clientuserdetailobj.PdfLink,
                            Status = clientuserobj.IsActive.HasValue ? (clientuserobj.IsActive.Value ? "Active" : "Inactive") : "Inactive",
                            EmployeeType = clientusertype.GetByID(clientuserobj.DetailType.Value).TypeName,
                            CreatedDateCustom = clientuserobj.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm tt")
                        };
                        return WebJSResponse.ResponseSimple(new { clientuserjson = clientuserobjj, assignedrolesjson = assignedroles, attachmentsjson = attachments });
                    }
                    else
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "No employee found !", "There is no employee associated to this employee id.", new { });
                    }


                }
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Incorrect employee-id !", "Kindly provide correct employee id.", new { });
            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }

        }

        [Route("DownloadAllEmployeeRecords")]
        [HttpGet]

        public ActionResult DownloadAllEmployeeRecords(string qr)
        {
            try
            {
                LaboratoryBusiness.Repositories.User.IClientUserRepository cuser = this.currentdomaindb.ClientUserRepository();
                LaboratoryBusiness.Repositories.User.IClientUserDetailRepository cuserdetail = this.currentdomaindb.ClientUserDetailRepository();
                List<FileManager> employeepdfs = new List<FileManager>();

                string[] stringids = qr.Split(',');
                int employeeid = 0;

                LaboratoryBusiness.POCO.User.Cl_ClientUser cuserobj;
                LaboratoryBusiness.POCO.User.Cl_ClientUserDetail cuserdetailobj;
                foreach (string id in stringids)
                {
                    employeeid = Convert.ToInt32(id);// Convert.ToInt32(Encoding.UTF8.GetString(Convert.FromBase64String(id)));
                    cuserobj = cuser.GetByID(employeeid);
                    cuserdetailobj = cuserdetail.GetByID(cuserobj.DetailID.Value);
                    employeepdfs.Add(new FileManager(cuserdetailobj.PdfLink, "EmployeeDetails_" + employeeid.ToString()));

                }

                FileManager zipfile = ZipManager.ListOfFilesToZipFile("EmployeeDetailsPdfs", employeepdfs);

                return File(zipfile.FileBytes, "application/zip", zipfile.Name + zipfile.Extension);
            }
            catch (Exception ex)
            {
                return HttpNotFound("Somthing went wrong while trying to download" + ex.Message);
            }

        }

    }

   
}