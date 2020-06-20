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
    [RoutePrefix("User/Employee")]
    public class EmployeeController : RedirectController
    {
        // GET: Employee
         [Route]
         [ClientAuthorizeMember]
        public ActionResult Index()
        {
            if (!HelpingClass.ValidateConnection())
            {
                throw new Exception("LabSystemDB_Connection does not establish!");
            }

            ViewBag.currentdomaindb = this.currentdomaindb;
            return View("~/Views/User/Employee/Index.cshtml");
        }


         [HttpPost]
         [Route("Create")]
         public JsonResult Create(int employeetype, string firstname, string lastname, string username, string email,
             string mobileno, string address, bool status, string assignedrolesid, DateTime joiningdate, string qualifications,
             string typeofcollection, string license, bool employementtype, DateTime terminationdate, string terminationreason, int editid)
         {

             try
             {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IClientUserTypeRepository clientusertype = this.currentdomaindb.ClientUserTypeRepository();
                Repositories.User.IClientUserDetailRepository clientuserdetail = this.currentdomaindb.ClientUserDetailRepository();
                Repositories.User.IRoleRepository role = this.currentdomaindb.RoleRepository();
                Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();

                Repositories.User.IAdminDetailRepository admindetail = this.currentdomaindb.AdminDetailRepository();
                BusinessPOCO.User.Cl_AdminDetail _addetail = admindetail.GetAll().FirstOrDefault();

                Repositories.Admin.ISystemUserRepository su = new LaboratoryBusiness.BLL.Admin.SystemUserRepository();
                BusinessPOCO.Admin.SystemUser supoco = su.GetByID(_addetail.SystemClientID.Value);

                Repositories.Admin.IClientRepository cl = new LaboratoryBusiness.BLL.Admin.ClientRepository();
                BusinessPOCO.Admin.Client clpoco = cl.GetByID(supoco.DetailID.Value);

                 if (editid == 0)
                 {
                     if (clientuser.GetAll().Where(x => (x.Username.Trim()).Equals(username.Trim() + "@" + clpoco.CompanyName.Replace(" ", "").Trim()) || (string.IsNullOrEmpty(x.Email) ? false : x.Email.Equals(email.Trim()))).Count() > 0)
                     {
                         return WebJSResponse.ResponseToastr(ToastrEnum.error, "Username or email already exist !", "Please try another !", new { });

                     }

                     BusinessPOCO.User.Cl_ClientUserDetail clientuserdetailobj = new BusinessPOCO.User.Cl_ClientUserDetail();
                     clientuserdetailobj.JoiningDate = joiningdate;
                     if(status==false) {
                         clientuserdetailobj.TerminationDate = terminationdate;
                         clientuserdetailobj.TerminationReason = terminationreason;
                     }
                     clientuserdetailobj.Qualifications = qualifications;
                     if(employeetype==3 || assignedrolesid.Contains("3")) {
                         clientuserdetailobj.TypeOfCollection = typeofcollection;
                         clientuserdetailobj.License = license;
                     }

                     if (employeetype == 4 || assignedrolesid.Contains("4"))
                     {
                         clientuserdetailobj.License = license;
                     }

                     if (employeetype == 5 || assignedrolesid.Contains("5"))
                     {
                         clientuserdetailobj.IsFulltime = employementtype;
                     }

                     clientuserdetailobj.CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                     clientuserdetailobj.CreatedDate = DateTime.Now;

                     clientuserdetail.Insert(clientuserdetailobj);
                     clientuserdetail.Save();


                     string _randompassword = System.Web.Security.Membership.GeneratePassword(8, 0);
                     byte[] pass = System.Text.Encoding.UTF8.GetBytes(_randompassword);
                     string _encryptedpassword = System.Convert.ToBase64String(pass);


                     var clientuserobj = new BusinessPOCO.User.Cl_ClientUser();
                     clientuserobj.FirstName = firstname.Trim();
                     clientuserobj.LastName = lastname.Trim();
                     clientuserobj.Username = username.Trim() + "@" + clpoco.CompanyName.Replace(" ", "").Trim();
                     clientuserobj.Password = _encryptedpassword;
                     clientuserobj.Email = email.Trim();
                     clientuserobj.MobileNo = mobileno.Trim();
                     clientuserobj.Address = address.Trim();
                     clientuserobj.IsActive = status;
                     clientuserobj.IsBlock = false;
                     clientuserobj.CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                     clientuserobj.CreatedDate = DateTime.Now;
                     clientuserobj.DetailType = employeetype;
                     clientuserobj.DetailID = clientuserdetailobj.UserDetailID;

                     clientuser.Insert(clientuserobj);
                     clientuser.Save();

                     var assignedroles = new List<BusinessPOCO.User.Cl_RoleMapping>();

                     if (!string.IsNullOrEmpty(assignedrolesid))
                     {
                         string[] rolesid = assignedrolesid.Split(',');

                         

                         foreach (var roleid in rolesid)
                         {
                             var rolemappingobj = new BusinessPOCO.User.Cl_RoleMapping();
                             rolemappingobj.RoleID = Convert.ToInt32(roleid);
                             rolemappingobj.UserID = clientuserobj.ClientUserID;
                             rolemappingobj.IsDefault = false;
                             rolemappingobj.CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                             rolemappingobj.CreatedDate = DateTime.Now;

                             rolemapping.Insert(rolemappingobj);
                             assignedroles.Add(rolemappingobj);
                         }
                     }
                     var rolemappingobj2 = new BusinessPOCO.User.Cl_RoleMapping();
                     rolemappingobj2.RoleID = employeetype;
                     rolemappingobj2.UserID = clientuserobj.ClientUserID;
                     rolemappingobj2.IsDefault = true;
                     rolemappingobj2.CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                     rolemappingobj2.CreatedDate = DateTime.Now;

                     rolemapping.Insert(rolemappingobj2);
                     assignedroles.Add(rolemappingobj2);
                     rolemapping.Save();

                     bool iswelcomeemailsent = false;
                     bool isemployeepdfcreated = false;

                     try
                     {
                         Dictionary<string, string> dic = new Dictionary<string, string>();
                         dic.Add("@username@", username.Trim() + "@" + clpoco.CompanyName.Replace(" ", "").Trim());
                         dic.Add("@password@", _randompassword.Trim());
                         dic.Add("@companyname@", clpoco.CompanyName.Replace(" ", "").Trim());
                         dic.Add("@urllink@", HelpingClass.GetProtocol() + clpoco.Subdomain + "/User/Account");

                         string htmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\NewUserEmployee.html", dic);

                         EmailManager.SendEMail("EmailConfig2", email.Trim(), null, null, "Welcome to " + clpoco.CompanyName.Replace(" ", "").Trim() + "@IndoUkLabs", htmltemplate, null);

                         iswelcomeemailsent = true;
                     }
                     catch (Exception ex)
                     {
                         iswelcomeemailsent = false;
                     }

                     try
                     {
                         string pdflink = string.Empty;
                         FileManager fmd = PdfCreator.CreateEmployeePDF(clientuserobj.ClientUserID, this.subdomainurl, this.currentdomaindb, out pdflink);

                         clientuserdetailobj.PdfLink = HelpingClass.LocalUploadPathToRelativeWebPath(pdflink);
                         clientuserdetail.Update(clientuserdetailobj);
                         clientuserdetail.Save();

                         isemployeepdfcreated = true;
                     }
                     catch (Exception ex)
                     {
                         isemployeepdfcreated = false;
                     }

                     if (iswelcomeemailsent && isemployeepdfcreated)
                     {
                         NotificationManager.FireOnClient("Account", "your account has been created", "/User/Account/MyProfile", clientuserobj.ClientUserID, ((MySession.GetClientSession(this.subdomainurl).ClientUserID)), this.currentdomaindb);
                         return WebJSResponse.ResponseSimple(new { clientuserjson = clientuserobj, clientuserdetailjson = clientuserdetailobj });
                     }
                     else
                     {
                         NotificationManager.FireOnClient("Account", "your account has been created", "/User/Account/MyProfile", clientuserobj.ClientUserID, ((MySession.GetClientSession(this.subdomainurl).ClientUserID)), this.currentdomaindb);
                         //return WebJSResponse.ResponseSWAL(SwalEnum.warning, "Created Successfully !", "Patient has been created successfully<br>but something went wrong while sending email & generating pdf.", new { clientuserjson = clientuserobj, clientuserdetailjson = clientuserdetailobj });
                         return WebJSResponse.ResponseSimple(new { clientuserjson = clientuserobj, clientuserdetailjson = clientuserdetailobj });
                     }


                 }

                 return WebJSResponse.ResponseToastr(ToastrEnum.error, "In edit phase", "Currently page is in edit phase!please try again by clicking on menu.", new { });

             }
             catch (Exception ex)
             {
                 return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
             }
         }

         [HttpPost]
         [Route("Update")]
         public JsonResult Update(int employeetype, string firstname, string lastname, string email,
             string mobileno, string address, bool status, string assignedrolesid, DateTime joiningdate, string qualifications,
             string typeofcollection, string license, bool employementtype, DateTime terminationdate, string terminationreason, int editid)
         {

             try
             {
                 Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                 Repositories.User.IClientUserTypeRepository clientusertype = this.currentdomaindb.ClientUserTypeRepository();
                 Repositories.User.IClientUserDetailRepository clientuserdetail = this.currentdomaindb.ClientUserDetailRepository();
                 Repositories.User.IRoleRepository role = this.currentdomaindb.RoleRepository();
                 Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();

                 Repositories.User.IAdminDetailRepository admindetail = this.currentdomaindb.AdminDetailRepository();
                 BusinessPOCO.User.Cl_AdminDetail _addetail = admindetail.GetAll().FirstOrDefault();

                 Repositories.Admin.ISystemUserRepository su = new LaboratoryBusiness.BLL.Admin.SystemUserRepository();
                 BusinessPOCO.Admin.SystemUser supoco = su.GetByID(_addetail.SystemClientID.Value);

                 Repositories.Admin.IClientRepository cl = new LaboratoryBusiness.BLL.Admin.ClientRepository();
                 BusinessPOCO.Admin.Client clpoco = cl.GetByID(supoco.DetailID.Value);

                 if (editid > 0)
                 {
                     if (clientuser.GetAll().Where(x => ((string.IsNullOrEmpty(x.Email) ? false : x.Email.Equals(email.Trim())) && x.ClientUserID != editid)).Count() > 0)
                     {
                         return WebJSResponse.ResponseToastr(ToastrEnum.error, "Username or email already exist !", "Please try another !", new { });

                     }


                     var clientuserobj = clientuser.GetByID(editid);
                     clientuserobj.FirstName = firstname.Trim();
                     clientuserobj.LastName = lastname.Trim();
                     clientuserobj.Email = email.Trim();
                     clientuserobj.MobileNo = mobileno.Trim();
                     clientuserobj.Address = address.Trim();
                     clientuserobj.IsActive = status;
                     clientuserobj.UpdatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                     clientuserobj.UpdatedDate = DateTime.Now;
                     clientuserobj.DetailType = employeetype;

                     clientuser.Update(clientuserobj);
                     clientuser.Save();

                     BusinessPOCO.User.Cl_ClientUserDetail clientuserdetailobj = clientuserdetail.GetByID(clientuserobj.DetailID.Value);
                     clientuserdetailobj.JoiningDate = joiningdate;
                     if (status == false)
                     {
                         clientuserdetailobj.TerminationDate = terminationdate;
                         clientuserdetailobj.TerminationReason = terminationreason;
                     }
                     else
                     {
                         clientuserdetailobj.TerminationDate = null;
                         clientuserdetailobj.TerminationReason = null;
                     }
                     clientuserdetailobj.Qualifications = qualifications;
                     if (employeetype == 3 || assignedrolesid.Contains("3"))
                     {
                         clientuserdetailobj.TypeOfCollection = typeofcollection;
                         clientuserdetailobj.License = license;
                     }
                     else
                     {
                         clientuserdetailobj.TypeOfCollection = null;
                         clientuserdetailobj.License = null;
                     }

                     if (employeetype == 4 || assignedrolesid.Contains("4"))
                     {
                         clientuserdetailobj.License = license;
                     }
                     else
                     {
                         if (!(employeetype == 3 || assignedrolesid.Contains("3")))
                         {
                             clientuserdetailobj.License = null;
                         }
                     }

                     if (employeetype == 5 || assignedrolesid.Contains("5"))
                     {
                         clientuserdetailobj.IsFulltime = employementtype;
                     }
                     else
                     {
                         clientuserdetailobj.IsFulltime = null;
                     }

                     clientuserdetailobj.UpdatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                     clientuserdetailobj.UpdatedDate = DateTime.Now;

                     clientuserdetail.Update(clientuserdetailobj);
                     clientuserdetail.Save();


                     var _currentassignedroles = rolemapping.GetAll().Where(x => x.UserID == clientuserobj.ClientUserID).ToList();

                     foreach (var currentrole in _currentassignedroles)
                     {
                         rolemapping.Delete(currentrole.RoleMappingID);
                     }

                     rolemapping.Save();

                     var assignedroles = new List<BusinessPOCO.User.Cl_RoleMapping>();

                     if (!string.IsNullOrEmpty(assignedrolesid))
                     {
                         string[] rolesid = assignedrolesid.Split(',');

                         

                         foreach (var roleid in rolesid)
                         {
                             var rolemappingobj = new BusinessPOCO.User.Cl_RoleMapping();
                             rolemappingobj.RoleID = Convert.ToInt32(roleid);
                             rolemappingobj.UserID = clientuserobj.ClientUserID;
                             rolemappingobj.IsDefault = false;
                             rolemappingobj.CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                             rolemappingobj.CreatedDate = DateTime.Now;

                             rolemapping.Insert(rolemappingobj);
                             assignedroles.Add(rolemappingobj);
                         }
                     }
                     var rolemappingobj2 = new BusinessPOCO.User.Cl_RoleMapping();
                     rolemappingobj2.RoleID = employeetype;
                     rolemappingobj2.UserID = clientuserobj.ClientUserID;
                     rolemappingobj2.IsDefault = true;
                     rolemappingobj2.CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                     rolemappingobj2.CreatedDate = DateTime.Now;

                     rolemapping.Insert(rolemappingobj2);
                     assignedroles.Add(rolemappingobj2);
                     rolemapping.Save();

                     bool isemployeepdfcreated = false;

                     try
                     {
                         string pdflink = string.Empty;
                         FileManager fmd = PdfCreator.CreateEmployeePDF(clientuserobj.ClientUserID, this.subdomainurl, this.currentdomaindb, out pdflink);

                         clientuserdetailobj.PdfLink = HelpingClass.LocalUploadPathToRelativeWebPath(pdflink);
                         clientuserdetail.Update(clientuserdetailobj);
                         clientuserdetail.Save();

                         isemployeepdfcreated = true;
                     }
                     catch (Exception ex)
                     {
                         isemployeepdfcreated = false;
                     }

                     if (isemployeepdfcreated)
                     {
                         return WebJSResponse.ResponseSimple(new { clientuserjson = clientuserobj, clientuserdetailjson = clientuserdetailobj });
                     }
                     else
                     {
                         //return WebJSResponse.ResponseSWAL(SwalEnum.warning, "Created Successfully !", "Patient has been created successfully<br>but something went wrong while sending email & generating pdf.", new { clientuserjson = clientuserobj, clientuserdetailjson = clientuserdetailobj });
                         return WebJSResponse.ResponseSimple(new { clientuserjson = clientuserobj, clientuserdetailjson = clientuserdetailobj });
                     }


                 }

                 return WebJSResponse.ResponseToastr(ToastrEnum.error, "In create phase", "Currently page is in create phase!please try again by clicking on edit button of record.", new { });

             }
             catch (Exception ex)
             {
                 return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
             }
         }

         [HttpPost]
         [Route("UploadFile")]
         public JsonResult UploadFile(int UserDetailID,bool IsUpdated)
         {
             try
             {
                 LaboratoryBusiness.Repositories.User.IClientUserAttachmentDetailRepository cluserdetailattachment = this.currentdomaindb.ClientUserAttachmentDetailRepository();
                 LaboratoryBusiness.POCO.User.Cl_ClientUserAttachmentDetail cluserdetailattachmentobj = new BusinessPOCO.User.Cl_ClientUserAttachmentDetail();

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

                     fl.UploadFile(fm, "\\EmployeeUploadedFiles", out path);

                     path = HelpingClass.LocalUploadPathToRelativeWebPath(path);

                     cluserdetailattachmentobj = new BusinessPOCO.User.Cl_ClientUserAttachmentDetail();
                     cluserdetailattachmentobj.Link = path;
                     cluserdetailattachmentobj.Extension = fm.Extension;
                     cluserdetailattachmentobj.AttachmentName = Path.GetFileNameWithoutExtension(file.FileName);
                     cluserdetailattachmentobj.CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                     cluserdetailattachmentobj.CreatedDate = DateTime.Now;
                     cluserdetailattachmentobj.UserDetailID = UserDetailID;

                     cluserdetailattachment.Insert(cluserdetailattachmentobj);
                     cluserdetailattachment.Save();

                 }
                 // Returns message that successfully uploaded  
                 if (!IsUpdated)
                 {
                     return WebJSResponse.ResponseSWAL(SwalEnum.success, "Created Successfully !", "Employee has been created successfully<br>.", new { });
                 }
                 else
                 {
                     return WebJSResponse.ResponseSWAL(SwalEnum.success, "Updated Successfully !", "Employee has been updated successfully<br>.", new { });
                 }
             }
             catch (Exception ex)
             {
                 if (!IsUpdated)
                 {
                     return WebJSResponse.ResponseSWAL(SwalEnum.warning, "Created Successfully !", "Employee has been created successfully<br>but something went wrong while uploading files.", new { });
                 }
                 else
                 {
                     return WebJSResponse.ResponseSWAL(SwalEnum.warning, "Updated Successfully !", "Employee has been updated successfully<br>but something went wrong while uploading files.", new { });
                 }
             }
         }

         [HttpPost]
         [Route("Get")]
         public JsonResult Get(int employeeid)
         {
             try
             {
                 Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                 Repositories.User.IClientUserDetailRepository clientuserdetail = this.currentdomaindb.ClientUserDetailRepository();
                 Repositories.User.IClientUserAttachmentDetailRepository clientuserattachmentdetail = this.currentdomaindb.ClientUserAttachmentDetailRepository();
                 Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();


                 if (employeeid > 0)
                 {

                     var clientuserobj = clientuser.GetByID(employeeid);
                     var clientuserdetailobj = clientuserdetail.GetAll().Where(x => x.UserDetailID == clientuserobj.DetailID.Value).FirstOrDefault();
                     var attachments = clientuserattachmentdetail.GetAll().Where(x => x.UserDetailID == clientuserdetailobj.UserDetailID).ToList();
                     if (attachments.Count == 0)
                     {
                         attachments = null;
                     }

                     var assignedroles = (from rlm in rolemapping.GetAll()
                                                              where rlm.UserID.Value == employeeid && (rlm.IsDefault.HasValue?!rlm.IsDefault.Value:true)
                                                              select rlm).ToList();
                     if (clientuserdetailobj != null && clientuserobj != null)
                     {
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
                             JoiningDateCustom = clientuserdetailobj.JoiningDate.Value.ToString("yyyy-MM-dd"),
                             Qualifications = clientuserdetailobj.Qualifications,
                             TerminationDateCustom = clientuserdetailobj.TerminationDate.HasValue?clientuserdetailobj.TerminationDate.Value.ToString("yyyy-MM-dd"):null,
                             TerminationReason = clientuserdetailobj.TerminationReason == null ? null : clientuserdetailobj.TerminationReason,
                             TypeOfCollection = clientuserdetailobj.TypeOfCollection == null ? null : clientuserdetailobj.TypeOfCollection,
                             License = clientuserdetailobj.License == null ? null : clientuserdetailobj.License,
                             IsFullTime = clientuserdetailobj.IsFulltime.HasValue?(object)clientuserdetailobj.IsFulltime.Value:null,
                             DetailType = clientuserobj.DetailType.Value
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

         [HttpPost]
         [Route("GetList")]
         public JsonResult GetList(bool status)
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
                               select new
                               {
                                   s.ClientUserID,
                                   s.FirstName,
                                   s.LastName,
                                   FullName = s.FirstName+" "+s.LastName,
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
                                   Roles = string.Join(",", ((from ar in assignedroles
                                                              join r in roles on ar.RoleID equals r.RoleID
                                                              where ar.UserID == s.ClientUserID
                                                              select new { Roles = (r.RoleName + " " + (ar.IsDefault.Value ? "Default" : "")) }))),
                                   status = s.IsActive.HasValue ? (s.IsActive.Value == true ? "Active" : "Inactive") : "Inactive",
                               }).Where(x => (x.IsActive.HasValue ? (x.IsActive.Value == status) : (false == status)) && !(x.DetailTypeID.Value == clientuserdetailtypeid_patient || x.DetailTypeID.Value == clientuserdetailtypeid_admin)).OrderByDescending(x => x.ClientUserID).ToList();


                 return WebJSResponse.ResponseSimple(new { employeejson = returnList });
             }
             catch (Exception ex)
             {
                 return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
             }
         }

         [HttpPost]
         [Route("GetDetail")]
         public JsonResult GetDetail(int employeeid)
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
                             TerminationDateCustom = clientuserdetailobj.TerminationDate.HasValue ? clientuserdetailobj.TerminationDate.Value.ToString("MM/dd/yyyy") : null,
                             TerminationReason = clientuserdetailobj.TerminationReason,
                             Qualifications = clientuserdetailobj.Qualifications,
                             TypeOfCollection = clientuserdetailobj.TypeOfCollection,
                             License = clientuserdetailobj.License,
                             EmployementType = clientuserdetailobj.IsFulltime.HasValue ? clientuserdetailobj.IsFulltime.Value?"Full-time":"Part-time" : null,
                             PdfLink=clientuserdetailobj.PdfLink,
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

         [HttpPost]
         [Route("DeleteFile")]
         public JsonResult DeleteFile(int attachmentid)
         {
             try
             {
                 Repositories.User.IClientUserAttachmentDetailRepository clientuserattachmentdetail = this.currentdomaindb.ClientUserAttachmentDetailRepository();


                 if (attachmentid > 0)
                 {
                     clientuserattachmentdetail.Delete(attachmentid);
                     clientuserattachmentdetail.Save();
                     return WebJSResponse.ResponseToastr(ToastrEnum.success, "Deleted Successfully !", "Attachment has been deleted successfully.", new { });

                 }

                 return WebJSResponse.ResponseToastr(ToastrEnum.error, "Incorrect attachment-id !", "Kindly provide correct attachment id.", new { });


             }
             catch (Exception ex)
             {
                 return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
             }
         }

         [HttpPost]
         [Route("UnblockEmployee")]
         public JsonResult UnblockEmployee(int clientuserid)
         {
             try
             {
                 Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();

                 clientuser.Unblock(clientuserid, false, this.currentdomaindb.IncorrectPasswordAttemptRepository(), this.currentdomaindb.ClientUserRepository()
                     , this.currentdomaindb.ClientUserTypeRepository(), this.currentdomaindb.AdminDetailRepository());

                 return WebJSResponse.ResponseToastr(ToastrEnum.success, "User unblocked.", "User has been unblocked succussfully!", new { });




             }
             catch (Exception ex)
             {
                 return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
             }
         }

         [HttpPost]
         [Route("ForgetPassword")]
         public JsonResult ForgetPassword(int clientuserid)
         {
             try
             {

                 Repositories.User.IClientUserRepository userrep = this.currentdomaindb.ClientUserRepository();

                 Repositories.User.IForgetPasswordRepository forgetPassword = this.currentdomaindb.ForgetPasswordRepository();

                 BusinessPOCO.User.Cl_ClientUser user = userrep.GetByID(clientuserid);
                 if (user != null)
                 {
                     Guid guid = Guid.NewGuid();
                     BusinessPOCO.User.Cl_ForgetPassword pocofp = new BusinessPOCO.User.Cl_ForgetPassword();
                     pocofp.CreatedDate = DateTime.Now;
                     pocofp.ClientUserID = user.ClientUserID;
                     pocofp.UniqueCode = guid.ToString();
                     pocofp.ExpiresInMinutes = 60 * 24;
                     forgetPassword.Insert(pocofp);


                     try
                     {
                         Dictionary<string, string> dic = new Dictionary<string, string>();
                         dic.Add("@username@", user.Username.Trim());

                         dic.Add("@urllink@", HelpingClass.GetProtocol() + this.subdomainurl + "/User/Account/" + "ChangePassword?guid=" + pocofp.UniqueCode);
                         string htmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\ForgetPassword.html", dic);

                         EmailManager.SendEMail("EmailConfig2", user.Email, null, null, "Forget Password", htmltemplate, null);
                         return WebJSResponse.ResponseSWAL(SwalEnum.success, "Sent Successfully", "Forget password email has been sent sucessfully<br>", new { });


                     }
                     catch (Exception ex)
                     {
                         return WebJSResponse.ResponseToastr(ToastrEnum.error, "something went wrong in email sending functionality", "please try again letter<br>", new { });

                     }
                 }
                 else
                 {
                     return WebJSResponse.ResponseToastr(ToastrEnum.error, "user does not exists", "please try another<br>", new { });
                 }


             }
             catch (Exception ex)
             {
                 return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
             }
         }


        [Route("ActiveEmployees")]
        [ClientAuthorizeMember]
        //[SystemAuthorizeMember]
       
        public ActionResult ActiveEmployees()
        {
            return View("~/Views/User/Employee/ActiveEmployee.cshtml");
        }
        [Route("InactiveEmployees")]
        [ClientAuthorizeMember]
       // [SystemAuthorizeMember]
       
        public ActionResult InactiveEmployees()
        {
            return View("~/Views/User/Employee/InactiveEmployees.cshtml");
        }
    }
}