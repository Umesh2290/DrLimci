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
    [RoutePrefix("User/Account")]
    public class AccountController : RedirectController
    {
        // GET: Account
        [Route]
        [ClientNonAuthorizeMember]
        public ActionResult Index()
        {
            if (!HelpingClass.ValidateConnection())
            {
                throw new Exception("LabSystemDB_Connection does not establish!");
            }

            Repositories.Admin.IClientRepository cl = new BLL.Admin.ClientRepository();
            var client = cl.GetAll().Where(x => (x.Subdomain != null ? x.Subdomain : "").Equals(this.subdomainurl)).FirstOrDefault();
            if (client != null)
            {
                if (client.LogoImgLink != null)
                {
                    ViewBag.LogoImg = client.LogoImgLink;
                }
                else
                {
                    ViewBag.LogoImg = "/Content/MainWebsite/assets/img/logo/logo.png";
                }

                if (client.BackgroundImgLink != null)
                {
                    ViewBag.BackroundImg = client.BackgroundImgLink;
                }
                else
                {
                    ViewBag.BackroundImg = "/Content/images/photo-wide-4.jpg";
                }
            }
            else
            {
                ViewBag.LogoImg = "/Content/MainWebsite/assets/img/logo/logo.png";
                ViewBag.BackroundImg = "/Content/images/photo-wide-4.jpg";
            }

            return View("~/Views/User/Account/Index.cshtml");
        }

        [HttpPost]
        [ClientNonAuthorizeMember]
        [Route("Login")]
        public JsonResult Login(string username, string password)
        {
            try
            {
                Repositories.User.IClientUserRepository userrep = currentdomaindb.ClientUserRepository();
                Repositories.User.IIncorrectPasswordAttemptRepository incorrectpassword = currentdomaindb.IncorrectPasswordAttemptRepository();

                int blockafternumofattemts = 10;//Change to 10 after testing
                int afterhourstounblock = -1;//Change to -12 after testing

                //BusinessPOCO.Admin.SystemUser user = userrep.GetAll().Where(x => x.Username.Equals(username) && x.DetailType.Value!=1).FirstOrDefault();//Uncomment when second phase live
                BusinessPOCO.User.Cl_ClientUser user = userrep.GetAll().Where(x => x.Username.ToLower().Equals(username.Trim().ToLower()) || x.Email.ToLower().Equals(username.ToLower().Trim())).FirstOrDefault();

                if (user == null)
                {
                    return WebJSResponse.ResponseToastr(ToastrEnum.error, "Username or email doesnot exist", "Please try another !", new { });
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
                        bool isunblock = userrep.Unblock(user.ClientUserID, true,this.currentdomaindb.IncorrectPasswordAttemptRepository(),this.currentdomaindb.ClientUserRepository(),this.currentdomaindb.ClientUserTypeRepository(),this.currentdomaindb.AdminDetailRepository());
                        if (!isunblock)
                        {
                            return WebJSResponse.ResponseToastr(ToastrEnum.error, "Sorry ! <br>you are currently in block status !", "Kindly wait or contact admin.", new { });
                        }
                    }

                    var attempts = incorrectpassword.GetAll().Where(x => x.ClientUserID == user.ClientUserID &&
                    (x.CreatedDate.Value.CompareTo(DateTime.Now.AddHours(afterhourstounblock)) == 0 || x.CreatedDate.Value.CompareTo(DateTime.Now.AddHours(afterhourstounblock)) == 1)
                    && x.IsInclude == true).ToList();

                    foreach (var attemp in attempts)
                    {
                        attemp.IsInclude = false;
                        incorrectpassword.Update(attemp);
                    }

                    incorrectpassword.Save();

                    ClientUserSessionContext sescontext = new ClientUserSessionContext();
                    ClientUser inneruser = ClientUser.Initialize(user,this.subdomainurl,this.currentdomaindb);
                    MySession.SetClientSession(this.subdomainurl, inneruser);

                    sescontext.SetAuthenticationToken(user.Username, false, inneruser);
                    return WebJSResponse.ResponseRedirect("/User/Home", new { });
                }

                else
                {
                    bool isunblock = userrep.Unblock(user.ClientUserID, true, this.currentdomaindb.IncorrectPasswordAttemptRepository(), this.currentdomaindb.ClientUserRepository(), this.currentdomaindb.ClientUserTypeRepository(), this.currentdomaindb.AdminDetailRepository());

                    Repositories.User.IClientUserTypeRepository cltyperepo = currentdomaindb.ClientUserTypeRepository();
                    int clientdetailtypeid = cltyperepo.GetAll().Where(x => x.TypeName.Equals("Client Admin")).FirstOrDefault().ClientUserTypeID;

                    if (!(user.IsBlock.HasValue ? user.IsBlock.Value : false) || isunblock)
                    {
                        byte[] incorrentpasswordbytes = System.Text.Encoding.UTF8.GetBytes(password);
                        string _encryptedpassword = System.Convert.ToBase64String(incorrentpasswordbytes);

                        var incorrentpasswordobj = new BusinessPOCO.User.Cl_IncorrectPasswordAttempt();
                        incorrentpasswordobj.ClientUserID = user.ClientUserID;
                        incorrentpasswordobj.CreatedDate = DateTime.Now;
                        incorrentpasswordobj.Password = _encryptedpassword;
                        incorrentpasswordobj.IsInclude = true;

                        incorrectpassword.Insert(incorrentpasswordobj);

                        incorrectpassword.Save();
                    }

                    var attempts = incorrectpassword.GetAll().Where(x => x.ClientUserID == user.ClientUserID &&
                        (x.CreatedDate.Value.CompareTo(DateTime.Now.AddHours(afterhourstounblock)) == 0 || x.CreatedDate.Value.CompareTo(DateTime.Now.AddHours(afterhourstounblock)) == 1)
                        && x.IsInclude == true).ToList();

                    if (attempts.Count == blockafternumofattemts)
                    {
                        if (!user.IsBlock.Value)
                        {
                            if (clientdetailtypeid == user.DetailType.Value)
                            {
                                Repositories.User.IAdminDetailRepository addetailrepo = currentdomaindb.AdminDetailRepository();
                                int systemclientid = addetailrepo.GetAll().Where(x => x.AdminDetailID == user.DetailID.Value).FirstOrDefault().SystemClientID.Value;
                                Repositories.Admin.ISystemUserRepository sysrepo = new BLL.Admin.SystemUserRepository();
                                var sysclient = sysrepo.GetByID(systemclientid);
                                sysclient.IsBlock = true;
                                sysrepo.Update(sysclient);
                                sysrepo.Save();
                            }
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
        [ClientAuthorizeMember]
        public ActionResult Logout()
        {
            Response.Cookies.Remove(this.subdomainurl + "cuser");
            Request.Cookies.Remove(this.subdomainurl + "cuser");
            HelpingClass.Clear(this.subdomainurl+ "cuser", this.HttpContext.ApplicationInstance.Context);
            MySession.SetClientSession(this.subdomainurl,null);
            return RedirectToRoute(new { controller = "User/Account", action = "Index" });
        }

        [Route("ChangeRole")]
        [HttpPost]
        public ActionResult ChangeRole(int id, string type)
        {
            try
            {
                if (type.Equals("Employee"))
                {
                    ClientUser tmpusr = MySession.GetClientSession(this.subdomainurl);
                    tmpusr.MenuPriority = MenuPriorityEnum.Employee;
                    MySession.SetClientSession(this.subdomainurl,tmpusr);
                }
                else
                {
                    ClientUser tmpusr = MySession.GetClientSession(this.subdomainurl);
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

                return WebJSResponse.ResponseRedirect("/User/Home", new { });
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
                List<string> _individualsectionrestriction = MySession.GetClientSession(this.subdomainurl).SectionRestrictions.Where(x => x.MenuID == menuid).Select(x => x.SectionSelector).Distinct().ToList();
                List<string> _rolebasedsectionrestriction = new List<string>();
                if (MySession.GetClientSession(this.subdomainurl).CurrentRole.IsActive.Value && MySession.GetClientSession(this.subdomainurl).MenuPriority == MenuPriorityEnum.Role)
                {
                    _rolebasedsectionrestriction = MySession.GetClientSession(this.subdomainurl).CurrentRole.SectionRestrictions.Where(x => x.MenuID == menuid).Select(x => x.SectionSelector).Distinct().ToList();
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
        [ClientNonAuthorizeMember]
        [HttpPost]
        public JsonResult ForgetPassword(string username)
        {
            try
            {

                Repositories.User.IClientUserRepository userrep = currentdomaindb.ClientUserRepository();

                Repositories.User.IForgetPasswordRepository forgetPassword = currentdomaindb.ForgetPasswordRepository();

                BusinessPOCO.User.Cl_ClientUser user = userrep.GetAll().Where(x => x.Username.Equals(username.Trim()) || x.Email.Equals(username.Trim())).FirstOrDefault();
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
                        dic.Add("@username@", username.Trim());

                        dic.Add("@urllink@", HelpingClass.GetProtocol()+this.subdomainurl+"/User/Account/" + "ChangePassword?guid=" + pocofp.UniqueCode);
                        string htmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\ForgetPassword.html", dic);

                        EmailManager.SendEMail("EmailConfig2", user.Email, null, null, "Forget Password", htmltemplate, null);
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

        [Route("ChangePassword")]
        [ClientNonAuthorizeMember]
        public ActionResult ChangePassword(string guid)
        {
            try
            {
                LaboratoryBusiness.Repositories.User.IForgetPasswordRepository forgetPassword = currentdomaindb.ForgetPasswordRepository();
                LaboratoryBusiness.POCO.User.Cl_ForgetPassword pocofp = new LaboratoryBusiness.POCO.User.Cl_ForgetPassword();
                pocofp = forgetPassword.GetByGUID(guid);
                if (pocofp != null)
                {
                    DateTime todayDate = DateTime.Now;
                    DateTime expiryDate = pocofp.CreatedDate.Value.AddMinutes(Convert.ToDouble(pocofp.ExpiresInMinutes));
                    if (todayDate > expiryDate)
                    {

                        ViewBag.IsLinkExpired = true;
                    }
                    else
                    {
                        ViewBag.IsLinkExpired = false;
                        ViewBag.ClientUserID = pocofp.ClientUserID;
                    }
                    return View("~/Views/User/Account/ChangePassword.cshtml");
                }
                else
                {

                    return Redirect("User/Error/NotFound");
                }
            }
            catch (Exception e)
            {
                ViewBag.IsLinkExpired = false;

                return View("~/Views/User/Account/ChangePassword.cshtml");
            }
        }

        [HttpPost]
        [Route("ChangePasswordSave")]
        public JsonResult ChangePasswordSave(string password, string confirmpassword, string clientUserID)
        {
            try
            {
                if (clientUserID.Contains('/'))
                {
                    clientUserID = clientUserID.Replace("/", "");
                }
                LaboratoryBusiness.Repositories.User.IClientUserRepository clUser = currentdomaindb.ClientUserRepository();
                LaboratoryBusiness.POCO.User.Cl_ClientUser pocofp = new LaboratoryBusiness.POCO.User.Cl_ClientUser();
                pocofp = clUser.GetByID(Convert.ToInt16(clientUserID));
                pocofp.Password = HelpingClass.Base64Encode(password);
                clUser.Update(pocofp);
                clUser.Save();
                try
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("@username@", pocofp.Username.Trim());
                    dic.Add("@password@", password);


                    string htmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\ChangePassword.html", dic);

                    EmailManager.SendEMail("EmailConfig2", pocofp.Email, null, null, "Password Change Confirmation", htmltemplate, null);
                    return WebJSResponse.ResponseToastr(ToastrEnum.success, "Password Change successfully check your email thanks.", "<br>", new { });


                }
                catch (Exception ex)
                {
                    return WebJSResponse.ResponseToastr(ToastrEnum.error, "something went wrong in email sending functionality", "please try again letter<br>", new { });

                }
                // forgetPassword.Update()
                //   return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" , new { });

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
                Repositories.User.INotificationRepository notificationrep = currentdomaindb.NotificationRepository();

                var notifications = notificationrep.GetAll().Where(x => x.EmployeeID == employeeid && (!x.Isviewed.HasValue || x.Isviewed.Value == false)).ToList();

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
                Repositories.User.INotificationRepository notificationrep = currentdomaindb.NotificationRepository();

                var notifications = (from nt in notificationrep.GetAll()
                                     where nt.EmployeeID.Value == employeeid
                                     select new { nt.ClickLink, nt.Icon, nt.Isviewed, nt.Title, nt.CreatedDatetime, nt.Description, TimeAgo = HelpingClass.Timeago(nt.CreatedDatetime.Value, DateTime.Now) }).OrderByDescending(x => x.CreatedDatetime).ToList();

                return WebJSResponse.ResponseSimple(new { morenotifjson = notifications });
            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("MyProfile")]
        [ClientAuthorizeMember]
        [HttpGet]
        public ActionResult MyProfile()
        {
            try
            {
                ViewBag.IsSuccessfullySave = 0;
                LaboratoryBusiness.Repositories.User.IClientUserRepository userrep = currentdomaindb.ClientUserRepository();
                LaboratoryBusiness.POCO.User.Cl_ClientUser user = userrep.GetAll().Where(x => x.ClientUserID.Equals(MySession.GetClientSession(this.subdomainurl).ClientUserID)).FirstOrDefault();


                putRolesInViewBag();
                ViewBag.username = (user.FirstName + " " + " " + user.LastName);
                ViewBag.profileimage = (string.IsNullOrEmpty(user.ProfilePic) ? "/Content/images/faces/1.jpg" : user.ProfilePic).Replace("~", "");
                return View("~/Views/User/Account/MyProfile.cshtml", user);

            }
            catch (Exception e)
            {
                return null;
            }
        }

        private void putRolesInViewBag()
        {
            string role = null;
            foreach (var dd in MySession.GetClientSession(this.subdomainurl).AssignedRoles)
            {
                role += dd.RoleName + " | ";
            }


            ViewBag.Roles = role;

        }

        [Route("SaveUserProfile")]

        [HttpPost]
        public ActionResult SaveUserProfile(LaboratoryBusiness.POCO.User.Cl_ClientUser user, HttpPostedFileBase file)
        {

            try
            {
                string path = string.Empty;
                bool isFile = false;
                if (file != null)
                {
                    MemoryStream ms = new MemoryStream();
                    file.InputStream.CopyTo(ms);

                    FileManager fm = new FileManager();
                    fm.FileBytes = ms.ToArray();
                    fm.MimeType = file.ContentType;
                    fm.Extension = Path.GetExtension(file.FileName);
                    fm.Name = Path.GetFileNameWithoutExtension(file.FileName)+DateTime.Now.ToString("ddMMyyyyhhmmss");

                    Client cl = new Client(this.subdomainurl);
                    FileInitializer fl = new FileInitializer(cl);

                    

                    fl.UploadFile(fm, "\\ProfileImages", out path);
                    path = HelpingClass.LocalUploadPathToRelativeWebPath(path);

                    isFile = true;

                }
                if (user.ClientUserID > 0)
                {
                    LaboratoryBusiness.Repositories.User.IClientUserRepository userrep = currentdomaindb.ClientUserRepository();
                    LaboratoryBusiness.Repositories.User.IAdminDetailRepository useradmin = currentdomaindb.AdminDetailRepository();
                    LaboratoryBusiness.Repositories.User.IClientUserTypeRepository usertype = currentdomaindb.ClientUserTypeRepository();

                    int clientdetailtypeid = usertype.GetAll().Where(x => x.TypeName.Equals("Client Admin")).FirstOrDefault().ClientUserTypeID;

                    LaboratoryBusiness.POCO.User.Cl_ClientUser user2 = userrep.GetAll().Where(x => x.ClientUserID.Equals(user.ClientUserID)).FirstOrDefault();


                    user2.FirstName = user.FirstName;
                    user2.LastName = user.LastName;
                    user2.Address = user.Address;
                    user2.MobileNo = user.MobileNo;
                    user2.Email = user.Email;
                    if (isFile)
                    {
                        user2.ProfilePic = path;

                    }
                    user2.UpdatedBy = user.ClientUserID;
                    user2.UpdatedDate = DateTime.Now;
                    userrep.Update(user2);
                    userrep.Save();
                    ViewBag.IsSuccessfullySave = 1;
                    ViewBag.username = (user2.FirstName + " " + user2.LastName);
                    ViewBag.profileimage = (string.IsNullOrEmpty(user2.ProfilePic) ? "/Content/images/faces/1.jpg" : user2.ProfilePic).Replace("~", "");
                    putRolesInViewBag();
                    ClientUser clus = MySession.GetClientSession(this.subdomainurl);
                    clus.ProfilePic = string.IsNullOrEmpty(user2.ProfilePic)?null:user2.ProfilePic.Replace("~", "");
                    MySession.SetClientSession(this.subdomainurl, clus);

                    clus.ReSync_MyClientUser_Session();

                    if(clientdetailtypeid == user2.DetailType.Value) 
                    {
                        LaboratoryBusiness.Repositories.Admin.ISystemUserRepository systemuser = new LaboratoryBusiness.BLL.Admin.SystemUserRepository();

                    LaboratoryBusiness.POCO.Admin.SystemUser suuser = (from su in systemuser.GetAll()
                                                                           join ad in useradmin.GetAll() on su.SystemUserID equals ad.SystemClientID
                                                                           where ad.AdminDetailID==user2.DetailID
                                                                           select su).FirstOrDefault();

                        if(suuser != null) 
                        {
                            suuser.FirstName = user2.FirstName;
                            suuser.LastName = user2.LastName;
                            suuser.Address = user2.Address;
                            suuser.MobileNo = user2.MobileNo;
                            suuser.Email = user2.Email;
                            suuser.ProfilePic = user2.ProfilePic;
                            suuser.UpdatedDate = DateTime.Now;
                            suuser.UpdatedBy = ((user.ClientUserID)*-1);

                            systemuser.Update(suuser);
                            systemuser.Save();

                        }
                    }

                    return View("~/Views/User/Account/MyProfile.cshtml", user2);

                    //  

                }
                else
                {
                    ViewBag.IsSuccessfullySave = 2;
                    putRolesInViewBag();
                    return View("~/Views/User/Account/MyProfile.cshtml", user);

                }
            }
            catch (Exception e)
            {
                ViewBag.IsSuccessfullySave = 2;
                putRolesInViewBag();
                return View("~/Views/User/Account/MyProfile.cshtml", user);

            }

        }

        [Route("ChangePasswordInternal")]

        [HttpPost]
        public ActionResult ChangePasswordInternal(string oldpassword, string newpassword, string confirmpassword)
        {
            try
            {
                LaboratoryBusiness.Repositories.User.IClientUserRepository clUser = currentdomaindb.ClientUserRepository();
                LaboratoryBusiness.Repositories.User.IAdminDetailRepository useradmin = currentdomaindb.AdminDetailRepository();
                LaboratoryBusiness.Repositories.Admin.ISystemUserRepository sysUser = new LaboratoryBusiness.BLL.Admin.SystemUserRepository();
                LaboratoryBusiness.Repositories.User.IClientUserTypeRepository usertype = currentdomaindb.ClientUserTypeRepository();

                int clientdetailtypeid = usertype.GetAll().Where(x => x.TypeName.Equals("Client Admin")).FirstOrDefault().ClientUserTypeID;

                LaboratoryBusiness.POCO.User.Cl_ClientUser pocofp = new LaboratoryBusiness.POCO.User.Cl_ClientUser();
                pocofp = clUser.GetByID(Convert.ToInt16(MySession.GetClientSession(this.subdomainurl).ClientUserID));
                pocofp.Password = HelpingClass.Base64Decode(pocofp.Password);
                if (oldpassword == pocofp.Password)
                {
                    pocofp.Password = HelpingClass.Base64Encode(newpassword);
                    clUser.Update(pocofp);
                    clUser.Save();

                    if(clientdetailtypeid == pocofp.DetailType.Value) 
                    {
                        LaboratoryBusiness.Repositories.Admin.ISystemUserRepository systemuser = new LaboratoryBusiness.BLL.Admin.SystemUserRepository();

                    LaboratoryBusiness.POCO.Admin.SystemUser suuser = (from su in systemuser.GetAll()
                                                                           join ad in useradmin.GetAll() on su.SystemUserID equals ad.SystemClientID
                                                                           where ad.AdminDetailID==pocofp.DetailID
                                                                           select su).FirstOrDefault();

                        if(suuser != null) 
                        {
                            suuser.Password = HelpingClass.Base64Encode(newpassword);

                            systemuser.Update(suuser);
                            systemuser.Save();

                        }
                    }

                    return WebJSResponse.ResponseToastr(ToastrEnum.success, "Password Change successfully.", "Password Changed <br>", new { });
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