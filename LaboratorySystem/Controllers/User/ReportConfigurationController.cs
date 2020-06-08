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
    [RoutePrefix("User/ReportConfiguration")]
    public class ReportConfigurationController : RedirectController
    {
        [Route]
        [ClientAuthorizeMember]
        // GET: ReportConfiguration
        public ActionResult Index(string IsEdit, string EditId)
        {
            if (EditId != null && IsEdit == null)
            {
                ViewBag.IsView = true;
            }
            else if (EditId != null && IsEdit != null)
            {
                if (IsEdit.Equals("0"))
                {
                    ViewBag.IsView = true;
                }
                else
                {
                    ViewBag.IsView = false;
                }
            }
            else
            {
                ViewBag.IsView = false;
            }

            ViewBag.currentdomaindb = this.currentdomaindb;
            ViewBag.subdomainurl = this.subdomainurl;
            return View("~/Views/User/ReportConfiguration/Index.cshtml");
            //return View();
        }
        [HttpPost]
        [Route("AddConfigurationInfo")]
        public JsonResult Create(string labname, string phoneno, string labaddress, string headofficeaddress, string labemail, string labuniquecode,string registrationno, string vatrate ,
          int editid)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.ILabReportConfiguration labReportConfiguration = this.currentdomaindb.LabReportConfigurationRepository();
                Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();


                bool islabReportConfigurationcreated = false;

                if (editid == 0)
                {

                    BusinessPOCO.User.Cl_LabReportConfiguration configdetailobj = new BusinessPOCO.User.Cl_LabReportConfiguration();
                    configdetailobj.LabID = 1;
                    
                    configdetailobj.LabName = labname.Trim();
                    configdetailobj.labEmail = labemail.Trim();
                    configdetailobj.LabCompanyNumber = phoneno.Trim();
                    configdetailobj.LabAddress = labaddress.Trim();
                    configdetailobj.LabHeadOfficeAddress = headofficeaddress.Trim();
                    configdetailobj.LabUniqueCode = labuniquecode.Trim();
                    configdetailobj.CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                    configdetailobj.CreatedDate = DateTime.Now;
                    configdetailobj.RegistrationNumber = registrationno.Trim();
                    configdetailobj.VatRate = vatrate.Trim();
                    labReportConfiguration.Insert(configdetailobj);
                    labReportConfiguration.Save();
                    islabReportConfigurationcreated = true;
                   





                    if (islabReportConfigurationcreated)
                    {
                        return WebJSResponse.ResponseSWAL(SwalEnum.success, "Created Successfully !", "Report Configuration has been created successfully<br>", new { labReportConfigurationjson = configdetailobj });
                    }
                    else
                    {

                        return WebJSResponse.ResponseSWAL(SwalEnum.warning, "Created Successfully !", "Report Configuration has been created successfully<br>but something went wrong.", new { labReportConfigurationjson = configdetailobj });
                    }


                }
                else
                {

                    BusinessPOCO.User.Cl_LabReportConfiguration configdetailobj = new BusinessPOCO.User.Cl_LabReportConfiguration();
                    configdetailobj.LabID = 1;
                    configdetailobj.ConfigID = 1;
                    configdetailobj.LabName = labname.Trim();
                    configdetailobj.labEmail = labemail.Trim();
                    configdetailobj.LabCompanyNumber = phoneno.Trim();
                    configdetailobj.LabAddress = labaddress.Trim();
                    configdetailobj.LabHeadOfficeAddress = headofficeaddress.Trim();
                    configdetailobj.LabUniqueCode = labuniquecode.Trim();
                    configdetailobj.UpdatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                    configdetailobj.UpdatedDate = DateTime.Now;
                    configdetailobj.RegistrationNumber = registrationno.Trim();
                    configdetailobj.VatRate = vatrate.Trim();

                    labReportConfiguration.Update(configdetailobj);
                    labReportConfiguration.Save();
                    islabReportConfigurationcreated = true;
                    if (islabReportConfigurationcreated)
                    {
                        return WebJSResponse.ResponseSWAL(SwalEnum.success, "Updated Successfully !", "Report Configuration has been updated successfully<br>", new { labReportConfigurationjson = configdetailobj });
                    }
                    else
                    {

                        return WebJSResponse.ResponseSWAL(SwalEnum.warning, "Updated Successfully !", "Report Configuration  has been updated successfully<br>but something went wrong.", new { labReportConfigurationjson = configdetailobj });
                    }


                }

               // return WebJSResponse.ResponseToastr(ToastrEnum.error, "In edit phase", "Currently page is in edit phase!please try again by clicking on menu.", new { });

            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }
        [HttpPost]
        [Route("UploadFile")]
        public JsonResult UploadFile(int configid, int AttachmentTypeID, int SupplementReportID = 0)
        {
            try
            {
                LaboratoryBusiness.Repositories.User.ILabReportConfiguration testattachment = this.currentdomaindb.LabReportConfigurationRepository();
                LaboratoryBusiness.POCO.User.Cl_LabReportConfiguration testattachmentobj = new BusinessPOCO.User.Cl_LabReportConfiguration();

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

                    fl.UploadFile(fm, "\\ReportConfiguration", out path);

                    path = HelpingClass.LocalUploadPathToRelativeWebPath(path);

                    testattachmentobj = new BusinessPOCO.User.Cl_LabReportConfiguration();
                    testattachmentobj.LabImage = path;
                    //testattachmentobj.Extension = fm.Extension;
                    //testattachmentobj.Name = Path.GetFileNameWithoutExtension(file.FileName);
                    testattachmentobj.UpdatedBy = SupplementReportID == 0 ? MySession.GetClientSession(this.subdomainurl).ClientUserID : SupplementReportID;
                    testattachmentobj.UpdatedDate = DateTime.Now;
                    testattachmentobj.ConfigID = 1;
                    

                    testattachment.UpdateImage(testattachmentobj);
                    testattachment.Save();

                }
                // Returns message that successfully uploaded  
                return WebJSResponse.ResponseSimple(new { ResponseType = "swal-success", Title = "Saved Successfully !", Description = "Lab Report Logo has been saved successfully<br>." });
               
            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseSimple(new { ResponseType = "swal-warning", Title = "Saved Successfully !", Description = "Sample Collection has been saved successfully<br>but something went wrong while uploading files." });
               
            }
        }
        [HttpPost]
        [Route("GetList")]
        public JsonResult GetList()
        {
            try
            {
                Repositories.User.ILabReportConfiguration labReportConfiguration = this.currentdomaindb.LabReportConfigurationRepository();

                object returnlist = null;
                returnlist = (from lab in labReportConfiguration.GetAll()
                              select new
                              {
                                  lab.ConfigID,
                                  lab.LabID,
                                  lab.labEmail,
                                  lab.LabAddress,
                                  lab.LabName,
                                  lab.LabHeadOfficeAddress,
                                  lab.UpdatedBy,
                                  lab.UpdatedDate,
                                  lab.CreatedDate,
                                  lab.CreatedBy,
                                  lab.LabImage,
                                  lab.LabUniqueCode,
                                  lab.LabCompanyNumber,
                                  lab.RegistrationNumber,
                                  lab.VatRate

                              }).OrderByDescending(x => x.ConfigID).ToList();



                return WebJSResponse.ResponseSimple(new { labReportConfigurationjson = returnlist });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }
    }
}