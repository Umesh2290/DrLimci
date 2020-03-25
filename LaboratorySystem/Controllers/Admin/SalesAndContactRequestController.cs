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
    [RoutePrefix("Admin/SalesAndContactRequest")]
    public class SalesAndContactRequestController : RedirectController
    {

        [HttpPost]
        [Route("GetList")]
        public JsonResult GetList(int typeid,int statusid)
        {
            try
            {
                Repositories.Admin.ISalesAndContactQueryTypeRepository querytype = new BLL.Admin.SalesAndContactQueryTypeRepository();
                Repositories.Admin.ISalesAndContactQueryStatusRepository querystatus = new BLL.Admin.SalesAndContactQueryStatusRepository();
                Repositories.Admin.ISalesAndContactQueryRepository salesandcontact = new BLL.Admin.SalesAndContactQueryRepository();



                var returnList = (from sc in salesandcontact.GetAll()
                                  join qs in querystatus.GetAll() on sc.StatusID equals qs.SalesAndContactQueryStatusID
                                  where sc.StatusID.Value == statusid && sc.TypeID.Value == typeid
                                  select new { sc.SalesAndContactQueryID, sc.Name, sc.Email, sc.ContactNo, qs.StatusName }).ToList();




                return WebJSResponse.ResponseSimple(new { subdomainjson = returnList });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("GetDetail")]
        [HttpPost]
        public JsonResult GetDetail(int salesandcontactqueryid)
        {
            try
            {
                Repositories.Admin.ISalesAndContactQueryTypeRepository querytype = new BLL.Admin.SalesAndContactQueryTypeRepository();
                Repositories.Admin.ISalesAndContactQueryStatusRepository querystatus = new BLL.Admin.SalesAndContactQueryStatusRepository();
                Repositories.Admin.ISalesAndContactQueryRepository salesandcontact = new BLL.Admin.SalesAndContactQueryRepository();
                Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();

                var salesandcontactobj = (from sc in salesandcontact.GetAll()
                                  join qs in querystatus.GetAll() on sc.StatusID equals qs.SalesAndContactQueryStatusID
                                  where sc.SalesAndContactQueryID == salesandcontactqueryid
                                          select new { sc, qs.StatusName, RequestCreatedDate = sc.RequestCreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt") }).ToList();

                object newaction = null;
                object pendingaction = null;

                if (salesandcontactobj != null)
                {
                    if (salesandcontactobj.FirstOrDefault().sc.NewActionBy.HasValue)
                    {
                        var sysuser = systemuser.GetByID(salesandcontactobj.FirstOrDefault().sc.NewActionBy.Value);
                        var status = querystatus.GetByID(salesandcontactobj.FirstOrDefault().sc.NewActionStatusID.Value);

                        newaction = new { NewActionDate = salesandcontactobj.FirstOrDefault().sc.NewActionDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                                          NewActionBy = sysuser.FirstName + " " + sysuser.MiddleName + " " + sysuser.LastName,
                                          NewActionStatus = status.StatusName
                        };
                    }

                    if (salesandcontactobj.FirstOrDefault().sc.PendingActionBy.HasValue)
                    {
                        var sysuser = systemuser.GetByID(salesandcontactobj.FirstOrDefault().sc.PendingActionBy.Value);

                        pendingaction = new
                        {
                            PendingActionDate = salesandcontactobj.FirstOrDefault().sc.PendingActionDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                            PendingActionBy = sysuser.FirstName + " " + sysuser.MiddleName + " " + sysuser.LastName
                        };
                    }
                }

                return WebJSResponse.ResponseSimple(new { salesandcontactobjjson = salesandcontactobj,newactionjson=newaction,pendingactionjson=pendingaction });

            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("UpdateStatus")]
        [HttpPost]
        public JsonResult UpdateStatus(int salesandcontactqueryid,string comments,int statusid,int pageid)
        {
            try
            {
                Repositories.Admin.ISalesAndContactQueryTypeRepository querytype = new BLL.Admin.SalesAndContactQueryTypeRepository();
                Repositories.Admin.ISalesAndContactQueryStatusRepository querystatus = new BLL.Admin.SalesAndContactQueryStatusRepository();
                Repositories.Admin.ISalesAndContactQueryRepository salesandcontact = new BLL.Admin.SalesAndContactQueryRepository();

                string statusname = querystatus.GetByID(statusid).StatusName;


                var salesandcontactobj = salesandcontact.GetByID(salesandcontactqueryid);
                salesandcontactobj.StatusID = statusid;
                if (pageid == 1)
                {
                    salesandcontactobj.NewActionBy = MySession.SystemSession.SystemUserID;
                    salesandcontactobj.NewActionComments = comments;
                    salesandcontactobj.NewActionDate = DateTime.Now;
                    salesandcontactobj.NewActionStatusID = statusid;
                }

                else if (pageid == 2)
                {
                    salesandcontactobj.PendingActionBy = MySession.SystemSession.SystemUserID;
                    salesandcontactobj.PendingActionComments = comments;
                    salesandcontactobj.PendingActionDate = DateTime.Now;
                }

                salesandcontact.Update(salesandcontactobj);

                salesandcontact.Save();

                if (statusname.Contains("Deleted"))
                {
                    return WebJSResponse.ResponseSWAL(SwalEnum.success, "Deleted Successfully !", "Request has been deleted successfully<br>", new { salesandcontactjson = salesandcontactobj });
                }
                else
                {
                    return WebJSResponse.ResponseSWAL(SwalEnum.success, "Updated Successfully !", "Status has been updated successfully<br>", new { salesandcontactjson = salesandcontactobj });
                }

            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }


        [Route("NewSales")]
        [SystemAuthorizeMember]
        public ActionResult NewSales()
        {
            return View("~/Views/Admin/SalesAndContactRequest/NewSales.cshtml");
        }

        [Route("PendingSales")]
        [SystemAuthorizeMember]
        public ActionResult PendingSales()
        {
            return View("~/Views/Admin/SalesAndContactRequest/PendingSales.cshtml");
        }

        [Route("ClosedSales")]
        [SystemAuthorizeMember]
        public ActionResult ClosedSales()
        {
            return View("~/Views/Admin/SalesAndContactRequest/ClosedSales.cshtml");
        }


        [Route("NewContact")]
        [SystemAuthorizeMember]
        public ActionResult NewContact()
        {
            return View("~/Views/Admin/SalesAndContactRequest/NewContact.cshtml");
        }

        [Route("PendingContact")]
        [SystemAuthorizeMember]
        public ActionResult PendingContact()
        {
            return View("~/Views/Admin/SalesAndContactRequest/PendingContact.cshtml");
        }

        [Route("ClosedContact")]
        [SystemAuthorizeMember]
        public ActionResult ClosedContact()
        {
            return View("~/Views/Admin/SalesAndContactRequest/ClosedContact.cshtml");
        }
    }
}