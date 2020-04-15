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
    [RoutePrefix("User/Patients")]
    public class PatientsController : RedirectController
    {
        // GET: Patients

        [HttpPost]
        [Route("Create")]
        public JsonResult Create(string firstname, string middlename, string lastname, string username,
            string email, string mobileno, string address, bool status, string age,string gender,string streetname,
            string city,string alternateaddress, string alternatephoneno,string referingdoctor,string referinghospital,string paymentmode,
                decimal payment, int editid)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IClientUserTypeRepository clientusertype = this.currentdomaindb.ClientUserTypeRepository();
                Repositories.User.IPatientDetailRepository patientdetail = this.currentdomaindb.PatientDetailRepository();
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

                    BusinessPOCO.User.Cl_PatientDetail patientdetailobj = new BusinessPOCO.User.Cl_PatientDetail();
                    patientdetailobj.Age = age.Trim();
                    patientdetailobj.AlternateAddress = alternateaddress.Trim();
                    patientdetailobj.AlternatePhoneNo = alternatephoneno.Trim();
                    patientdetailobj.City = city.Trim();
                    patientdetailobj.CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                    patientdetailobj.CreatedDate = DateTime.Now;
                    patientdetailobj.MiddleName = middlename.Trim();
                    patientdetailobj.Payment = payment;
                    patientdetailobj.PaymentMode = paymentmode.Trim();
                    patientdetailobj.ReferingDoctor = referingdoctor.Trim();
                    patientdetailobj.ReferingHospital = referinghospital.Trim();
                    patientdetailobj.Sex = gender.Trim();
                    patientdetailobj.Streetname = streetname.Trim();
                    patientdetailobj.HospitalID= MySession.GetClientSession(this.subdomainurl).HospitalDetailID.HasValue ? MySession.GetClientSession(this.subdomainurl).HospitalDetailID.Value : 0;

                    patientdetail.Insert(patientdetailobj);

                    patientdetail.Save();

                    string _randompassword = System.Web.Security.Membership.GeneratePassword(8, 0);
                    byte[] pass = System.Text.Encoding.UTF8.GetBytes(_randompassword);
                    string _encryptedpassword = System.Convert.ToBase64String(pass);

                    int clientuserdetailtypeid = clientusertype.GetAll().Where(x => x.TypeName.Equals("Patient")).FirstOrDefault().ClientUserTypeID;

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
                    clientuserobj.DetailType = clientuserdetailtypeid;
                    clientuserobj.DetailID = patientdetailobj.PatientDetailID;

                    clientuser.Insert(clientuserobj);
                    clientuser.Save();

                    int patientroleid = role.GetAll().Where(x => x.RoleName.Equals("Patient")).FirstOrDefault().RoleID;

                    BusinessPOCO.User.Cl_RoleMapping rlm = new BusinessPOCO.User.Cl_RoleMapping();
                    rlm.UserID = clientuserobj.ClientUserID;
                    rlm.RoleID = patientroleid;
                    rlm.IsDefault = true;
                    rlm.CreatedDate = DateTime.Now;
                    rlm.CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;

                    rolemapping.Insert(rlm);
                    rolemapping.Save();

                    bool isinvoiceemailsent = false;
                    bool iswelcomeemailsent = false;


                    try
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("@username@", username.Trim() + "@" + clpoco.CompanyName.Replace(" ", "").Trim());
                        dic.Add("@password@", _randompassword.Trim());
                        dic.Add("@companyname@", clpoco.CompanyName.Replace(" ", "").Trim());
                        dic.Add("@urllink@", HelpingClass.GetProtocol() + clpoco.Subdomain + "/User/Account");

                        string htmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\NewPatient.html", dic);

                        EmailManager.SendEMail("EmailConfig2", email.Trim(), null, null, "Welcome to " + clpoco.CompanyName.Replace(" ", "").Trim() + "@IndoUkLabs", htmltemplate, null);

                        iswelcomeemailsent = true;

                    }
                    catch (Exception ex)
                    {
                        iswelcomeemailsent = false;
                    }

                    string paymentpdflink = string.Empty;
                    try
                    {

                        FileManager fmp = PdfCreator.CreatePatientPaymentPDF(clientuserobj.ClientUserID, this.subdomainurl, this.currentdomaindb, out paymentpdflink);
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("@patientname@", clientuserobj.FirstName + " " + patientdetailobj.MiddleName+" " + clientuserobj.LastName);
                        dic.Add("@companyname@", clpoco.CompanyName.Replace(" ", "").Trim());

                        string htmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\NewPatientInvoice.html", dic);

                        EmailManager.SendEMail("EmailConfig2", email.Trim(), null, null, "Welcome to " + clpoco.CompanyName.Replace(" ", "").Trim() + "@IndoUkLabs | Invoice for registration fees", htmltemplate, new List<FileManager>() { fmp });

                        isinvoiceemailsent = true;

                    }
                    catch (Exception ex)
                    {
                        isinvoiceemailsent = false;
                    }

                    string pdflink = string.Empty;
                    FileManager fmd = PdfCreator.CreatePatientPDF(clientuserobj.ClientUserID, this.subdomainurl, this.currentdomaindb, out pdflink);

                    BusinessPOCO.User.Cl_PatientDetail patientdetailobjupdate = patientdetail.GetByID(patientdetailobj.PatientDetailID);
                    patientdetailobjupdate.PaymentReceiptPdfLink = HelpingClass.LocalUploadPathToRelativeWebPath( paymentpdflink);
                    patientdetailobjupdate.PdfLink = HelpingClass.LocalUploadPathToRelativeWebPath(pdflink);

                    patientdetail.Update(patientdetailobjupdate);
                    patientdetail.Save();

                    if (iswelcomeemailsent && isinvoiceemailsent)
                    {
                        NotificationManager.FireOnClient("Account", "your account has been created", "/User/Account/MyProfile", clientuserobj.ClientUserID, ((MySession.GetClientSession(this.subdomainurl).ClientUserID)), this.currentdomaindb);
                        return WebJSResponse.ResponseSWAL(SwalEnum.success, "Created Successfully !", "Patient has been created successfully<br>", new { clientuserjson = clientuserobj, patientdetailjson = patientdetailobj });
                    }
                    else
                    {
                        NotificationManager.FireOnClient("Account", "your account has been created", "/User/Account/MyProfile", clientuserobj.ClientUserID, ((MySession.GetClientSession(this.subdomainurl).ClientUserID)), this.currentdomaindb);
                        return WebJSResponse.ResponseSWAL(SwalEnum.warning, "Created Successfully !", "Patient has been created successfully<br>but something went wrong while sending email & generating pdf.", new { clientuserjson = clientuserobj, patientdetailjson = patientdetailobj });
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
        public JsonResult Update(string firstname, string middlename, string lastname,
            string email, string mobileno, string address, bool status, string age, string gender, string streetname,
            string city, string alternateaddress, string alternatephoneno, string referingdoctor, string referinghospital, string paymentmode,
                decimal payment, int editid)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IClientUserTypeRepository clientusertype = this.currentdomaindb.ClientUserTypeRepository();
                Repositories.User.IPatientDetailRepository patientdetail = this.currentdomaindb.PatientDetailRepository();



                if (editid > 0)
                {
                    if (clientuser.GetAll().Where(x => ((string.IsNullOrEmpty(x.Email) ? false : x.Email.Equals(email.Trim())) && x.ClientUserID!= editid)).Count() > 0)
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "Username or email already exist !", "Please try another !", new { });

                    }

                    var clientuserobj = clientuser.GetByID(editid);
                    if (clientuserobj != null)
                    {
                        clientuserobj.FirstName = firstname.Trim();
                        clientuserobj.LastName = lastname.Trim();
                        clientuserobj.Email = email.Trim();
                        clientuserobj.MobileNo = mobileno.Trim();
                        clientuserobj.Address = address.Trim();
                        clientuserobj.IsActive = status;
                        clientuserobj.UpdatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                        clientuserobj.UpdatedDate = DateTime.Now;

                        clientuser.Update(clientuserobj);
                        clientuser.Save();

                        BusinessPOCO.User.Cl_PatientDetail patientdetailobj = patientdetail.GetByID(clientuserobj.DetailID.Value);

                        if (patientdetailobj != null)
                        {
                            patientdetailobj.Age = age.Trim();
                            patientdetailobj.AlternateAddress = alternateaddress.Trim();
                            patientdetailobj.AlternatePhoneNo = alternatephoneno.Trim();
                            patientdetailobj.City = city.Trim();
                            patientdetailobj.UpdatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                            patientdetailobj.UpdatedDate = DateTime.Now;
                            patientdetailobj.MiddleName = middlename.Trim();
                            patientdetailobj.Payment = payment;
                            patientdetailobj.PaymentMode = paymentmode.Trim();
                            patientdetailobj.ReferingDoctor = referingdoctor.Trim();
                            patientdetailobj.ReferingHospital = referinghospital.Trim();
                            patientdetailobj.Sex = gender.Trim();
                            patientdetailobj.Streetname = streetname.Trim();

                            patientdetail.Update(patientdetailobj);

                            patientdetail.Save();
                        }

                        bool ispdfcreated = false;
                        string paymentpdflink = string.Empty;
                        string pdflink = string.Empty;

                        try
                        {
                            FileManager fmd = PdfCreator.CreatePatientPDF(clientuserobj.ClientUserID, this.subdomainurl, this.currentdomaindb, out pdflink);
                            FileManager fmp = PdfCreator.CreatePatientPaymentPDF(clientuserobj.ClientUserID, this.subdomainurl, this.currentdomaindb, out paymentpdflink);

                            if (patientdetailobj != null)
                            {
                                patientdetailobj.PdfLink = HelpingClass.LocalUploadPathToRelativeWebPath(pdflink);
                                patientdetailobj.PaymentReceiptPdfLink = HelpingClass.LocalUploadPathToRelativeWebPath(paymentpdflink);

                                patientdetail.Update(patientdetailobj);
                                patientdetail.Save();
                            }
                            ispdfcreated = true;

                        }
                        catch (Exception ex)
                        {
                            ispdfcreated = false;
                        }


                        if (ispdfcreated)
                        {
                            return WebJSResponse.ResponseSWAL(SwalEnum.success, "Updated Successfully !", "Patient has been updated successfully<br>", new { clientuserjson = clientuserobj, patientdetailjson = patientdetailobj });
                        }
                        else
                        {
                            return WebJSResponse.ResponseSWAL(SwalEnum.success, "Updated Successfully !", "Patient has been updated successfully<br>but something went wrong while generating pdf.", new { clientuserjson = clientuserobj, patientdetailjson = patientdetailobj });
                        }
                    }

                    else
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "No patient found !", "There is no patient associated to this patient id.", new { });
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
        [Route("Get")]
        public JsonResult Get(int patientid)
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
                           FirstName= clientuserobj.FirstName,
                           LastName=clientuserobj.LastName,
                           MobileNo=clientuserobj.MobileNo,
                           Email=clientuserobj.Email,
                           Address=clientuserobj.Address,
                           IsActive=clientuserobj.IsActive,
                           ClientUserID=clientuserobj.ClientUserID,
                           Username=clientuserobj.Username,
                           MiddleName=patientdetailobj.MiddleName,
                           Payment=patientdetailobj.Payment,
                           PaymentMode=patientdetailobj.PaymentMode,
                           PaymentReceiptPdfLink=patientdetailobj.PaymentReceiptPdfLink,
                           PdfLink= patientdetailobj.PdfLink,
                           ReferingDoctor=patientdetailobj.ReferingDoctor,
                           ReferingHospital=patientdetailobj.ReferingHospital,
                           Sex=patientdetailobj.Sex,
                           Streetname=patientdetailobj.Streetname,
                           City=patientdetailobj.City,
                           AlternatePhoneNo=patientdetailobj.AlternatePhoneNo,
                           AlternateAddress=patientdetailobj.AlternateAddress,
                           Age=patientdetailobj.Age,
                           PatientDetailID=patientdetailobj.PatientDetailID
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

        [HttpPost]
        [Route("GetList")]
        public JsonResult GetList(string status)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IClientUserTypeRepository clientusertype = this.currentdomaindb.ClientUserTypeRepository();
                Repositories.User.IPatientDetailRepository patientdetail = this.currentdomaindb.PatientDetailRepository();


                int clientuserdetailtypeid = clientusertype.GetAll().Where(x => x.TypeName.Equals("Patient")).FirstOrDefault().ClientUserTypeID;
                int hospitalid = MySession.GetClientSession(this.subdomainurl).HospitalDetailID.HasValue ? MySession.GetClientSession(this.subdomainurl).HospitalDetailID.Value : 0;
                object returnList = null;
                if (status == "All")
                {
                    if (!MySession.GetClientSession(this.subdomainurl).HospitalDetailID.HasValue)
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
                                          pd.City,
                                          cl.IsBlock
                                      }).ToList();
                    }
                    else
                    {
                        returnList = (from cl in clientuser.GetAll()
                                      join pd in patientdetail.GetAll() on cl.DetailID equals pd.PatientDetailID
                                      where pd.HospitalID == hospitalid
                                      where cl.DetailType.Value == clientuserdetailtypeid
                                      select new
                                      {
                                          cl.ClientUserID,
                                          cl.Username,
                                          Status = cl.IsActive.HasValue ? (cl.IsActive.Value ? "Active" : "Inactive") : "Inactive",
                                          FullName = cl.FirstName + " " + pd.MiddleName + " " + cl.LastName,
                                          pd.Sex,
                                          pd.City,
                                          cl.IsBlock
                                      }).ToList();
                    }
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
                                      cl.IsBlock,
                                      pd.City
                                  }).Where(x=>x.Status.Equals("Active")).ToList();
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
                                      cl.IsBlock,
                                      pd.City
                                  }).Where(x => x.Status.Equals("Inactive")).ToList();
                }
                


                return WebJSResponse.ResponseSimple(new { patientjson = returnList });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("GetDetail")]
        [HttpPost]
        public JsonResult GetDetail(int clientdetailid)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IPatientDetailRepository patientdetail = this.currentdomaindb.PatientDetailRepository();


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

        [HttpPost]
        [Route("UnblockPatient")]
        public JsonResult UnblockPatient(int clientuserid)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();

                clientuser.Unblock(clientuserid, false,this.currentdomaindb.IncorrectPasswordAttemptRepository(),this.currentdomaindb.ClientUserRepository()
                    ,this.currentdomaindb.ClientUserTypeRepository(),this.currentdomaindb.AdminDetailRepository());

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
                        return WebJSResponse.ResponseSWAL(SwalEnum.success,"Sent Successfully", "Forget password email has been sent sucessfully<br>", new { });


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

        [Route]
         [ClientAuthorizeMember]
        public ActionResult Index()
        {
            ViewBag.currentdomaindb = this.currentdomaindb;

            return View("~/Views/User/Patients/Index.cshtml");
        }
        [Route("ViewAll")]
        [ClientAuthorizeMember]
       
        [HttpGet]
        public ActionResult ViewAll()
        {
            Repositories.User.IClientUserRepository user = this.currentdomaindb.ClientUserRepository();

            var activeblockedpatient = user.GetAll().Where(x => (x.IsActive.HasValue ? x.IsActive.Value : false) &&
                (x.IsBlock.HasValue ? x.IsBlock.Value : false) && x.DetailType.Value == 8).ToList();

            foreach (var patient in activeblockedpatient)
            {
                user.Unblock(patient.ClientUserID, true, this.currentdomaindb.IncorrectPasswordAttemptRepository(), this.currentdomaindb.ClientUserRepository()
                    , this.currentdomaindb.ClientUserTypeRepository(), this.currentdomaindb.AdminDetailRepository());
            }

            return View("~/Views/User/Patients/ViewAll.cshtml");
        }
    }
}