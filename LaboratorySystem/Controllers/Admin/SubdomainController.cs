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
    [RoutePrefix("Admin/Subdomain")]
    public class SubdomainController : RedirectController
    {

        [HttpPost]
        [Route("GetList")]
        public JsonResult GetList(bool status)
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
                                  where su.IsActive.Value == status
                                  select new
                                  {
                                      cl.ClientDetailID,
                                      cl.Subdomain,
                                      ActivatedOn = cl.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                                      st.ProviderName,
                                      status = su.IsActive.HasValue ? (su.IsActive.Value == true ? "Active" : "Inactive") : "Inactive",
                                      su.SystemUserID,
                                  }).ToList();


                return WebJSResponse.ResponseSimple(new { subdomainjson = returnList });




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

        [Route("ActiveSubdomains")]
        [SystemAuthorizeMember]
        public ActionResult ActiveSubdomains()
        {

            return View("~/Views/Admin/Subdomain/ActiveSubdomains.cshtml");
        }

        [Route("InactiveSubdomains")]
        [SystemAuthorizeMember]
        public ActionResult InactiveSubdomains()
        {
            return View("~/Views/Admin/Subdomain/InactiveSubdomains.cshtml");
        }
    }
}