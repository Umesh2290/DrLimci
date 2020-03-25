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
    [RoutePrefix("Admin/Home")]
    public class HomeController : RedirectController
    {
        // GET: Home
        [Route]
        // GET: Home
        [SystemAuthorizeMember]
        public ActionResult Index()
        {
            foreach (var role in MySession.SystemSession.AssignedRoles)
            {
                if (role.IsDefault == true)
                {
                    ViewBag.RoleIDForDashboard = role.RoleID;
                }
            }

            return View("~/Views/Admin/Home/Index.cshtml");
        }
        [HttpGet]
        [Route("GetDomainData")]
        public ActionResult GetDomainData(string status)
        {
            try
            {
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();
                Repositories.Admin.ISystemUserTypeRepository systemsusertype = new BLL.Admin.SystemUserTypeRepository();
                Repositories.Admin.IClientRepository client = new BLL.Admin.ClientRepository();
                Repositories.Admin.IStorageandDBProviderRepository standdbprovider = new BLL.Admin.StorageandDBProviderRepository();


                int clientdetailtypeid = systemsusertype.GetAll().Where(x => x.TypeName.Equals("Client")).FirstOrDefault().SystemUserTypeID;

                var returnList = (from cl in client.GetAll()
                                  join su in systemuser.GetAll() on cl.ClientDetailID equals su.DetailID
                                  join st in standdbprovider.GetAll() on cl.STandDBprovdiderID equals st.ProviderID
                                  where su.IsActive.Value == true
                                  select new
                                  {
                                      cl.ClientDetailID,
                                      cl.Subdomain,
                                      ActivatedOn = cl.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                                      st.ProviderName,
                                      status = su.IsActive.HasValue ? (su.IsActive.Value == true ? "Active" : "Inactive") : "Inactive",
                                      su.SystemUserID,
                                  }).ToList().GroupBy(e => e.ActivatedOn);


                return WebJSResponse.ResponseSimple(new { subdomainjson = returnList });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }
        [HttpGet]
        [Route("GetClientData")]
        public ActionResult GetClientData(string status)
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
                                  where su.DetailType.Value == clientdetailtypeid
                                  select new
                                  {
                                      CreatedDate = cl.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                                      cl.ClientDetailID,
                                      cl.CompanyName,
                                      cl.Subdomain,
                                      pl.PlanName,
                                      status = su.IsActive.HasValue ? (su.IsActive.Value == true ? "Active" : "Inactive") : "Inactive",
                                      su.SystemUserID,
                                      FullName = su.FirstName + " " + su.MiddleName + " " + su.LastName,
                                      su.IsBlock
                                  }).ToList().GroupBy(e => e.CreatedDate);


                return WebJSResponse.ResponseSimple(new { clientjson = returnList });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }

        }
        [HttpGet]
        [Route("ActiveInActiveClient")]
        public ActionResult ActiveInActiveClient(string status)
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
                                  where su.IsActive == true && su.DetailType.Value == clientdetailtypeid
                                  select new
                                  {
                                      CreatedDate = cl.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                                      cl.ClientDetailID,
                                      cl.CompanyName,
                                      cl.Subdomain,
                                      pl.PlanName,
                                      status = su.IsActive.HasValue ? (su.IsActive.Value == true ? "Active" : "Inactive") : "Inactive",
                                      su.SystemUserID,
                                      FullName = su.FirstName + " " + su.MiddleName + " " + su.LastName,
                                      su.IsBlock
                                  }).ToList().Count;
                var returnList2 = (from cl in client.GetAll()
                                   join su in systemuser.GetAll() on cl.ClientDetailID equals su.DetailID
                                   join pl in plan.GetAll() on cl.PlanID equals pl.PlanID
                                   where su.IsActive == false && su.DetailType.Value == clientdetailtypeid
                                   select new
                                   {
                                       CreatedDate = cl.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                                       cl.ClientDetailID,
                                       cl.CompanyName,
                                       cl.Subdomain,
                                       pl.PlanName,
                                       status = su.IsActive.HasValue ? (su.IsActive.Value == true ? "Active" : "Inactive") : "Inactive",
                                       su.SystemUserID,
                                       FullName = su.FirstName + " " + su.MiddleName + " " + su.LastName,
                                       su.IsBlock
                                   }).ToList().Count;


                return WebJSResponse.ResponseSimple(new { activeClients = returnList, returnList2 });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }

        }
        [HttpGet]
        [Route("ContactRequest")]
        public ActionResult ContactRequest(string status)
        {
            try
            {
                Repositories.Admin.ISalesAndContactQueryRepository sand = new BLL.Admin.SalesAndContactQueryRepository();
                Repositories.Admin.ISalesAndContactQueryStatusRepository sacqsr = new BLL.Admin.SalesAndContactQueryStatusRepository();



                int salesCount = sand.GetAll().Where(x => x.TypeID.Equals(1)).ToList().Count;
                int contactCount = sand.GetAll().Where(x => x.TypeID.Equals(2)).ToList().Count;

                int[] data = new int[2];
                data[0] = salesCount;
                data[1] = contactCount;

                return WebJSResponse.ResponseSimple(new { contactjson = data });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }

        }
        [HttpGet]
        [Route("NewPendingClosed")]
        public ActionResult NewPendingClosed(string status)
        {
            try
            {
                Repositories.Admin.ISalesAndContactQueryRepository sand = new BLL.Admin.SalesAndContactQueryRepository();
                Repositories.Admin.ISalesAndContactQueryStatusRepository sacqsr = new BLL.Admin.SalesAndContactQueryStatusRepository();

                var createdDates = (from cl in sand.GetAll()
                                    select new
                                    {
                                        CreatedDate = cl.RequestCreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                                    }).ToArray();



                var newRequest = (from cl in sand.GetAll().
                                      Where(x => x.StatusID.Equals(1))
                                  select new
                                  {
                                      CreatedDate = cl.RequestCreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                                  }).ToList().GroupBy(e => e.CreatedDate);

                var pendingRequest = (from cl in sand.GetAll().
                                      Where(x => x.StatusID.Equals(2))
                                      select new
                                      {
                                          CreatedDate = cl.RequestCreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                                      }).ToList().GroupBy(e => e.CreatedDate);

                var closedRequest = (from cl in sand.GetAll().
                                       Where(x => x.StatusID.Equals(3))
                                     select new
                                     {
                                         CreatedDate = cl.RequestCreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                                     }).ToList().GroupBy(e => e.CreatedDate);



                return WebJSResponse.ResponseSimple(new { newRequest, pendingRequest, closedRequest, createdDates });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }

        }
    }
}