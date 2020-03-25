using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repositories = LaboratoryBusiness.Repositories;
using BLL = LaboratoryBusiness.BLL;
using BusinessPOCO = LaboratoryBusiness.POCO;

namespace LaboratorySystem.Controllers.Admin
{
    [RoutePrefix("Admin/Employee")]
    public class EmployeeController : RedirectController
    {

        // GET: Plan
        [Route]
        // GET: Home
        [SystemAuthorizeMember]
        public ActionResult Index()
        {
            return View("~/Views/Admin/Employee/Index.cshtml");
        }

        [HttpPost]
        [Route("Create")]
        public JsonResult Create(string firstname, string middlename, string lastname, string username, string email, string mobileno, string address, bool status, string assignedrolesid,int defaultrole, int editid)
        {
            try
            {
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.ISystemUserTypeRepository systemsusertype = new BLL.Admin.SystemUserTypeRepository();
                Repositories.Admin.IRoleRepository role = new BLL.Admin.RoleRepository();
                Repositories.Admin.IRoleMappingRepository rolemapping = new BLL.Admin.RoleMappingRepository();

                string maincompanyname = HelpingClass.MainCompanyName();

                if (editid == 0)
                {
                    if (systemuser.GetAll().Where(x => x.Username.Trim().Equals(username.Trim() + "@" + maincompanyname) || (string.IsNullOrEmpty(x.Email) ? false : x.Email.Equals(email.Trim()))).Count() > 0)
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "Username or email already exist !", "Please try another !", new { });

                    }



                    string _randompassword = System.Web.Security.Membership.GeneratePassword(8, 0);
                    byte[] pass = System.Text.Encoding.UTF8.GetBytes(_randompassword);
                    string _encryptedpassword = System.Convert.ToBase64String(pass);

                    int employeedetailtypeid = systemsusertype.GetAll().Where(x => x.TypeName.Equals("Employee")).FirstOrDefault().SystemUserTypeID;

                    var systemuserobj = new BusinessPOCO.Admin.SystemUser();
                    systemuserobj.FirstName = firstname.Trim();
                    systemuserobj.MiddleName = middlename.Trim();
                    systemuserobj.LastName = lastname.Trim();
                    systemuserobj.Username = username.Trim() + "@" + maincompanyname;
                    systemuserobj.Password = _encryptedpassword;
                    systemuserobj.Email = email.Trim();
                    systemuserobj.MobileNo = mobileno.Trim();
                    systemuserobj.Address = address.Trim();
                    systemuserobj.IsActive = status;
                    systemuserobj.IsBlock = false;
                    systemuserobj.CreatedBy = MySession.SystemSession.SystemUserID;
                    systemuserobj.CreatedDate = DateTime.Now;
                    systemuserobj.DetailType = employeedetailtypeid;
                    systemuserobj.DetailID = 0;

                    systemuser.Insert(systemuserobj);

                    systemuser.Save();

                    string[] rolesid = assignedrolesid.Split(',');

                   var assignedroles = new List<BusinessPOCO.Admin.RoleMapping>();

                    foreach (var roleid in rolesid)
                    {
                        var rolemappingobj = new BusinessPOCO.Admin.RoleMapping();
                        rolemappingobj.RoleID = Convert.ToInt32(roleid);
                        rolemappingobj.UserID = systemuserobj.SystemUserID;
                        rolemappingobj.IsDefault = (Convert.ToInt32(roleid)==defaultrole);
                        rolemappingobj.CreatedBy = MySession.SystemSession.SystemUserID;
                        rolemappingobj.CreatedDate = DateTime.Now;

                        rolemapping.Insert(rolemappingobj);
                        assignedroles.Add(rolemappingobj);
                    }

                    rolemapping.Save();

                    try
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("@username@", username.Trim());
                        dic.Add("@password@", _randompassword.Trim());
                        dic.Add("@urllink@", HelpingClass.CurrentWebUrl() + "Admin/Account");

                        string htmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\NewEmployee.html", dic);

                        EmailManager.SendEMail("EmailConfig1", email.Trim(), null, null, "Welcome Onboard to IndoUkLabs", htmltemplate, null);
                        NotificationManager.Fire("Account", "your account has been created", "/Admin/Account/MyProfile", systemuserobj.SystemUserID, MySession.SystemSession.SystemUserID);

                    }
                    catch (Exception ex)
                    {
                        return WebJSResponse.ResponseSWAL(SwalEnum.warning, "Created Successfully !", "Employee has been created successfully but something went wrong while sending email<br>" + ex.Message, new { systemuserjson = systemuserobj, assignedrolesjson = assignedroles });
                    }
                    return WebJSResponse.ResponseSWAL(SwalEnum.success, "Created Successfully !", "Employee has been created successfully<br>", new { systemuserjson = systemuserobj, assignedrolesjson = assignedroles });

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
        public JsonResult Update(string firstname, string middlename, string lastname, string email, string mobileno, string address, bool status, string assignedrolesid, int defaultrole, int editid)
        {
            try
            {
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.IRoleRepository role = new BLL.Admin.RoleRepository();
                Repositories.Admin.IRoleMappingRepository rolemapping = new BLL.Admin.RoleMappingRepository();

                if (editid > 0)
                {
                    if (systemuser.GetAll().Where(x => (string.IsNullOrEmpty(x.Email) ? false : x.Email.Equals(email.Trim())) && x.SystemUserID != editid).Count() > 0)
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "Email already exist !", "Please try another !", new { });

                    }

                    var systemuserobj = systemuser.GetByID(editid);
                    if (systemuserobj != null)
                    {
                        systemuserobj.FirstName = firstname.Trim();
                        systemuserobj.MiddleName = middlename.Trim();
                        systemuserobj.LastName = lastname.Trim();
                        systemuserobj.Email = email.Trim();
                        systemuserobj.MobileNo = mobileno.Trim();
                        systemuserobj.Address = address.Trim();
                        systemuserobj.IsActive = status;
                        systemuserobj.UpdatedBy = MySession.SystemSession.SystemUserID;
                        systemuserobj.UpdatedDate = DateTime.Now;

                        systemuser.Update(systemuserobj);

                        systemuser.Save();

                        var _currentassignedroles = rolemapping.GetAll().Where(x => x.UserID == systemuserobj.SystemUserID).ToList();

                        foreach (var currentrole in _currentassignedroles)
                        {
                            rolemapping.Delete(currentrole.RoleMappingID);
                        }

                        rolemapping.Save();

                        string[] rolesid = assignedrolesid.Split(',');

                        var assignedroles = new List<BusinessPOCO.Admin.RoleMapping>();

                        foreach (var roleid in rolesid)
                        {
                            var rolemappingobj = new BusinessPOCO.Admin.RoleMapping();
                            rolemappingobj.RoleID = Convert.ToInt32(roleid);
                            rolemappingobj.UserID = systemuserobj.SystemUserID;
                            rolemappingobj.IsDefault = (Convert.ToInt32(roleid) == defaultrole);
                            rolemappingobj.CreatedBy = MySession.SystemSession.SystemUserID;
                            rolemappingobj.CreatedDate = DateTime.Now;

                            rolemapping.Insert(rolemappingobj);
                            assignedroles.Add(rolemappingobj);
                        }

                        rolemapping.Save();


                        return WebJSResponse.ResponseSWAL(SwalEnum.success, "Updated Successfully !", "Employee has been updated successfully<br>", new { systemuserjson = systemuserobj, assignedrolesjson = assignedroles });
                    }

                    else
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "No employee found !", "There is no employee associated to this employee id.", new { });
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
        public JsonResult Get(int employeeid)
        {
            try
            {
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.IRoleMappingRepository rolemapping = new BLL.Admin.RoleMappingRepository();

                if (employeeid > 0)
                {

                    var systemuserobj = systemuser.GetByID(employeeid);
                    if (systemuserobj != null)
                    {
                        var assignedroles = rolemapping.GetAll().Where(x=>x.UserID==systemuserobj.SystemUserID).ToList();
                        return WebJSResponse.ResponseSimple(new { systemuserjson = systemuserobj, assignedrolesjson = assignedroles });
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
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.IRoleRepository role = new BLL.Admin.RoleRepository();
                Repositories.Admin.IRoleMappingRepository rolemapping = new BLL.Admin.RoleMappingRepository();
                Repositories.Admin.ISystemUserTypeRepository systemsusertype = new BLL.Admin.SystemUserTypeRepository();

                var roles = role.GetAll().ToList();
                var assignedroles = rolemapping.GetAll().ToList();

                int employeedetailtypeid = systemsusertype.GetAll().Where(x => x.TypeName.Equals("Employee")).FirstOrDefault().SystemUserTypeID;

                object returnlist = null;
                returnlist = (from s in systemuser.GetAll()
                              select new
                              {
                                  s.SystemUserID,
                                  s.FirstName,
                                  s.MiddleName,
                                  s.LastName,
                                  s.MobileNo,
                                  s.ProfilePic,
                                  s.Username,
                                  s.IsActive,
                                  s.Email,
                                  s.Address,
                                  s.DetailID,
                                  DetailTypeID = s.DetailType,
                                  DetailType = "Employee",
                                  s.CreatedBy,
                                  s.CreatedDate,
                                  s.UpdatedBy,
                                  s.UpdatedDate,
                                  s.IsBlock,
                                  Roles = string.Join(",", ((from ar in assignedroles
                                           join r in roles on ar.RoleID equals r.RoleID
                                           where ar.UserID == s.SystemUserID
                                           select new { Roles = (r.RoleName+" "+(ar.IsDefault.Value?"Default":""))}))),
                                  status = s.IsActive.HasValue ? (s.IsActive.Value == true ? "Active" : "Inactive") : "Inactive",
                              }).Where(x => (x.IsActive.HasValue ? (x.IsActive.Value == status) : (false == status)) && x.DetailTypeID.Value == employeedetailtypeid).OrderByDescending(x => x.SystemUserID).ToList();



                return WebJSResponse.ResponseSimple(new { employeejson = returnlist });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [HttpPost]
        [Route("GetDetail")]
        public JsonResult GetDetail(int systemuserid)
        {
            try
            {
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.IRoleRepository role = new BLL.Admin.RoleRepository();
                Repositories.Admin.IRoleMappingRepository rolemapping = new BLL.Admin.RoleMappingRepository();


                object returnlist = null;
                object assignedroles = null;
                var data = systemuser.GetByID(systemuserid);
                if (data != null)
                {
                    var roles = role.GetAll().ToList();
                    assignedroles = (from rm in rolemapping.GetAll()
                                        join r in roles on rm.RoleID equals r.RoleID
                                        where rm.UserID == data.SystemUserID
                                        select new { r.RoleName,IsDefault=(rm.IsDefault.Value?"Yes":"No") }).ToList();
                        returnlist = new
                        {
                            data.SystemUserID,
                            data.FirstName,
                            data.MiddleName,
                            data.LastName,
                            data.MobileNo,
                            data.ProfilePic,
                            data.Username,
                            data.IsActive,
                            data.Email,
                            data.Address,
                            data.DetailID,
                            DetailType = "Employee",
                            data.CreatedBy,
                            data.CreatedDate,
                            data.UpdatedBy,
                            data.UpdatedDate,
                            data.IsBlock,
                            status = data.IsActive.HasValue ? (data.IsActive.Value == true ? "Active" : "Inactive") : "Inactive",
                        };
                }

                return WebJSResponse.ResponseSimple(new { employeejson = returnlist, assignedrolesjson = assignedroles });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }


        [HttpPost]
        [Route("UnblockEmployee")]
        public JsonResult UnblockEmployee(int systemuserid)
        {
            try
            {
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();

                systemuser.Unblock(systemuserid, false);

                return WebJSResponse.ResponseToastr(ToastrEnum.success, "User unblocked.", "User has been unblocked succussfully!", new { });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("ActiveEmployees")]
        [SystemAuthorizeMember]
        public ActionResult ActiveEmployees()
        {
            Repositories.Admin.ISystemUserRepository user = new BLL.Admin.SystemUserRepository();

            var activeblockedemployees = user.GetAll().Where(x => (x.IsActive.HasValue ? x.IsActive.Value : false) &&
                (x.IsBlock.HasValue ? x.IsBlock.Value : false) && x.DetailType.Value == 2).ToList();

            foreach (var employee in activeblockedemployees)
            {
                user.Unblock(employee.SystemUserID, true);
            }

            return View("~/Views/Admin/Employee/ActiveEmployees.cshtml");
        }

        [Route("InactiveEmployees")]
        [SystemAuthorizeMember]
        public ActionResult InactiveEmployees()
        {
            Repositories.Admin.ISystemUserRepository user = new BLL.Admin.SystemUserRepository();

            var inactiveblockedemployees = user.GetAll().Where(x => (x.IsActive.HasValue ? x.IsActive.Value : false) &&
                (x.IsBlock.HasValue ? x.IsBlock.Value : false) && x.DetailType.Value == 2).ToList();

            foreach (var employee in inactiveblockedemployees)
            {
                user.Unblock(employee.SystemUserID, true);
            }

            return View("~/Views/Admin/Employee/InactiveEmployees.cshtml");
        } 
    }
}