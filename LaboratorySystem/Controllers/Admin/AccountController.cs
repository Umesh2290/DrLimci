using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repositories = LaboratoryBusiness.Repositories;
using BLL = LaboratoryBusiness.BLL;
using BusinessPOCO = LaboratoryBusiness.POCO;
using System.IO;

namespace LaboratorySystem.Controllers.Admin
{
    [RoutePrefix("Admin/Account")]
    public class AccountController : RedirectController
    {
        // GET: Account
        [Route]
        [SystemNonAuthorizeMember]
        public ActionResult Index()
        {
            if (!HelpingClass.ValidateConnection())
            {
               throw new Exception("LabSystemDB_Connection does not establish!");
            }
            return View("~/Views/Admin/Account/Index.cshtml");
        }

        [HttpPost]
        [SystemNonAuthorizeMember]
        [Route("Login")]
        public JsonResult Login(string username, string password)
        {
            try
            {
                Repositories.Admin.ISystemUserRepository userrep = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.IIncorrectPasswordAttemptRepository incorrectpassword = new BLL.Admin.IncorrectPasswordAttemptRepository();
                Repositories.Admin.ISystemUserTypeRepository systemsusertype = new BLL.Admin.SystemUserTypeRepository();

                int blockafternumofattemts = 3;//Change to 10 after testing
                int afterhourstounblock = -1;//Change to -12 after testing

                //BusinessPOCO.Admin.SystemUser user = userrep.GetAll().Where(x => x.Username.Equals(username) && x.DetailType.Value!=1).FirstOrDefault();//Uncomment when second phase live
                BusinessPOCO.Admin.SystemUser user = userrep.GetAll().Where(x => x.Username.ToLower().Equals(username.Trim().ToLower()) || x.Email.ToLower().Equals(username.Trim().ToLower())).FirstOrDefault();

                if (user == null)
                {
                    return WebJSResponse.ResponseToastr(ToastrEnum.error, "Username or email does not exist", "Please try another !", new { });
                }

                int clientdetailtypeid = systemsusertype.GetAll().Where(x => x.TypeName.Equals("Client")).FirstOrDefault().SystemUserTypeID;
                if (user.DetailType.Value == clientdetailtypeid)
                {
                    return WebJSResponse.ResponseToastr(ToastrEnum.error, "Username or email does not exist", "Please try another !(Code:CL)", new { });
                }


                byte[] pass = System.Convert.FromBase64String(user.Password);
                if (System.Text.ASCIIEncoding.ASCII.GetString(pass).Equals(password))
                {
                    if (user.IsActive.HasValue ? !user.IsActive.Value : true)
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "Sorry ! <br>you are currently inactive", "Please try later !", new { });
                    }

                    if (user.IsBlock.HasValue ? user.IsBlock.Value : false)
                    {
                        bool isunblock = userrep.Unblock(user.SystemUserID, true);
                        if (!isunblock)
                        {
                            return WebJSResponse.ResponseToastr(ToastrEnum.error, "Sorry ! <br>you are currently in block status !", "Kindly wait or contact admin.", new { });
                        }
                    }

                    var attempts = incorrectpassword.GetAll().Where(x => x.SystemUserID == user.SystemUserID &&
                    (x.CreatedDate.Value.CompareTo(DateTime.Now.AddHours(afterhourstounblock)) == 0 || x.CreatedDate.Value.CompareTo(DateTime.Now.AddHours(afterhourstounblock)) == 1)
                    && x.IsInclude == true).ToList();

                    foreach (var attemp in attempts)
                    {
                        attemp.IsInclude = false;
                        incorrectpassword.Update(attemp);
                    }

                    incorrectpassword.Save();

                    SystemUserSessionContext sescontext = new SystemUserSessionContext();
                    SystemUser inneruser = SystemUser.Initialize(user);
                    MySession.SystemSession = inneruser;

                    //For Testing Client db accessing
                    //DBInitializer clientdb = new DBInitializer(inneruser.Client);
                    //var menus = clientdb.MenuRepository();
                    //var abc = menus.GetAll().ToList();

                    //FileManager fm1 = new FileManager("https://res.cloudinary.com/demo/image/upload/q_60/sample.jpg");
                    //FileManager fm1 = new FileManager(@"E:\LabProject2\LaboratorySystem\LaboratorySystem\bin\ScriptFile.txt");
                    //FileManager fm2 = new FileManager(@"E:\LabProject2\LaboratorySystem\LaboratorySystem\bin\ScriptFile.txt");
                    //FileManager zippedfile = ZipManager.ListOfFilesToZipFile("myzip", new List<FileManager>() { fm1, fm2 });

                    //string htmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(@"E:\LabProject2\LaboratorySystem\LaboratorySystem\bin\testhtml.html", null);

                    //FileManager fm3 = PdfManager.HtmlToPdf("Invoice", htmltemplate);

                    //EmailManager.SendEMail("EmailConfig1", "dev.tahajamali@gmail.com", null, null, "Test Email", htmltemplate, new List<FileManager>() { zippedfile, fm3 });

                    //FileInitializer fin = new FileInitializer(inneruser.Client);
                    //fin.UploadFile(zippedfile, "\\TestFolder\\Zips");

                    //RealTimeBroadcaster.BroadCast("s-"+MySession.SystemSession.SystemUserID.ToString(), "my message");

                    sescontext.SetAuthenticationToken(user.Username, false, inneruser);
                    return WebJSResponse.ResponseRedirect("/Admin/Home", new { });
                }

