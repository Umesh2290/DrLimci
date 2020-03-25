using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repositories = LaboratoryBusiness.Repositories;
using BLL = LaboratoryBusiness.BLL;
using BusinessPOCO = LaboratoryBusiness.POCO;
using System.Web.Script.Serialization;

namespace LaboratorySystem.Controllers.Admin
{
    [RoutePrefix("Admin/Plan")]
    public class PlanController : RedirectController
    {
        // GET: Plan
        [Route]
        // GET: Home
        [SystemAuthorizeMember]
        public ActionResult Index()
        {
            return View("~/Views/Admin/Plan/Index.cshtml");
        }

        [HttpPost]
        [Route("Create")]
        public JsonResult Create(string planname, string plandescription, string plandetail, string plancost, int planstatus,int editid)
        {
            try
            {
                Repositories.Admin.IPlanRepository plan = new BLL.Admin.PlanRepository();

                if (editid == 0)
                {
                    if (plan.GetAll().Where(x => x.PlanName.Trim().Equals(planname.Trim())).Count() > 0)
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "Plan name already exist !", "Please try another !", new { });

                    }

                    var planobj = new BusinessPOCO.Admin.Plan();
                    planobj.PlanName = planname;
                    planobj.PlanDescription = plandescription;
                    planobj.PlanDetail = plandetail;
                    planobj.PlanStatus = planstatus;
                    planobj.PlanCost = Convert.ToDecimal(plancost);
                    planobj.CreatedBy = MySession.SystemSession.SystemUserID;
                    planobj.CreatedDate = DateTime.Now;

                    plan.Insert(planobj);

                    plan.Save();

                    return WebJSResponse.ResponseSWAL(SwalEnum.success, "Created Successfully !", "Plan has been created successfully<br>", new { planjson = planobj });

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
        public JsonResult Update(string planname, string plandescription, string plandetail, string plancost, int planstatus, int editid)
        {
            try
            {
                Repositories.Admin.IPlanRepository plan = new BLL.Admin.PlanRepository();

                if (editid > 0)
                {

                    var planobj = plan.GetByID(editid);
                    if (planobj != null)
                    {
                        planobj.PlanDescription = plandescription;
                        planobj.PlanDetail = plandetail;
                        planobj.PlanStatus = planstatus;
                        planobj.PlanCost = Convert.ToDecimal(plancost);
                        planobj.UpdatedBy = MySession.SystemSession.SystemUserID;
                        planobj.UpdatedDate = DateTime.Now;

                        plan.Update(planobj);

                        plan.Save();

                        return WebJSResponse.ResponseSWAL(SwalEnum.success, "Updated Successfully !", "Plan has been updated successfully<br>", new { planjson = planobj });
                    }

                    else
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "No plan found !", "There is no plan associated to this plan id.", new { });
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
        public JsonResult Get(int planid)
        {
            try
            {
                Repositories.Admin.IPlanRepository plan = new BLL.Admin.PlanRepository();

                if (planid > 0)
                {

                    var planobj = plan.GetByID(planid);
                    if (planobj != null)
                    {
                        return WebJSResponse.ResponseSimple(new { planjson = planobj });
                    }

                    else
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "No plan found !", "There is no plan associated to this plan id.", new { });
                    }


                }

                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Incorrect plan-id !", "Kindly provide correct plan id.", new { });


            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [HttpPost]
        [Route("GetList")]
        public JsonResult GetList(int planstatus)
        {
            try
            {
                Repositories.Admin.IPlanRepository plan = new BLL.Admin.PlanRepository();
                object returnlist = null;
                if (planstatus == 1 || planstatus == 2)
                {
                    returnlist = (from p in plan.GetAll()
                                      select new
                                      {
                                          p.PlanID,
                                          p.PlanName,
                                          p.PlanStatus,
                                          p.PlanDescription,
                                          p.PlanDetail,
                                          p.PlanCost,
                                          p.CreatedBy,
                                          p.CreatedDate,
                                          p.UpdatedBy,
                                          p.UpdatedDate,
                                          status = p.PlanStatus.Value == 1 ? "Active" : "Draft"
                                      }).Where(x => x.PlanStatus.Value == 1 || x.PlanStatus == 2).OrderByDescending(x=>x.PlanID).ToList();
                }

                else if (planstatus == 0)
                {
                    returnlist = (from p in plan.GetAll()
                                      select new
                                      {
                                          p.PlanID,
                                          p.PlanName,
                                          p.PlanStatus,
                                          p.PlanDescription,
                                          p.PlanDetail,
                                          p.PlanCost,
                                          p.CreatedBy,
                                          p.CreatedDate,
                                          p.UpdatedBy,
                                          p.UpdatedDate,
                                          status = p.PlanStatus.Value == 0 ? "Inactive" : "N/A"
                                      }).Where(x => x.PlanStatus.Value == 0).OrderByDescending(x => x.PlanID).ToList();
                }

                else
                {
                    returnlist = new { };
                }


                return WebJSResponse.ResponseSimple(new { planjson = returnlist });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [HttpPost]
        [Route("GetDetail")]
        public JsonResult GetDetail(int planid)
        {
            try
            {
                Repositories.Admin.IPlanRepository plan = new BLL.Admin.PlanRepository();
                object returnlist = null;
                var data = plan.GetByID(planid);
                if (data != null)
                {
                    returnlist = new
                    {
                        data.PlanID,
                        data.PlanName,
                        data.PlanStatus,
                        data.PlanDetail,
                        data.PlanDescription,
                        data.PlanCost,
                        data.CreatedBy,
                        data.CreatedDate,
                        data.UpdatedBy,
                        data.UpdatedDate,
                        status = data.PlanStatus.Value == 1 ? "Active" : (data.PlanStatus.Value == 2 ? "Draft" : "Inactive")
                    };
                }

                return WebJSResponse.ResponseSimple(new { planjson = returnlist });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("ActivePlans")]
        [SystemAuthorizeMember]
        public ActionResult ActivePlans()
        {
            return View("~/Views/Admin/Plan/ActivePlans.cshtml");
        }

        [Route("InactivePlans")]
        [SystemAuthorizeMember]
        public ActionResult InactivePlans()
        {
            return View("~/Views/Admin/Plan/InactivePlans.cshtml");
        } 

    }
}