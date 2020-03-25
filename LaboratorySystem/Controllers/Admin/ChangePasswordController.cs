using LaboratoryBusiness.POCO.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaboratorySystem.Controllers.Admin
{
    public class ChangePasswordController : RedirectController
    {
        public ActionResult Index(string guid)
        {
            try
            {
                LaboratoryBusiness.Repositories.Admin.IForgetPasswordRepository forgetPassword = new LaboratoryBusiness.BLL.Admin.ForgetPasswordRepository();
                ForgetPassword pocofp = new ForgetPassword();
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
                        ViewBag.SystemUserID = pocofp.SystemUserID;
                    }
                    return View();
                }
                else
                {

                    return Redirect("Admin/Error/NotFound");
                }
            }
            catch (Exception e)
            {
                ViewBag.IsLinkExpired = false;
               
                return View();
            }
        }
      

        [HttpPost]
        public ActionResult ChangePasswordSave(string password, string confirmpassword,string systemUserID)
        {
            try
            {
                if (systemUserID.Contains('/'))
                {systemUserID=systemUserID.Replace("/","");
                }
                LaboratoryBusiness.Repositories.Admin.ISystemUserRepository sysUser = new LaboratoryBusiness.BLL.Admin.SystemUserRepository();
                LaboratoryBusiness.POCO.Admin.SystemUser pocofp = new LaboratoryBusiness.POCO.Admin.SystemUser();
                pocofp = sysUser.GetByID(Convert.ToInt16(systemUserID));
                pocofp.Password=HelpingClass.Base64Encode(password);
                sysUser.Update(pocofp);
                sysUser.Save();
                try
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("@username@", pocofp.Username.Trim());
                    dic.Add("@password@", password);
                   

                    string htmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\ChangePassword.html", dic);

                    EmailManager.SendEMail("EmailConfig1", pocofp.Email, null, null, "Password Change Confirmation", htmltemplate, null);
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
    }
}