                else
                {
                    bool isunblock = userrep.Unblock(user.SystemUserID, true);


                    if (!(user.IsBlock.HasValue?user.IsBlock.Value:false) || isunblock)
                    {
                        byte[] incorrentpasswordbytes = System.Text.Encoding.UTF8.GetBytes(password);
                        string _encryptedpassword = System.Convert.ToBase64String(incorrentpasswordbytes);

                        var incorrentpasswordobj = new BusinessPOCO.Admin.IncorrectPasswordAttempt();
                        incorrentpasswordobj.SystemUserID = user.SystemUserID;
                        incorrentpasswordobj.CreatedDate = DateTime.Now;
                        incorrentpasswordobj.Password = _encryptedpassword;
                        incorrentpasswordobj.IsInclude = true;

                        incorrectpassword.Insert(incorrentpasswordobj);

                        incorrectpassword.Save();
                    }

                    var attempts = incorrectpassword.GetAll().Where(x => x.SystemUserID == user.SystemUserID &&
                        (x.CreatedDate.Value.CompareTo(DateTime.Now.AddHours(afterhourstounblock)) == 0 || x.CreatedDate.Value.CompareTo(DateTime.Now.AddHours(afterhourstounblock)) == 1)
                        && x.IsInclude == true).ToList();

                    if (attempts.Count == blockafternumofattemts)
                    {
                        if (!user.IsBlock.Value)
                        {
                            user.IsBlock = true;
                            userrep.Update(user);

                            userrep.Save();
                        }

                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "After " + blockafternumofattemts.ToString() + " unsuccessfull attemps you are now blocked.", "Kindly wait " + (afterhourstounblock * -1).ToString() + " hours or contact admin.", user);

                    }

