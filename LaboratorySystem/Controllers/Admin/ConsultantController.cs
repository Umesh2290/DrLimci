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
    [RoutePrefix("Admin/Consultant")]
    public class ConsultantController : RedirectController
    {
        // GET: Plan
        [Route]
        // GET: Home
        [SystemAuthorizeMember]
        public ActionResult Index()
        {
            return View("~/Views/Admin/Consultant/Index.cshtml");
        }

        [HttpPost]
        [Route("Create")]
        public JsonResult Create(string firstname, string middlename, string lastname,string username, string email, string mobileno, string address,bool status,string specialisationfield,int editid)
        {
            try
            {
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.IConsultantRepository consultant = new BLL.Admin.ConsultantRepository();
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

                    BusinessPOCO.Admin.Consultant consultantobj = new BusinessPOCO.Admin.Consultant();
                    consultantobj.SpecialisationAreas = specialisationfield.Trim();
                    consultantobj.CreatedBy = MySession.SystemSession.SystemUserID;
                    consultantobj.CreatedDate = DateTime.Now;

                    consultant.Insert(consultantobj);

                    consultant.Save();


                    string _randompassword  = System.Web.Security.Membership.GeneratePassword(8,0);
                    byte[] pass = System.Text.Encoding.UTF8.GetBytes(_randompassword);
                    string _encryptedpassword = System.Convert.ToBase64String(pass);

                    int consultantdetailtypeid = systemsusertype.GetAll().Where(x=>x.TypeName.Equals("Consultant")).FirstOrDefault().SystemUserTypeID;

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
                    systemuserobj.DetailType = consultantdetailtypeid;
                    systemuserobj.DetailID = consultantobj.ConsultantID;

                    systemuser.Insert(systemuserobj);

                    systemuser.Save();

                    int roleid = role.GetAll().Where(x => x.RoleName.Equals("Consultant")).FirstOrDefault().RoleID;

                    var rolemappingobj = new BusinessPOCO.Admin.RoleMapping();
                    rolemappingobj.RoleID = roleid;
                    rolemappingobj.UserID = systemuserobj.SystemUserID;
                    rolemappingobj.IsDefault = true;
                    rolemappingobj.CreatedBy = MySession.SystemSession.SystemUserID;
                    rolemappingobj.CreatedDate = DateTime.Now;

                    rolemapping.Insert(rolemappingobj);

                    rolemapping.Save();

                    try
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("@username@", username.Trim());
                        dic.Add("@password@", _randompassword.Trim());
                        dic.Add("@urllink@", HelpingClass.CurrentWebUrl() + "Admin/Account");

                        string htmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\NewConsultant.html", dic);

                        NotificationManager.Fire("Account", "your account has been created", "/Admin/Account/MyProfile", systemuserobj.SystemUserID, MySession.SystemSession.SystemUserID);
                        EmailManager.SendEMail("EmailConfig1", email.Trim(), null, null, "Welcome Onboard to IndoUkLabs", htmltemplate, null);

                    }
                    catch (Exception ex)
                    {
                        return WebJSResponse.ResponseSWAL(SwalEnum.warning, "Created Successfully !", "Consultant has been created successfully but something went wrong while sending email<br>"+ex.Message, new { systemuserjson = systemuserobj, consultantjson = consultantobj });
                    }
                    return WebJSResponse.ResponseSWAL(SwalEnum.success, "Created Successfully !", "Consultant has been created successfully<br>", new { systemuserjson = systemuserobj,consultantjson = consultantobj });

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
        public JsonResult Update(string firstname, string middlename, string lastname, string email, string mobileno, string address, bool status, string specialisationfield, int editid)
        {
            try
            {
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.IConsultantRepository consultant = new BLL.Admin.ConsultantRepository();

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

                        var consultantobj = consultant.GetByID(systemuserobj.DetailID.Value);
                        consultantobj.SpecialisationAreas = specialisationfield.Trim();
                        consultantobj.UpdatedBy = MySession.SystemSession.SystemUserID;
                        consultantobj.UpdatedDate = DateTime.Now;

                        consultant.Update(consultantobj);
                        consultant.Save();

                        return WebJSResponse.ResponseSWAL(SwalEnum.success, "Updated Successfully !", "Consultant has been updated successfully<br>", new { systemuserjson = systemuserobj,consultantjson=consultantobj });
                    }

                    else
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "No consultant found !", "There is no consultant associated to this consultant id.", new { });
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
        public JsonResult Get(int consultantid)
        {
            try
            {
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.IConsultantRepository consultant = new BLL.Admin.ConsultantRepository();

                if (consultantid > 0)
                {

                    var systemuserobj = systemuser.GetByID(consultantid);
                    if (systemuserobj != null)
                    {
                        var consultantobj = consultant.GetByID(systemuserobj.DetailID.Value);
                        return WebJSResponse.ResponseSimple(new { systemuserjson = systemuserobj, consultantjson = consultantobj });
                    }

                    else
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "No consultant found !", "There is no consultant associated to this consultant id.", new { });
                    }


                }

                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Incorrect consultant-id !", "Kindly provide correct consultant id.", new { });


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
                Repositories.Admin.IConsultantRepository consultant = new BLL.Admin.ConsultantRepository();

                object returnlist = null;
                returnlist = (from s in systemuser.GetAll()
                              join c in consultant.GetAll() on s.DetailID equals c.ConsultantID
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
                                      DetailType="Consultant",
                                      s.CreatedBy,
                                      s.CreatedDate,
                                      s.UpdatedBy,
                                      s.UpdatedDate,
                                      s.IsBlock,
                                      status = s.IsActive.HasValue? (s.IsActive.Value == true ? "Active" : "Inactive"):"Inactive",
                                      c.ConsultantID,
                                      c.SpecialisationAreas,
                                      ConsultantCreatedBy = c.CreatedBy,
                                      ConsultantCreatedDate = c.CreatedDate,
                                      ConsultantUpdatedDate = c.UpdatedDate,
                                      ConsultantUpdatedBy = c.UpdatedBy
                                  }).Where(x => (x.IsActive.HasValue? (x.IsActive.Value == status):(false==status))).OrderByDescending(x => x.SystemUserID).ToList();



                return WebJSResponse.ResponseSimple(new { consultantjson = returnlist });




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
                Repositories.Admin.IConsultantRepository consultant = new BLL.Admin.ConsultantRepository();

                object returnlist = null;
                var data = systemuser.GetByID(systemuserid);
                if (data != null)
                {
                    var consultantdata = consultant.GetByID(data.DetailID.Value);

                    if (consultantdata != null)
                    {
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
                            DetailType = "Consultant",
                            data.CreatedBy,
                            data.CreatedDate,
                            data.UpdatedBy,
                            data.UpdatedDate,
                            data.IsBlock,
                            status = data.IsActive.HasValue ? (data.IsActive.Value == true ? "Active" : "Inactive") : "Inactive",
                            consultantdata.ConsultantID,
                            consultantdata.SpecialisationAreas,
                            ConsultantCreatedBy = consultantdata.CreatedBy,
                            ConsultantCreatedDate = consultantdata.CreatedDate,
                            ConsultantUpdatedDate = consultantdata.UpdatedDate,
                            ConsultantUpdatedBy = consultantdata.UpdatedBy
                        };
                    }
                }

                return WebJSResponse.ResponseSimple(new { consultantjson = returnlist });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [HttpPost]
        [Route("UnblockConsultant")]
        public JsonResult UnblockConsultant(int systemuserid)
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

        [Route("ActiveConsultants")]
        [SystemAuthorizeMember]
        public ActionResult ActiveConsultants()
        {
            Repositories.Admin.ISystemUserRepository user = new BLL.Admin.SystemUserRepository();

            var activeblockedconsultants =  user.GetAll().Where(x => (x.IsActive.HasValue ? x.IsActive.Value : false) && 
                (x.IsBlock.HasValue ? x.IsBlock.Value : false) && x.DetailType.Value==3).ToList();

            foreach (var consultant in activeblockedconsultants)
            {
                user.Unblock(consultant.SystemUserID, true);
            }

            return View("~/Views/Admin/Consultant/ActiveConsultants.cshtml");
        }

        [Route("InactiveConsultants")]
        [SystemAuthorizeMember]
        public ActionResult InactiveConsultants()
        {
            Repositories.Admin.ISystemUserRepository user = new BLL.Admin.SystemUserRepository();

            var inactiveblockedconsultants = user.GetAll().Where(x => x.IsActive.HasValue ? !x.IsActive.Value : true &&
                (x.IsBlock.HasValue ? x.IsBlock.Value : false) && x.DetailType.Value == 3).ToList();

            foreach (var consultant in inactiveblockedconsultants)
            {
                user.Unblock(consultant.SystemUserID, true);
            }

            return View("~/Views/Admin/Consultant/InactiveConsultants.cshtml");
        } 
    }
}