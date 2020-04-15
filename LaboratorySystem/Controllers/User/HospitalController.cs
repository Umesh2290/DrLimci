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
    [RoutePrefix("User/Hospital")]
    public class HospitalController : RedirectController
    {
        // GET: Hospital
        [Route]
        [ClientAuthorizeMember]
        public ActionResult Index()
        {
            if (!HelpingClass.ValidateConnection())
            {
                throw new Exception("LabSystemDB_Connection does not establish!");
            }

            ViewBag.currentdomaindb = this.currentdomaindb;
            return View("~/Views/User/Hospital/Index.cshtml");
        }
        [HttpPost]
        [Route("Create")]
        public JsonResult Create(string hospitalname,  string phoneno, string address, string city,  string faxno,string hospitalCode,
            string firstname, string lastname, string username, string email,
             string mobileno,string password, int editid)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IHospitalDetail hospitalDetail  = this.currentdomaindb.HospitalDetailRepository();
                Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();


                bool ishospitalcreated = false;

                if (editid == 0)
                {
                    
                    BusinessPOCO.User.Cl_HospitalDetail hospitaldetailobj = new BusinessPOCO.User.Cl_HospitalDetail();
                    hospitaldetailobj.HospitalName = hospitalname.Trim();
                    hospitaldetailobj.Address = address.Trim();
                    hospitaldetailobj.City = city.Trim();
                    hospitaldetailobj.PhoneNo = phoneno.Trim();
                    hospitaldetailobj.CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                    hospitaldetailobj.CreatedDate = DateTime.Now;
                    hospitaldetailobj.FaxNo = faxno.Trim();
                    hospitaldetailobj.HospitalCode = hospitalCode.Trim();
                    hospitalDetail.Insert(hospitaldetailobj);

                    hospitalDetail.Save();
                    ishospitalcreated = true;

                    string _randompassword = password;
                    byte[] pass = System.Text.Encoding.UTF8.GetBytes(_randompassword);
                    string _encryptedpassword = System.Convert.ToBase64String(pass);
                    var clientuserobj = new BusinessPOCO.User.Cl_ClientUser();
                    clientuserobj.FirstName = firstname.Trim();
                    clientuserobj.LastName = lastname.Trim();
                    clientuserobj.Username = username.Trim();//+ "@" + clpoco.CompanyName.Replace(" ", "").Trim();
                    clientuserobj.Password = _encryptedpassword;
                    clientuserobj.Email = email.Trim();
                    clientuserobj.MobileNo = mobileno.Trim();
                    clientuserobj.Address = address.Trim();
                    clientuserobj.IsActive = true;
                    clientuserobj.IsBlock = false;
                    clientuserobj.CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                    clientuserobj.CreatedDate = DateTime.Now;
                    clientuserobj.DetailType = 9;
                    clientuserobj.DetailID = 3;
                    clientuserobj.HospitalDetailID = hospitaldetailobj.HospitalDetailID;

                    clientuser.Insert(clientuserobj);
                    clientuser.Save();
                    var rolemappingobj = new BusinessPOCO.User.Cl_RoleMapping();
                    rolemappingobj.RoleID = 9;
                    rolemappingobj.UserID = clientuserobj.ClientUserID;
                    rolemappingobj.IsDefault = true;
                    rolemappingobj.CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                    rolemappingobj.CreatedDate = DateTime.Now;

                    rolemapping.Insert(rolemappingobj);
                    rolemapping.Save();


                    if (ishospitalcreated)
                    {
                        return WebJSResponse.ResponseSWAL(SwalEnum.success, "Created Successfully !", "Hospital has been created successfully<br>", new { hospitaldetailjson = hospitaldetailobj });
                    }
                    else
                    {
                        
                        return WebJSResponse.ResponseSWAL(SwalEnum.warning, "Created Successfully !", "Hospital has been created successfully<br>but something went wrong.", new { hospitaldetailjson = hospitaldetailobj });
                    }


                }

                return WebJSResponse.ResponseToastr(ToastrEnum.error, "In edit phase", "Currently page is in edit phase!please try again by clicking on menu.", new { });

            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        //[HttpPost]
        //[Route("Update")]
        //public JsonResult Update(string firstname, string middlename, string lastname,
        //   string email, string mobileno, string address, bool status, string age, string gender, string streetname,
        //   string city, string alternateaddress, string alternatephoneno, string referingdoctor, string referinghospital, string paymentmode,
        //       decimal payment, int editid)
        //{
        //    try
        //    {
        //        Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
        //        Repositories.User.IHospitalDetail hospitalDetail = this.currentdomaindb.HospitalDetailRepository();
        //        Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();



        //        if (editid > 0)
        //        {
        //            if (clientuser.GetAll().Where(x => ((string.IsNullOrEmpty(x.Email) ? false : x.Email.Equals(email.Trim())) && x.ClientUserID != editid)).Count() > 0)
        //            {
        //                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Username or email already exist !", "Please try another !", new { });

        //            }

        //            var clientuserobj = clientuser.GetByID(editid);
        //            if (clientuserobj != null)
        //            {
        //                clientuserobj.FirstName = firstname.Trim();
        //                clientuserobj.LastName = lastname.Trim();
        //                clientuserobj.Email = email.Trim();
        //                clientuserobj.MobileNo = mobileno.Trim();
        //                clientuserobj.Address = address.Trim();
        //                clientuserobj.IsActive = status;
        //                clientuserobj.UpdatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
        //                clientuserobj.UpdatedDate = DateTime.Now;

        //                clientuser.Update(clientuserobj);
        //                clientuser.Save();

        //                BusinessPOCO.User.Cl_PatientDetail patientdetailobj = patientdetail.GetByID(clientuserobj.DetailID.Value);

        //                if (patientdetailobj != null)
        //                {
        //                    patientdetailobj.Age = age.Trim();
        //                    patientdetailobj.AlternateAddress = alternateaddress.Trim();
        //                    patientdetailobj.AlternatePhoneNo = alternatephoneno.Trim();
        //                    patientdetailobj.City = city.Trim();
        //                    patientdetailobj.UpdatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
        //                    patientdetailobj.UpdatedDate = DateTime.Now;
        //                    patientdetailobj.MiddleName = middlename.Trim();
        //                    patientdetailobj.Payment = payment;
        //                    patientdetailobj.PaymentMode = paymentmode.Trim();
        //                    patientdetailobj.ReferingDoctor = referingdoctor.Trim();
        //                    patientdetailobj.ReferingHospital = referinghospital.Trim();
        //                    patientdetailobj.Sex = gender.Trim();
        //                    patientdetailobj.Streetname = streetname.Trim();

        //                    patientdetail.Update(patientdetailobj);

        //                    patientdetail.Save();
        //                }

        //                bool ispdfcreated = false;
        //                string paymentpdflink = string.Empty;
        //                string pdflink = string.Empty;

        //                try
        //                {
        //                    FileManager fmd = PdfCreator.CreatePatientPDF(clientuserobj.ClientUserID, this.subdomainurl, this.currentdomaindb, out pdflink);
        //                    FileManager fmp = PdfCreator.CreatePatientPaymentPDF(clientuserobj.ClientUserID, this.subdomainurl, this.currentdomaindb, out paymentpdflink);

        //                    if (patientdetailobj != null)
        //                    {
        //                        patientdetailobj.PdfLink = HelpingClass.LocalUploadPathToRelativeWebPath(pdflink);
        //                        patientdetailobj.PaymentReceiptPdfLink = HelpingClass.LocalUploadPathToRelativeWebPath(paymentpdflink);

        //                        patientdetail.Update(patientdetailobj);
        //                        patientdetail.Save();
        //                    }
        //                    ispdfcreated = true;

        //                }
        //                catch (Exception ex)
        //                {
        //                    ispdfcreated = false;
        //                }


        //                if (ispdfcreated)
        //                {
        //                    return WebJSResponse.ResponseSWAL(SwalEnum.success, "Updated Successfully !", "Patient has been updated successfully<br>", new { clientuserjson = clientuserobj, patientdetailjson = patientdetailobj });
        //                }
        //                else
        //                {
        //                    return WebJSResponse.ResponseSWAL(SwalEnum.success, "Updated Successfully !", "Patient has been updated successfully<br>but something went wrong while generating pdf.", new { clientuserjson = clientuserobj, patientdetailjson = patientdetailobj });
        //                }
        //            }

        //            else
        //            {
        //                return WebJSResponse.ResponseToastr(ToastrEnum.error, "No patient found !", "There is no patient associated to this patient id.", new { });
        //            }


        //        }

        //        return WebJSResponse.ResponseToastr(ToastrEnum.error, "In create phase", "Currently page is in create phase!please try again by clicking on edit button of record.", new { });

        //    }
        //    catch (Exception ex)
        //    {
        //        return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
        //    }
        //}

        [HttpPost]
        [Route("GetList")]
        public JsonResult GetList()
        {
            try
            {
                Repositories.User.IHospitalDetail hospital = this.currentdomaindb.HospitalDetailRepository();

                object returnlist = null;
                returnlist = (from hos in hospital.GetAll()
                              select new
                              {
                                  hos.HospitalDetailID,
                                  hos.HospitalName,
                                  hos.Address,
                                  hos.City,
                                  hos.PhoneNo,
                                  hos.FaxNo,                                 
                                  hos.UpdatedBy,
                                  hos.UpdatedDate,                                  
                                  hos.CreatedDate,
                                  hos.CreatedBy,
                                  
                              }).OrderByDescending(x => x.HospitalDetailID).ToList();



                return WebJSResponse.ResponseSimple(new { hospitaljson = returnlist });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("ViewHospitals")]
        [ClientAuthorizeMember]
        //[SystemAuthorizeMember]

        public ActionResult ViewHospitals()
        {
            return View("~/Views/User/Hospital/ViewHospitals.cshtml");
        }

    }
}