                    return WebJSResponse.ResponseToastr(ToastrEnum.error, "Username or password is incorrect", "Please try another !", user);
                }
            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message + "<br>" + (ex.InnerException == null ? "Inner exception not available" : ex.InnerException.Message) + "<br>" + ex.Source, new { });
            }


        }

        [Route("Logout")]
        [SystemAuthorizeMember]
        public ActionResult Logout()
        {
            Response.Cookies.Remove(Request.Url.Host + "suser");
            Request.Cookies.Remove(Request.Url.Host + "suser");
            HelpingClass.Clear(Request.Url.Host + "suser", this.HttpContext.ApplicationInstance.Context);
            MySession.SystemSession = null;
            return RedirectToRoute(new { controller = "Admin/Account", action = "Index" });
        }

        [Route("ChangeRole")]
        [HttpPost]
        public ActionResult ChangeRole(int id, string type)
        {
            try
            {
                if (type.Equals("Employee"))
                {
                    SystemUser tmpusr = MySession.SystemSession;
                    tmpusr.MenuPriority = MenuPriorityEnum.Employee;
                    MySession.SystemSession = tmpusr;
                }
                else
                {
                    SystemUser tmpusr = MySession.SystemSession;
                    var role = tmpusr.AssignedRoles.Where(x => x.RoleID == id).FirstOrDefault();
                    if (role == null)
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "Role does not exist.", "Oops,something went wrong!", new { });
                    }

                    else
                    {
                        tmpusr.CurrentRole = role;
                        tmpusr.MenuPriority = MenuPriorityEnum.Role;
                    }
                }

                return WebJSResponse.ResponseRedirect("/Admin/Home", new { });
            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("SectionRestriction")]
        [HttpPost]
        public JsonResult SectionRestriction(int menuid)
        {
            try
            {
                List<string> _individualsectionrestriction = MySession.SystemSession.SectionRestrictions.Where(x => x.MenuID == menuid).Select(x => x.SectionSelector).Distinct().ToList();
                List<string> _rolebasedsectionrestriction = new List<string>();
                if (MySession.SystemSession.CurrentRole.IsActive.Value && MySession.SystemSession.MenuPriority == MenuPriorityEnum.Role)
                {
                    _rolebasedsectionrestriction = MySession.SystemSession.CurrentRole.SectionRestrictions.Where(x => x.MenuID == menuid).Select(x => x.SectionSelector).Distinct().ToList();
                }

                List<string> unionresult = _individualsectionrestriction.Union(_rolebasedsectionrestriction).ToList();

                return WebJSResponse.ResponseSimple(new { SectionList = unionresult });
            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }
        [Route("ForgetPassword")]
        [SystemNonAuthorizeMember]
        [HttpPost]
        public JsonResult ForgetPassword(string username)
        {
            try
            {

                Repositories.Admin.ISystemUserRepository userrep = new BLL.Admin.SystemUserRepository();

                Repositories.Admin.IForgetPasswordRepository forgetPassword = new BLL.Admin.ForgetPasswordRepository();
                Repositories.Admin.ISystemUserTypeRepository systemsusertype = new BLL.Admin.SystemUserTypeRepository();

                BusinessPOCO.Admin.SystemUser user = userrep.GetAll().Where(x => x.Username.Equals(username.Trim()) || x.Email.Equals(username.Trim())).FirstOrDefault();
                if (user != null)
                {
                    int clientdetailtypeid = systemsusertype.GetAll().Where(x => x.TypeName.Equals("Client")).FirstOrDefault().SystemUserTypeID;
                    if (user.DetailType.Value == clientdetailtypeid)
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "username or email does not exists", "please try another(Code:CL)<br>", new { });
                    }
                    Guid guid = Guid.NewGuid();
                    BusinessPOCO.Admin.ForgetPassword pocofp = new BusinessPOCO.Admin.ForgetPassword();
                    pocofp.CreatedDate = DateTime.Now;
                    pocofp.SystemUserID = user.SystemUserID;
                    pocofp.UniqueCode = guid.ToString();
                    pocofp.ExpiresInMinutes = 60 * 24;
                    forgetPassword.Insert(pocofp);


                    try
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("@username@", username.Trim());

                        dic.Add("@urllink@", HelpingClass.CurrentWebUrl() + "ChangePassword?guid=" + pocofp.UniqueCode);
                        string htmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\ForgetPassword.html", dic);

                        EmailManager.SendEMail("EmailConfig1", user.Email, null, null, "Forget Password", htmltemplate, null);
                        return WebJSResponse.ResponseToastr(ToastrEnum.success, "Forget password email has been send sucessfully", "<br>", new { });


                    }
                    catch (Exception ex)
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "something went wrong in email sending functionality", "please try again letter<br>", new { });

                    }
                }
                else
                {
                    return WebJSResponse.ResponseToastr(ToastrEnum.error, "username or email does not exists", "please try another<br>", new { });
                }


            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("UpdateNotification")]
        [HttpPost]
        public JsonResult UpdateNotification(int employeeid)
        {
            try
            {
                Repositories.Admin.INotificationRepository notificationrep = new BLL.Admin.NotificationRepository();

                var notifications = notificationrep.GetAll().Where(x => x.EmployeeID == employeeid && (!x.Isviewed.HasValue || x.Isviewed.Value==false)).ToList();

                foreach (var notification in notifications)
                {
                    notification.Isviewed = true;
                    notification.ViewedDatetime = DateTime.Now;

                    notificationrep.Update(notification);
                }

                notificationrep.Save();

                int updatednotifcounts = notificationrep.GetAll().Where(x => x.EmployeeID == employeeid && (!x.Isviewed.HasValue || x.Isviewed.Value == false)).Count();
                return WebJSResponse.ResponseSimple(new { notifjson = new { Count = updatednotifcounts } });
            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("MoreNotification")]
        [HttpPost]
        public JsonResult MoreNotification(int employeeid)
        {
            try
            {
                Repositories.Admin.INotificationRepository notificationrep = new BLL.Admin.NotificationRepository();

                var notifications = (from nt in notificationrep.GetAll()
                                     where nt.EmployeeID.Value == employeeid
                                     select new { nt.ClickLink,nt.Icon,nt.Isviewed,nt.Title,nt.CreatedDatetime,nt.Description,TimeAgo=HelpingClass.Timeago(nt.CreatedDatetime.Value,DateTime.Now)}).OrderByDescending(x=>x.CreatedDatetime).ToList();

                return WebJSResponse.ResponseSimple(new { morenotifjson = notifications });
            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("MyProfile")]
        [SystemAuthorizeMember]
        [HttpGet]
        public ActionResult MyProfile()
        {
            try
            {
                ViewBag.IsSuccessfullySave = 0;
                LaboratoryBusiness.Repositories.Admin.ISystemUserRepository userrep = new LaboratoryBusiness.BLL.Admin.SystemUserRepository();
                LaboratoryBusiness.POCO.Admin.SystemUser user = userrep.GetAll().Where(x => x.SystemUserID.Equals(MySession.SystemSession.SystemUserID)).FirstOrDefault();


                putRolesInViewBag();
                ViewBag.username = (user.FirstName+" "+user.MiddleName+" "+user.LastName);
                ViewBag.profileimage = (string.IsNullOrEmpty(user.ProfilePic) ? "/Content/images/faces/1.jpg" : user.ProfilePic).Replace("~", "");
                return View("~/Views/Admin/Account/MyProfile.cshtml", user);

            }
            catch (Exception e)
            {
                return null;
            }
        }
        private void putRolesInViewBag()
        {
            string role = null;
            foreach (var dd in MySession.SystemSession.AssignedRoles)
            {
                role += dd.RoleName + " | ";
            }


            ViewBag.Roles = role;

        }

        [Route("SaveUserProfile")]

        [HttpPost]
        public ActionResult SaveUserProfile(LaboratoryBusiness.POCO.Admin.SystemUser user, HttpPostedFileBase file)
        {

            try
            {
                bool isFile = false;
                if (file != null)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/ProfileImages/" + MySession.SystemSession.Username + "/"), Path.GetFileName(file.FileName)); // file path
                    System.IO.FileInfo file2 = new System.IO.FileInfo(Server.MapPath("~/Content/ProfileImages/" + MySession.SystemSession.Username + "/"));
                    file2.Directory.Create(); // If the directory already exists, this method does nothing.
                    file.SaveAs(path);
                    isFile = true;

                }
                if (user.SystemUserID > 0)
                {
                    LaboratoryBusiness.Repositories.Admin.ISystemUserRepository userrep = new LaboratoryBusiness.BLL.Admin.SystemUserRepository();
                    LaboratoryBusiness.POCO.Admin.SystemUser user2 = userrep.GetAll().Where(x => x.SystemUserID.Equals(user.SystemUserID)).FirstOrDefault();

                    user2.FirstName = user.FirstName;
                    user2.MiddleName = user.MiddleName;
                    user2.LastName = user.LastName;
                    user2.Address = user.Address;
                    user2.MobileNo = user.MobileNo;
                    user2.Email = user.Email;
                    if (isFile)
                    {
                        user2.ProfilePic = "~/Content/ProfileImages/" + MySession.SystemSession.Username + "/" + Path.GetFileName(file.FileName);

                    }
                    user2.UpdatedBy = user.SystemUserID;
                    user2.UpdatedDate = DateTime.Now;
                    user2.IsSaveSuccessfully = 1;
                    userrep.Update(user2);
                    userrep.Save();
                    ViewBag.IsSuccessfullySave = 1;
                    ViewBag.username = (user2.FirstName + " " + user2.MiddleName + " " + user2.LastName);
                    ViewBag.profileimage = (string.IsNullOrEmpty(user2.ProfilePic) ? "/Content/images/faces/1.jpg" : user2.ProfilePic).Replace("~", "");
                    putRolesInViewBag();
                    MySession.SystemSession.ProfilePic = string.IsNullOrEmpty(user2.ProfilePic)?null :user2.ProfilePic.Replace("~", "");
                    return View("~/Views/Admin/Account/MyProfile.cshtml", user2);

                    //  

                }
                else
                {
                    ViewBag.IsSuccessfullySave = 2;
                    putRolesInViewBag();
                    return View("~/Views/Admin/Account/MyProfile.cshtml", user);

                }
            }
            catch (Exception e)
            {
                ViewBag.IsSuccessfullySave = 2;
                putRolesInViewBag();
                return View("~/Views/Admin/Account/MyProfile.cshtml", user);

            }

        }

        [Route("ChangePasswordInternal")]

        [HttpPost]
        public ActionResult ChangePasswordInternal(string oldpassword, string newpassword, string confirmpassword)
        {
            try
            {
                LaboratoryBusiness.Repositories.Admin.ISystemUserRepository sysUser = new LaboratoryBusiness.BLL.Admin.SystemUserRepository();
                LaboratoryBusiness.POCO.Admin.SystemUser pocofp = new LaboratoryBusiness.POCO.Admin.SystemUser();
                pocofp = sysUser.GetByID(Convert.ToInt16(MySession.SystemSession.SystemUserID));
                pocofp.Password = HelpingClass.Base64Decode(pocofp.Password);
                if (oldpassword == pocofp.Password)
                {
                    pocofp.Password = HelpingClass.Base64Encode(newpassword);
                    sysUser.Update(pocofp);
                    sysUser.Save();
                    return WebJSResponse.ResponseToastr(ToastrEnum.success, "Password Changed Successfully.", "Please check your email. <br>", new { });
                }
                else
                {
                    return WebJSResponse.ResponseToastr(ToastrEnum.error, "old password is incorrect", "please try another<br>", new { });
                }

            }
            catch (Exception exx)
            {

                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + exx.Message, new { });
            }

        }

    }
}