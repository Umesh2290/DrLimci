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
    [RoutePrefix("User/RateList")]
    public class RateListController : RedirectController
    {
        [Route]
        [ClientAuthorizeMember]
        // GET: RateList
        public ActionResult Index()
        {
            ViewBag.currentdomaindb = this.currentdomaindb;

            return View("~/Views/User/RateList/Index.cshtml");
            //return View();
        }
        [HttpPost]
        [Route("Create")]
        public JsonResult Create(string testname, int testrate,int editid)
        {
            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.ITestRate testRate = this.currentdomaindb.TestRate();
                Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();


             
                if (editid == 0)
                {

                    BusinessPOCO.User.Cl_TestRate cl_TestRate = new BusinessPOCO.User.Cl_TestRate();
                    cl_TestRate.TestName = testname.Trim();
                    cl_TestRate.Cost = testrate;

                    testRate.Insert(cl_TestRate);
                    testRate.Save();
                                   
                    return WebJSResponse.ResponseSWAL(SwalEnum.success, "Created Successfully !", "Test Rate  has been created successfully<br>", new { testratejson = testRate });
                   


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
        public JsonResult Update(string testname, int testrate, int editid)
        {
            try
            {
                //Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.ITestRate testRate = this.currentdomaindb.TestRate();
                //Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();



                if (editid > 0)
                {

                    BusinessPOCO.User.Cl_TestRate cl_TestRate = new BusinessPOCO.User.Cl_TestRate();
                    cl_TestRate.ID = editid;
                    cl_TestRate.TestName = testname.Trim();
                    cl_TestRate.Cost = testrate;

                    testRate.Update(cl_TestRate);
                    testRate.Save();
                    return WebJSResponse.ResponseSWAL(SwalEnum.success, "Updated Successfully !", "Test Rate has been updated successfully<br>", new { testratejson = testRate });

                }
                
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "In edit phase", "Currently page is in edit phase!please try again by clicking on menu.", new { });

            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [HttpPost]
        [Route("Get")]
        public JsonResult Get(int id)
        {
            try
            {
               // Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.ITestRate testRate = this.currentdomaindb.TestRate();


                if (id > 0)
                {

                   
                    var testrateobj = testRate.GetAll().Where(x => x.ID == id).FirstOrDefault();
                    if (testrateobj != null )
                    {
                        var testrateobj1 = new
                        {
                            testrateobj.TestName,
                            testrateobj.Cost,
                            testrateobj.ID
                        };

                       
                        return WebJSResponse.ResponseSimple(new { testratejson = testrateobj1 });
                    }

                    else
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "No test rate found !", "There is no test rate associated to this test rate id.", new { });
                    }


                }

                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Incorrect test rate -id !", "Kindly provide correct test rate  id.", new { });


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
                Repositories.User.ITestRate testRate = this.currentdomaindb.TestRate();

                object returnlist = null;
                returnlist = (from lab in testRate.GetAll()
                              select new
                              {
                                  lab.ID,
                                  lab.TestName,
                                  lab.Cost
                                 

                              }).OrderByDescending(x => x.ID).ToList();



                return WebJSResponse.ResponseSimple(new { testratejson = returnlist });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("ViewAll")]
        [ClientAuthorizeMember]

        [HttpGet]
        public ActionResult ViewAll()
        {
            Repositories.User.ITestRate testRate = this.currentdomaindb.TestRate();

            var testall = testRate.GetAll();

            return View("~/Views/User/RateList/ViewAll.cshtml");
        }
    }
}