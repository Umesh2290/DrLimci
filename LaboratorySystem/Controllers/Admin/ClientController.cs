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
    [RoutePrefix("Admin/Client")]
    public class ClientController : RedirectController
    {

        // GET: Plan
        [Route]
        // GET: Home
        [SystemAuthorizeMember]
        public ActionResult Index()
        {
            if (!HelpingClass.ValidateConnection())
            {
                throw new Exception("LabSystemDB_Connection does not establish!");
            }
            return View("~/Views/Admin/Client/Index.cshtml");
        }

        [HttpPost]
        [Route("Create")]
        public JsonResult Create(string firstname, string middlename, string lastname, string username,string companyname,
            string subdomain,int planid,int planduration,int licensecost,int standdbproviderid,
            int invoiceno, string email, string mobileno, string address, bool status,List<ViewModels.ClientParameter> clparamters, int editid)
        {
            try
            {
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.ISystemUserTypeRepository systemsusertype = new BLL.Admin.SystemUserTypeRepository();
                Repositories.Admin.IRoleRepository role = new BLL.Admin.RoleRepository();
                Repositories.Admin.IRoleMappingRepository rolemapping = new BLL.Admin.RoleMappingRepository();
                Repositories.Admin.IClientRepository client = new BLL.Admin.ClientRepository();
                Repositories.Admin.IClientParameterRepository clientparamter = new BLL.Admin.ClientParameterRepository();
                Repositories.Admin.IPlanRepository plan = new BLL.Admin.PlanRepository();

                string domainpart = HelpingClass.GetDomainOnly();
                if (editid == 0)
                {
                    if (systemuser.GetAll().Where(x => (x.Username.Trim()).Equals(username.Trim() + "@" + companyname.Replace(" ", "").Trim()) || (string.IsNullOrEmpty(x.Email) ? false : x.Email.Equals(email.Trim()))).Count() > 0)
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "Username or email already exist !", "Please try another !", new { });

                    }

                    if (client.GetAll().Where(x => (x.InvoiceID.HasValue == false ? false : x.InvoiceID.Value==invoiceno)).Count() > 0)
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "Invoice No# already exists !", "Please try another !", new { });

                    }

                    if (client.GetAll().Where(x => (string.IsNullOrEmpty(x.CompanyName) ? false : x.CompanyName.Trim().Replace(" ","").Equals(companyname.Trim().Replace(" ","")))).Count() > 0)
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "Company name already exist !", "Please try another !", new { });

                    }

                    if (client.GetAll().Where(x => (string.IsNullOrEmpty(x.Subdomain) ? false : x.Subdomain.Trim().Replace(" ", "").Equals(subdomain.Trim().Replace(" ", "") + "." + domainpart))).Count() > 0)
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "subdomain already exist !", "Please try another !", new { });

                    }

                    var clientdetail = new  BusinessPOCO.Admin.Client();
                    clientdetail.CompanyName = companyname;
                    clientdetail.InvoiceID = invoiceno;
                    clientdetail.PlanDuration = planduration;
                    clientdetail.PlanID = planid;
                    clientdetail.PriceUnit = "Rs";
                    clientdetail.STandDBprovdiderID = standdbproviderid;
                    clientdetail.Subdomain = subdomain + "." + domainpart;
                    clientdetail.TotalLicenseCost = licensecost;
                    clientdetail.CreatedBy = MySession.SystemSession.SystemUserID;
                    clientdetail.CreatedDate = DateTime.Now;

                    client.Insert(clientdetail);

                    client.Save();

                    List<BusinessPOCO.Admin.ClientParameter> clplist = new List<BusinessPOCO.Admin.ClientParameter>();
                    if (clparamters != null)
                    {
                        
                        foreach (var clp in clparamters)
                        {
                            var cparamter = new BusinessPOCO.Admin.ClientParameter();
                            cparamter.ParameterID = clp.ParameterID;
                            cparamter.ParameterValue = clp.ParameterValue;
                            cparamter.ClientDetailID = clientdetail.ClientDetailID;
                            cparamter.CreatedBy = MySession.SystemSession.SystemUserID;
                            cparamter.CreatedDate = DateTime.Now;

                            clientparamter.Insert(cparamter);
                            clplist.Add(cparamter);
                        }

                        clientparamter.Save();
                    }

                    string _randompassword = System.Web.Security.Membership.GeneratePassword(8, 0);
                    byte[] pass = System.Text.Encoding.UTF8.GetBytes(_randompassword);
                    string _encryptedpassword = System.Convert.ToBase64String(pass);

                    int clientdetailtypeid = systemsusertype.GetAll().Where(x => x.TypeName.Equals("Client")).FirstOrDefault().SystemUserTypeID;
                    //int superadminroleid = role.GetAll().Where(x => x.RoleName.Equals("Superadmin")).FirstOrDefault().RoleID;

                    var systemuserobj = new BusinessPOCO.Admin.SystemUser();
                    systemuserobj.FirstName = firstname.Trim();
                    systemuserobj.MiddleName = middlename.Trim();
                    systemuserobj.LastName = lastname.Trim();
                    systemuserobj.Username = username.Trim() + "@" + companyname.Replace(" ","").Trim();
                    systemuserobj.Password = _encryptedpassword;
                    systemuserobj.Email = email.Trim();
                    systemuserobj.MobileNo = mobileno.Trim();
                    systemuserobj.Address = address.Trim();
                    systemuserobj.IsActive = status;
                    systemuserobj.IsBlock = false;
                    systemuserobj.CreatedBy = MySession.SystemSession.SystemUserID;
                    systemuserobj.CreatedDate = DateTime.Now;
                    systemuserobj.DetailType = clientdetailtypeid;
                    systemuserobj.DetailID = clientdetail.ClientDetailID;

                    systemuser.Insert(systemuserobj);

                    systemuser.Save();


                    //    var rolemappingobj = new BusinessPOCO.Admin.RoleMapping();
                    //    rolemappingobj.RoleID = superadminroleid;
                    //    rolemappingobj.UserID = systemuserobj.SystemUserID;
                    //    rolemappingobj.IsDefault = true;
                    //    rolemappingobj.CreatedBy = MySession.SystemSession.SystemUserID;
                    //    rolemappingobj.CreatedDate = DateTime.Now;

                    //    rolemapping.Insert(rolemappingobj);

                    //rolemapping.Save();
                    //Not eligible because client cannot be login from main system

                    bool isdbcreated = false;
                    bool isstorageconfigured = false;
                    bool iswelcomeemailsent = false;


                    try
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("@username@", username.Trim() +"@" + companyname.Replace(" ", "").Trim());
                        dic.Add("@password@", _randompassword.Trim());
                        dic.Add("@urllink@", HelpingClass.GetProtocol() + clientdetail.Subdomain + "/User/Account");

                        string htmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\NewEmployee.html", dic);

                        EmailManager.SendEMail("EmailConfig1", email.Trim(), null, null, "Welcome Onboard to IndoUkLabs", htmltemplate, null);

                        iswelcomeemailsent = true;

                    }
                    catch (Exception ex)
                    {
                        iswelcomeemailsent = false;
                    }

                    Client cl = new Client(clientdetail.ClientDetailID);
                    int clientuserid = 0;
                    DBInitializer db = null;
                    try
                    {
                        
                        db = new DBInitializer(cl);
                        Repositories.User.IClientUserRepository clientuser = db.ClientUserRepository();
                        Repositories.User.IClientUserTypeRepository clientusertype = db.ClientUserTypeRepository();
                        Repositories.User.IRoleRepository cl_role = db.RoleRepository();
                        Repositories.User.IRoleMappingRepository cl_rolemapping = db.RoleMappingRepository();
                        Repositories.User.IAdminDetailRepository admindetail = db.AdminDetailRepository();

                        var clientadmindetail = new BusinessPOCO.User.Cl_AdminDetail();
                        clientadmindetail.ClientUserID = 0;
                        clientadmindetail.SystemClientID = systemuserobj.SystemUserID;
                        clientadmindetail.CreatedBy = (MySession.SystemSession.SystemUserID*(-1));
                        clientadmindetail.CreatedDate = DateTime.Now;
                        

                        admindetail.Insert(clientadmindetail);

                        admindetail.Save();

                        int clientadmindetailtypeid = clientusertype.GetAll().Where(x => x.TypeName.Equals("Client Admin")).FirstOrDefault().ClientUserTypeID;
                        int clientadminroleid = cl_role.GetAll().Where(x => x.RoleName.Equals("Admin")).FirstOrDefault().RoleID;

                        var clientuserobj = new BusinessPOCO.User.Cl_ClientUser();
                        clientuserobj.FirstName = systemuserobj.FirstName;
                        clientuserobj.LastName = systemuserobj.LastName;
                        clientuserobj.Username = systemuserobj.Username;
                        clientuserobj.Password = systemuserobj.Password;
                        clientuserobj.Email = systemuserobj.Email;
                        clientuserobj.MobileNo = systemuserobj.MobileNo;
                        clientuserobj.Address = systemuserobj.Address;
                        clientuserobj.IsActive = systemuserobj.IsActive;
                        clientuserobj.IsBlock = systemuserobj.IsBlock;
                        clientuserobj.CreatedBy = (MySession.SystemSession.SystemUserID * (-1));
                        clientuserobj.CreatedDate = systemuserobj.CreatedDate;
                        clientuserobj.DetailType = clientadmindetailtypeid;
                        clientuserobj.DetailID = clientadmindetail.AdminDetailID;

                        clientuser.Insert(clientuserobj);

                        clientuser.Save();

                        clientuserid = clientuserobj.ClientUserID;

                        var cl_rolemappingobj = new BusinessPOCO.User.Cl_RoleMapping();
                        cl_rolemappingobj.RoleID = clientadminroleid;
                        cl_rolemappingobj.UserID = clientuserobj.ClientUserID;
                        cl_rolemappingobj.IsDefault = true;
                        cl_rolemappingobj.CreatedBy = (MySession.SystemSession.SystemUserID * (-1));
                        cl_rolemappingobj.CreatedDate = DateTime.Now;

                        cl_rolemapping.Insert(cl_rolemappingobj);

                        cl_rolemapping.Save();

                        isdbcreated = true;
                        
                    }
                    catch (Exception ex)
                    {
                        isdbcreated = false;
                    }


                    try
                    {
                        var planobj = plan.GetByID(planid);

                        Dictionary<string, string> dic = new Dictionary<string, string>();
                     //   dic.Add("@imgpath@", HelpingClass.GetDefaultDirectory() + "\\Content\\EmailTemplates\\InvoicePDF\\target001.png");
                        dic.Add("@imgpath@", clientdetail.LogoImgLink);
                        dic.Add("@companyName@", clientdetail.CompanyName);
                        //dic.Add("@address@", clientdetail.a);

                        dic.Add("@Email@", systemuserobj.Email);
                        dic.Add("@MobileNo@", systemuserobj.MobileNo);
                        dic.Add("@fullname@", HelpingClass.SpacetoSpaceCode(string.Join(" ", systemuserobj.FirstName.ToCharArray()) + "   " + string.Join(" ", systemuserobj.MiddleName.ToCharArray()) + "   " + string.Join(" ", systemuserobj.LastName.ToCharArray())));
                        dic.Add("@address@", HelpingClass.SpacetoSpaceCode(string.Join(" ",systemuserobj.Address.ToCharArray())));
                        dic.Add("@invoiceno@", HelpingClass.SpacetoSpaceCode(string.Join(" ", clientdetail.InvoiceID.Value.ToString("000").ToCharArray())));
                        dic.Add("@planname@", HelpingClass.SpacetoSpaceCode(string.Join(" ", planobj.PlanName.ToCharArray())));
                        dic.Add("@plandescription@", HelpingClass.SpacetoSpaceCode(string.Join(" ", planobj.PlanDescription.ToCharArray())));
                        dic.Add("@price@", HelpingClass.SpacetoSpaceCode(string.Join(" ", clientdetail.PriceUnit.ToCharArray()) + "  " + string.Join(" ", clientdetail.TotalLicenseCost.Value.ToString().ToCharArray())));
                        dic.Add("@totalamount@", HelpingClass.SpacetoSpaceCode(string.Join(" ", clientdetail.PriceUnit.ToCharArray()) + "  " + string.Join(" ", clientdetail.TotalLicenseCost.Value.ToString().ToCharArray())));
                        dic.Add("@duration@", HelpingClass.SpacetoSpaceCode(string.Join(" ", clientdetail.PlanDuration.ToString().ToCharArray())+"  Months"));
                        dic.Add("@startdate@", HelpingClass.SpacetoSpaceCode(string.Join(" ", clientdetail.CreatedDate.Value.ToString("dd/MM/yyyy").ToCharArray())));
                        dic.Add("@enddate@", HelpingClass.SpacetoSpaceCode(string.Join(" ", clientdetail.CreatedDate.Value.AddMonths(clientdetail.PlanDuration.Value).ToString("dd/MM/yyyy").ToCharArray())));

                        string pdfhtmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\InvoicePDF\invoicehtml.html", dic);

                        FileManager pd = PdfManager.HtmlToPdf("Invoice", pdfhtmltemplate);
                        FileInitializer fl = new FileInitializer(cl);
                        string pdfuploadedpath ;
                        string pdffilelocalsavedpath;
                        FileManager.ByteArrayToFile(pd, HelpingClass.GetDefaultDirectory() + "\\ClientsData\\" + cl.CompanyName.Trim().Replace(" ", ""), out pdffilelocalsavedpath);
                        fl.UploadFile(pd, "\\PersonalFiles", out pdfuploadedpath);


                        var clientoupdate = client.GetByID(cl.ClientDetailID);
                        clientoupdate.InvoicePDFLink = (("\\ClientsData\\" + cl.CompanyName.Trim().Replace(" ", "") + "\\" + pd.Name+pd.Extension).Replace("\\","/"));

                        client.Update(clientoupdate);
                        client.Save();

                        dic = new Dictionary<string, string>();
                        dic.Add("@companyname@", clientdetail.CompanyName.Trim());

                        string htmltemplate = HtmlRendering.ReplaceParameterOfHtmlTemplate(HelpingClass.GetDefaultDirectory() + @"\Content\EmailTemplates\NewInvoice.html", dic);

                        EmailManager.SendEMail("EmailConfig1", email.Trim(), null, null, "Welcome to IndoUkLabs | Invoice for " + clientoupdate.CompanyName, htmltemplate, new List<FileManager>() { pd });

                        isstorageconfigured = true;
                    }
                    catch (Exception ex)
                    {
                        isstorageconfigured = false;
                    }

                    if (isdbcreated && isstorageconfigured && iswelcomeemailsent)
                    {
                        NotificationManager.FireOnClient("Account", "your account has been created", "/User/Account/MyProfile", clientuserid, ((MySession.SystemSession.SystemUserID)*-1),db);
                        return WebJSResponse.ResponseSimple(new { systemuserjson = systemuserobj, clientdetailsjson = clientdetail, clientparamater = clplist, responsetype = "swal-success" });
                    }
                    else
                    {
                        NotificationManager.FireOnClient("Account", "your account has been created", "/User/Account/MyProfile", clientuserid, ((MySession.SystemSession.SystemUserID) * -1), db);
                        return WebJSResponse.ResponseSimple(new { systemuserjson = systemuserobj, clientdetailsjson = clientdetail, clientparamater = clplist, responsetype = "swal-warning" });
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
            string subdomain, int planid, int planduration, int licensecost, int standdbproviderid,
            int invoiceno, string email, string mobileno, string address, bool status, List<ViewModels.ClientParameter> clparamters, int editid)
        {
            try
            {
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.ISystemUserTypeRepository systemsusertype = new BLL.Admin.SystemUserTypeRepository();
                Repositories.Admin.IClientRepository client = new BLL.Admin.ClientRepository();
                Repositories.Admin.IClientParameterRepository clientparamter = new BLL.Admin.ClientParameterRepository();

                string domainpart = HelpingClass.GetDomainOnly();

                if (editid > 0)
                {

                    if (client.GetAll().Where(x => (x.InvoiceID.HasValue == false ? false : x.InvoiceID.Value == invoiceno) && x.ClientDetailID!=editid).Count() > 0)
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "Invoice No# already exists !", "Please try another !", new { });

                    }

                    if (client.GetAll().Where(x => (string.IsNullOrEmpty(x.Subdomain) ? false : x.Subdomain.Trim().Replace(" ", "").Equals(subdomain.Trim().Replace(" ", "") + "." + domainpart)) && x.ClientDetailID != editid).Count() > 0)
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "subdomain already exist !", "Please try another !", new { });

                    }

                    int clientdetailtypeid = systemsusertype.GetAll().Where(x => x.TypeName.Equals("Client")).FirstOrDefault().SystemUserTypeID;

                    var clientdetail = client.GetByID(editid);
                    if (clientdetail != null)
                    {
                        clientdetail.InvoiceID = invoiceno;
                        clientdetail.PlanDuration = planduration;
                        clientdetail.PlanID = planid;
                        clientdetail.PriceUnit = "Rs";
                        clientdetail.STandDBprovdiderID = standdbproviderid;
                        clientdetail.Subdomain = subdomain.EndsWith(("." + domainpart))?subdomain: (subdomain+"." + domainpart);
                        clientdetail.TotalLicenseCost = licensecost;
                        clientdetail.UpdatedBy = MySession.SystemSession.SystemUserID;
                        clientdetail.UpdatedDate = DateTime.Now;

                        client.Update(clientdetail);

                        client.Save();
                    }

                    var systemuserobj = systemuser.GetAll().Where(x => x.DetailID.Value == clientdetail.ClientDetailID && x.DetailType.Value == clientdetailtypeid).FirstOrDefault();
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

                        try
                        {
                            Client cl = new Client(clientdetail.ClientDetailID);
                            DBInitializer db = new DBInitializer(cl);

                            Repositories.User.IClientUserRepository cl_clientuser = db.ClientUserRepository();
                            Repositories.User.IAdminDetailRepository admindetail = db.AdminDetailRepository();
                            Repositories.User.IClientUserTypeRepository clientusertype = db.ClientUserTypeRepository();

                            int clientadmindetailtypeid = clientusertype.GetAll().Where(x => x.TypeName.Equals("Client Admin")).FirstOrDefault().ClientUserTypeID;

                            var clientuserobj = (from cl_cl in cl_clientuser.GetAll()
                                                 join ca in admindetail.GetAll() on cl_cl.DetailID equals ca.AdminDetailID
                                                 join su in systemuser.GetAll() on ca.SystemClientID equals su.SystemUserID
                                                 where cl_cl.DetailType == clientadmindetailtypeid
                                                 select cl_cl).FirstOrDefault();

                            clientuserobj.FirstName = systemuserobj.FirstName;
                            clientuserobj.LastName = systemuserobj.LastName;
                            clientuserobj.Email = systemuserobj.Email;
                            clientuserobj.MobileNo = systemuserobj.MobileNo;
                            clientuserobj.Address = systemuserobj.Address;
                            clientuserobj.IsActive = systemuserobj.IsActive;
                            clientuserobj.UpdatedBy = ((MySession.SystemSession.SystemUserID)*-1);
                            clientuserobj.UpdatedDate = DateTime.Now;

                            cl_clientuser.Update(clientuserobj);
                            cl_clientuser.Save();
                        }
                        catch (Exception ex)
                        {

                        }
                        var _currentclientparameters = clientparamter.GetAll().Where(x => x.ClientDetailID == editid).ToList();

                        foreach (var currentparameter in _currentclientparameters)
                        {
                            clientparamter.Delete(currentparameter.ClientParameterID);
                        }

                        clientparamter.Save();

                        List<BusinessPOCO.Admin.ClientParameter> clplist = new List<BusinessPOCO.Admin.ClientParameter>();
                        if (clparamters != null)
                        {
                            foreach (var clp in clparamters)
                            {
                                var cparamter = new BusinessPOCO.Admin.ClientParameter();
                                cparamter.ParameterID = clp.ParameterID;
                                cparamter.ParameterValue = clp.ParameterValue;
                                cparamter.ClientDetailID = clientdetail.ClientDetailID;
                                cparamter.CreatedBy = ((MySession.SystemSession.SystemUserID) * -1);
                                cparamter.CreatedDate = DateTime.Now;

                                clientparamter.Insert(cparamter);
                                clplist.Add(cparamter);
                            }

                            clientparamter.Save();
                        }

                        


                        return WebJSResponse.ResponseSimple(new { systemuserjson = systemuserobj, clientdetailsjson = clientdetail, clientparamterjson=clplist,responsetype="swal-success" });
                    }

                    else
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "No client found !", "There is no client associated to this client id.", new { });
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
        public JsonResult UploadFile(int ClientDetailID, bool IsUpdated,bool IsWarning)
        {
            try
            {
                LaboratoryBusiness.Repositories.Admin.IClientRepository clrepo = new BLL.Admin.ClientRepository();
                LaboratoryBusiness.POCO.Admin.Client client = clrepo.GetByID(ClientDetailID);


                FileManager fm = new FileManager();
                string path = string.Empty;


                HttpFileCollectionBase files = Request.Files;

                string fname;

                    

                if(files.Count > 0) 
                {
                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = files[0].FileName.Split('|')[0].Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = files[0].FileName.Split('|')[0];
                    }

                    if(files[0].FileName.Split('|')[1].Equals("LogoImg")) 
                    {
                        fm = new FileManager();
                    fm.FileBytes = FileManager.InputStreamToByteArray(files[0].InputStream);
                    fm.MimeType = files[0].ContentType;
                    fm.Extension = Path.GetExtension(fname);
                    fm.Name = "ClientLogo";
                    }
                    else 
                    {
                        fm = new FileManager();
                    fm.FileBytes = FileManager.InputStreamToByteArray(files[0].InputStream);
                    fm.MimeType = files[0].ContentType;
                    fm.Extension = Path.GetExtension(fname);
                    fm.Name = "ClientBackground";
                    }

                    string logoimglocalsavedpath = null;
                    string backgroundimglocalsavedpath = null;

                    if (fm.Name.Equals("ClientLogo"))
                    {
                        FileManager.ByteArrayToFile(fm, HelpingClass.GetDefaultDirectory() + "\\ClientsData\\" + client.CompanyName.Trim().Replace(" ", "") + "\\ClientConfigImages", out logoimglocalsavedpath);
                    }
                    else
                    {
                        FileManager.ByteArrayToFile(fm, HelpingClass.GetDefaultDirectory() + "\\ClientsData\\" + client.CompanyName.Trim().Replace(" ", "") + "\\ClientConfigImages", out backgroundimglocalsavedpath);
                    }



                    if(files.Count > 1) 
                    {
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = files[1].FileName.Split('|')[0].Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = files[1].FileName.Split('|')[0];
                    }
                        if(files[1].FileName.Split('|')[1].Equals("BackgroundImg")) 
                        {
                            fm = new FileManager();
                            fm.FileBytes = FileManager.InputStreamToByteArray(files[1].InputStream);
                            fm.MimeType = files[1].ContentType;
                            fm.Extension = Path.GetExtension(fname);
                            fm.Name = "ClientBackground";

                            FileManager.ByteArrayToFile(fm, HelpingClass.GetDefaultDirectory() + "\\ClientsData\\" + client.CompanyName.Trim().Replace(" ", "") + "\\ClientConfigImages", out backgroundimglocalsavedpath);
                        }
                    }

                    client.BackgroundImgLink = (backgroundimglocalsavedpath != null ? HelpingClass.LocalUploadPathToRelativeWebPath(backgroundimglocalsavedpath) : null) == null && client.BackgroundImgLink != null ? client.BackgroundImgLink : (backgroundimglocalsavedpath != null ? HelpingClass.LocalUploadPathToRelativeWebPath(backgroundimglocalsavedpath) : null);
                    client.LogoImgLink = (logoimglocalsavedpath != null ? HelpingClass.LocalUploadPathToRelativeWebPath(logoimglocalsavedpath) : null) == null && client.LogoImgLink != null ? client.LogoImgLink : (logoimglocalsavedpath != null ? HelpingClass.LocalUploadPathToRelativeWebPath(logoimglocalsavedpath) : null);

                    clrepo.Update(client);
                    clrepo.Save();
                }

                // Returns message that successfully uploaded  
                if (!IsUpdated)
                {
                    if (!IsWarning)
                    {
                        return WebJSResponse.ResponseSWAL(SwalEnum.success, "Created Successfully !", "Client has been created successfully<br>", new { });
                    }
                    else
                    {
                        return WebJSResponse.ResponseSWAL(SwalEnum.warning, "Created Successfully !", "Client has been created successfully<br>but something went wrong while sendine email , configuring storage or during database setup", new { });
                    }
                }
                else
                {
                    if (!IsWarning)
                    {
                        return WebJSResponse.ResponseSWAL(SwalEnum.success, "Updated Successfully !", "Client has been updated successfully<br>", new { });
                    }
                    else
                    {
                        return WebJSResponse.ResponseSWAL(SwalEnum.warning, "Updated Successfully !", "Client has been updated successfully<br>but something went wrong while sendine email , configuring storage or during database setup", new { });
                    }
                }
            }
            catch (Exception ex)
            {
                if (!IsUpdated)
                {
                    return WebJSResponse.ResponseSWAL(SwalEnum.warning, "Created Successfully !", "Client has been created successfully<br>but something went wrong while uploading files.", new { });
                }
                else
                {
                    return WebJSResponse.ResponseSWAL(SwalEnum.warning, "Updated Successfully !", "Client has been updated successfully<br>but something went wrong while uploading files.", new { });
                }
            }
        }

        [HttpPost]
        [Route("DeleteFile")]
        public JsonResult DeleteFile(int ClientDetailID,string Type)
        {
            try
            {
                LaboratoryBusiness.Repositories.Admin.IClientRepository clrepo = new BLL.Admin.ClientRepository();
                LaboratoryBusiness.POCO.Admin.Client client = clrepo.GetByID(ClientDetailID);


                if (ClientDetailID > 0 && client!=null)
                {
                    if (Type.Equals("Logo"))
                    {
                        client.LogoImgLink = null;
                    }
                    else if (Type.Equals("Background"))
                    {
                        client.BackgroundImgLink = null;
                    }

                    clrepo.Update(client);
                    clrepo.Save();

                    return WebJSResponse.ResponseToastr(ToastrEnum.success, "Deleted Successfully !", Type + " file has been deleted successfully.", new { });

                }

                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Incorrect client-id !", "Kindly provide correct client id.", new { });


            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [HttpPost]
        [Route("Get")]
        public JsonResult Get(int clientid)
        {
            try
            {
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.ISystemUserTypeRepository systemsusertype = new BLL.Admin.SystemUserTypeRepository();
                Repositories.Admin.IClientRepository client = new BLL.Admin.ClientRepository();
                Repositories.Admin.IClientParameterRepository clientparamter = new BLL.Admin.ClientParameterRepository();
                Repositories.Admin.IProviderParamterRepository providerparamter = new BLL.Admin.ProviderParamterRepository();


                if (clientid > 0)
                {
                    int clientdetailtypeid = systemsusertype.GetAll().Where(x => x.TypeName.Equals("Client")).FirstOrDefault().SystemUserTypeID;

                    var clientdetailobj = client.GetByID(clientid);
                    var systemuserobj = systemuser.GetAll().Where(x => x.DetailType.Value == clientdetailtypeid && x.DetailID.Value == clientdetailobj.ClientDetailID).FirstOrDefault();
                    if (clientdetailobj!=null && systemuserobj != null)
                    {
                        var clientparameterobj = (from cp in clientparamter.GetAll()
                                             join pp in providerparamter.GetAll() on cp.ParameterID equals pp.ParameterID
                                             where cp.ClientDetailID.Value == clientdetailobj.ClientDetailID
                                             select new { pp.ParameterID, pp.ParameterName, cp.ParameterValue, pp.IsRequired }).ToList();
                        return WebJSResponse.ResponseSimple(new { systemuserjson = systemuserobj, clientdetailjson = clientdetailobj, clientparameterjson = clientparameterobj });
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
                Repositories.Admin.ISystemUserTypeRepository systemsusertype = new BLL.Admin.SystemUserTypeRepository();
                Repositories.Admin.IClientRepository client = new BLL.Admin.ClientRepository();
                Repositories.Admin.IPlanRepository plan = new BLL.Admin.PlanRepository();


                int clientdetailtypeid = systemsusertype.GetAll().Where(x => x.TypeName.Equals("Client")).FirstOrDefault().SystemUserTypeID;

                var returnList = (from cl in client.GetAll()
                                  join su in systemuser.GetAll() on cl.ClientDetailID equals su.DetailID
                                  join pl in plan.GetAll() on cl.PlanID equals pl.PlanID
                                  where su.IsActive.Value == status && su.DetailType.Value == clientdetailtypeid
                                  select new
                                  {
                                      cl.ClientDetailID,
                                      cl.CompanyName,
                                      cl.Subdomain,
                                      pl.PlanName,
                                      status = su.IsActive.HasValue ? (su.IsActive.Value == true ? "Active" : "Inactive") : "Inactive",
                                      su.SystemUserID,
                                      FullName = su.FirstName + " " + su.MiddleName + " " + su.LastName,
                                      su.IsBlock
                                  }).ToList();


                return WebJSResponse.ResponseSimple(new { clientjson = returnList });




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
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.ISystemUserTypeRepository systemsusertype = new BLL.Admin.SystemUserTypeRepository();
                Repositories.Admin.IClientParameterRepository clientparameter = new BLL.Admin.ClientParameterRepository();
                Repositories.Admin.IProviderParamterRepository providerparameter = new BLL.Admin.ProviderParamterRepository();
                Repositories.Admin.IClientRepository client = new BLL.Admin.ClientRepository();
                Repositories.Admin.IPlanRepository plan = new BLL.Admin.PlanRepository();
                Repositories.Admin.IStorageandDBProviderRepository standdbprovider = new BLL.Admin.StorageandDBProviderRepository();

                int clientdetailtypeid = systemsusertype.GetAll().Where(x => x.TypeName.Equals("Client")).FirstOrDefault().SystemUserTypeID;

                var clientobj = client.GetByID(clientdetailid);
                var systemuserobj = systemuser.GetAll().Where(x => x.DetailType.Value == clientdetailtypeid && x.DetailID == clientdetailid).FirstOrDefault();
                var clientparameters = (from cp in clientparameter.GetAll()
                                        join pp in providerparameter.GetAll() on cp.ParameterID equals pp.ParameterID
                                        where cp.ClientDetailID == clientdetailid
                                        select new { pp.ParameterID, pp.ParameterName, cp.ParameterValue }).ToList();
                var customformatted = new
                {
                    CreatedDateFormatted = clientobj.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                    FormattedCost = clientobj.PriceUnit + " " + clientobj.TotalLicenseCost.Value.ToString(),
                    status = systemuserobj.IsActive.HasValue ? (systemuserobj.IsActive.Value == true ? "Active" : "Inactive") : "Inactive",
                };

                var planobj = plan.GetByID(clientobj.PlanID.Value);
                var standdbproviderobj = standdbprovider.GetByID(clientobj.STandDBprovdiderID.Value);

                return WebJSResponse.ResponseSimple(new
                {
                    clientobjjson = clientobj,
                    systemuserobjjson = systemuserobj,
                    clientparametersjson = clientparameters,
                    customformattedjson = customformatted,
                    planobjjson = planobj,
                    standdbproviderobjjson = standdbproviderobj
                });
            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }


        [HttpPost]
        [Route("UnblockClient")]
        public JsonResult UnblockClient(int systemuserid)
        {
            try
            {
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();

                bool isunblock = systemuser.Unblock(systemuserid, false, true);

                if (isunblock)
                {
                    var sysuserobj = systemuser.GetByID(systemuserid);
                    int clientdetailid = sysuserobj.DetailID.Value;
                    Client cl = new Client(clientdetailid);
                    DBInitializer db = new DBInitializer(cl);
                    Repositories.User.IAdminDetailRepository addetailrepo = db.AdminDetailRepository();
                    Repositories.User.IClientUserRepository clrepo = db.ClientUserRepository();
                    Repositories.User.IClientUserTypeRepository cltyperepo = db.ClientUserTypeRepository();

                    int clientdetailtypeid = cltyperepo.GetAll().Where(x => x.TypeName.Equals("Client Admin")).FirstOrDefault().ClientUserTypeID;
                    int admindetailid = addetailrepo.GetAll().Where(x=>x.SystemClientID.Value==systemuserid).FirstOrDefault().AdminDetailID;
                    int clientuserid = clrepo.GetAll().Where(x=>x.DetailType.Value==clientdetailtypeid && x.DetailID.Value==admindetailid).FirstOrDefault().ClientUserID;
                    db.ClientUserRepository().Unblock(clientuserid, false, db.IncorrectPasswordAttemptRepository(), db.ClientUserRepository(),
                        db.ClientUserTypeRepository(), db.AdminDetailRepository());
                }

                return WebJSResponse.ResponseToastr(ToastrEnum.success, "User unblocked.", "User has been unblocked succussfully!", new { });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [HttpPost]
        [Route("GetProviderParameters")]
        public JsonResult GetProviderParameters(int standproviderid)
        {
            try
            {
                Repositories.Admin.IProviderParamterRepository provideparameter = new BLL.Admin.ProviderParamterRepository();
                Repositories.Admin.IProviderParameterMappingRepository provideparametermapping = new BLL.Admin.ProviderParameterMappingRepository();

                var parameterlist = (from pmp in provideparametermapping.GetAll()
                                    join pp in provideparameter.GetAll() on pmp.ParameterID equals pp.ParameterID
                                    where pmp.ProviderID.Value == standproviderid
                                    select pp).ToList();

                return WebJSResponse.ResponseSimple(new { parameterlistjson = parameterlist });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("ActiveClients")]
        [SystemAuthorizeMember]
        public ActionResult ActiveClients()
        {
            Repositories.Admin.ISystemUserRepository user = new BLL.Admin.SystemUserRepository();

            try
            {
                var activeblockedclients = user.GetAll().Where(x => (x.IsActive.HasValue ? x.IsActive.Value : false) &&
                    (x.IsBlock.HasValue ? x.IsBlock.Value : false) && x.DetailType.Value == 1).ToList();
                //bool isunblock = false;
                Client cl;
                DBInitializer db;
                foreach (var client in activeblockedclients)
                {
                    //isunblock = user.Unblock(client.SystemUserID, true,true);
                    //if (isunblock)
                    //{
                        var sysuserobj = user.GetByID(client.SystemUserID);
                        int clientdetailid = sysuserobj.DetailID.Value;
                        cl = new Client(clientdetailid);
                        db = new DBInitializer(cl);
                        Repositories.User.IAdminDetailRepository addetailrepo = db.AdminDetailRepository();
                        Repositories.User.IClientUserRepository clrepo = db.ClientUserRepository();
                        Repositories.User.IClientUserTypeRepository cltyperepo = db.ClientUserTypeRepository();

                        int clientdetailtypeid = cltyperepo.GetAll().Where(x => x.TypeName.Equals("Client Admin")).FirstOrDefault().ClientUserTypeID;
                        int admindetailid = addetailrepo.GetAll().Where(x => x.SystemClientID.Value == client.SystemUserID).FirstOrDefault().AdminDetailID;
                        int clientuserid = clrepo.GetAll().Where(x => x.DetailType.Value == clientdetailtypeid && x.DetailID.Value == admindetailid).FirstOrDefault().ClientUserID;
                        db.ClientUserRepository().Unblock(clientuserid, true, db.IncorrectPasswordAttemptRepository(), db.ClientUserRepository(),
                            db.ClientUserTypeRepository(), db.AdminDetailRepository());
                    //}
                }
            }
            catch (Exception ex)
            {

            }

            return View("~/Views/Admin/Client/ActiveClients.cshtml");
        }

        [Route("InactiveClients")]
        [SystemAuthorizeMember]
        public ActionResult InactiveClients()
        {
            Repositories.Admin.ISystemUserRepository user = new BLL.Admin.SystemUserRepository();

            try
            {
                var activeblockedclients = user.GetAll().Where(x => (x.IsActive.HasValue ? x.IsActive.Value : false) &&
                    (x.IsBlock.HasValue ? x.IsBlock.Value : false) && x.DetailType.Value == 1).ToList();
                //bool isunblock = false;
                Client cl;
                DBInitializer db;
                foreach (var client in activeblockedclients)
                {
                    //isunblock = user.Unblock(client.SystemUserID, true, true);
                    //if (isunblock)
                    //{
                        var sysuserobj = user.GetByID(client.SystemUserID);
                        int clientdetailid = sysuserobj.DetailID.Value;
                        cl = new Client(clientdetailid);
                        db = new DBInitializer(cl);
                        Repositories.User.IAdminDetailRepository addetailrepo = db.AdminDetailRepository();
                        Repositories.User.IClientUserRepository clrepo = db.ClientUserRepository();
                        Repositories.User.IClientUserTypeRepository cltyperepo = db.ClientUserTypeRepository();

                        int clientdetailtypeid = cltyperepo.GetAll().Where(x => x.TypeName.Equals("Client Admin")).FirstOrDefault().ClientUserTypeID;
                        int admindetailid = addetailrepo.GetAll().Where(x => x.SystemClientID.Value == client.SystemUserID).FirstOrDefault().AdminDetailID;
                        int clientuserid = clrepo.GetAll().Where(x => x.DetailType.Value == clientdetailtypeid && x.DetailID.Value == admindetailid).FirstOrDefault().ClientUserID;
                        db.ClientUserRepository().Unblock(clientuserid, true, db.IncorrectPasswordAttemptRepository(), db.ClientUserRepository(),
                            db.ClientUserTypeRepository(), db.AdminDetailRepository());
                    //}
                }
            }
            catch (Exception ex)
            {

            }
            return View("~/Views/Admin/Client/InactiveClients.cshtml");
        }
    }
}