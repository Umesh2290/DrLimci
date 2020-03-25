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
        public JsonResult Create(string hospitalname,  
             string phoneno, string address, string city,  string faxno,  int editid)
        {
            try
            {
               
                Repositories.User.IHospitalDetail hospitalDetail  = this.currentdomaindb.HospitalDetailRepository();
                
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
                    hospitalDetail.Insert(hospitaldetailobj);

                    hospitalDetail.Save();
                    ishospitalcreated = true;

                   
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

        [Route("GetAllHospital")]
        [ClientAuthorizeMember]
        //[SystemAuthorizeMember]

        public ActionResult ActiveEmployees()
        {
            return View("~/Views/User/Hospital/GetAllHospital.cshtml");
        }

    }
}