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
    [RoutePrefix("User/InventoryRequest")]
    public class InventoryRequestController : RedirectController
    {

        [HttpPost]
        [Route("RaiseCreate")]
        public JsonResult RaiseCreate(string itemname, string description, string quantity, string comments)
        {
            try
            {
                Repositories.User.IInventoryRequestRepository inventoryrequest = this.currentdomaindb.InventoryRequestRepository();
                Repositories.User.IInventoryRequestStatusRepository inventoryrequeststatus = this.currentdomaindb.InventoryRequestStatusRepository();
                Repositories.User.IRoleRepository role = this.currentdomaindb.RoleRepository();
                Repositories.User.IRoleMappingRepository rolemapping = this.currentdomaindb.RoleMappingRepository();
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();


                BusinessPOCO.User.Cl_InventoryRequestStatus requeststatus = inventoryrequeststatus.GetAll().Where(x => x.StatusName.Equals("Open")).FirstOrDefault();

                BusinessPOCO.User.Cl_InventoryRequest inventoryrequestobj = new BusinessPOCO.User.Cl_InventoryRequest();
                inventoryrequestobj.ItemName = itemname.Trim();
                inventoryrequestobj.Quantity = quantity.Trim();
                inventoryrequestobj.RequestCreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                inventoryrequestobj.RequestCreatedDate = DateTime.Now;
                inventoryrequestobj.Description = description.Trim();
                inventoryrequestobj.Comments = comments.Trim();
                inventoryrequestobj.StatusID = requeststatus.InventoryRequestStatusID;

                    
                inventoryrequest.Insert(inventoryrequestobj);

                inventoryrequest.Save();

                var users = (from rl in role.GetAll()
                             join rlm in rolemapping.GetAll() on rl.RoleID equals rlm.RoleID
                             join cu in clientuser.GetAll() on rlm.UserID equals cu.ClientUserID
                             where rl.RoleName.Equals("Inventory Manager") || rl.RoleName.Equals("Admin")
                             select cu).ToList();
                foreach (var user in users)
                {
                    NotificationManager.FireOnClient("Inventory Request",
                        "Request ID # " + inventoryrequestobj.InventoryRequestID.ToString() + " , Item Name : " + inventoryrequestobj.ItemName,
                        "/User/InventoryRequest/OpenRequests", user.ClientUserID, MySession.GetClientSession(this.subdomainurl).ClientUserID, this.currentdomaindb,
                        "i-Speach-Bubble-6 text-primary mr-1", "Inventory Request", "Request ID # " + inventoryrequestobj.InventoryRequestID.ToString() + " , Item Name : " + inventoryrequestobj.ItemName, "toastr-sucsess");
                }

                return WebJSResponse.ResponseSWAL(SwalEnum.success, "Created Successfully !", "Inventory request has been created successfully<br>", new {  });


            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [HttpPost]
        [Route("GetList")]
        public JsonResult GetList(string status)
        {
            try
            {
                Repositories.User.IInventoryRequestRepository inventoryrequest = this.currentdomaindb.InventoryRequestRepository();
                Repositories.User.IInventoryRequestStatusRepository inventoryrequeststatus = this.currentdomaindb.InventoryRequestStatusRepository();

                object returnlist = null;
                if (status.Equals("Open"))
                {
                    returnlist = (from inv in inventoryrequest.GetAll()
                                  join invst in inventoryrequeststatus.GetAll() on inv.StatusID equals invst.InventoryRequestStatusID
                                  select new
                                  {
                                      inv.InventoryRequestID,
                                      inv.ItemName,
                                      inv.Description,
                                      inv.Quantity,
                                      inv.StatusID,
                                      StatusCustom = invst.StatusName
                                  }).Where(x => (x.StatusCustom == "Open" || x.StatusCustom == "Progress")).OrderByDescending(x => x.InventoryRequestID).ToList();
                }
                else
                {

                    returnlist = (from inv in inventoryrequest.GetAll()
                                  join invst in inventoryrequeststatus.GetAll() on inv.StatusID equals invst.InventoryRequestStatusID
                                  select new
                                  {
                                      inv.InventoryRequestID,
                                      inv.ItemName,
                                      inv.Description,
                                      inv.Quantity,
                                      inv.StatusID,
                                      StatusCustom = invst.StatusName
                                  }).Where(x => (x.StatusCustom == "Rejected" || x.StatusCustom == "Completed")).OrderByDescending(x => x.InventoryRequestID).ToList();
                }




                return WebJSResponse.ResponseSimple(new { inventoryrequestjson = returnlist });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [HttpPost]
        [Route("GetDetail")]
        public JsonResult GetDetail(int inventoryrequestid)
        {
            try
            {
                Repositories.User.IInventoryRequestRepository inventoryrequest = this.currentdomaindb.InventoryRequestRepository();
                Repositories.User.IInventoryRequestStatusRepository inventoryrequeststatus = this.currentdomaindb.InventoryRequestStatusRepository();
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();

                object returnlist = null;
                object openAction = null;
                object progressAction = null;
                List<BusinessPOCO.User.Cl_InventoryRequestStatus> inventoryrequeststatuses = null;
                var data = inventoryrequest.GetByID(inventoryrequestid);
                if (data != null)
                {
                    BusinessPOCO.User.Cl_ClientUser cu = clientuser.GetByID(data.RequestCreatedBy.Value);
                    returnlist = new
                    {
                        data.InventoryRequestID,
                        data.ItemName,
                        data.Description,
                        data.Comments,
                        data.Quantity,
                        StatusName = inventoryrequeststatus.GetByID(data.StatusID.Value).StatusName,
                        RequestCreatedDateCustom = data.RequestCreatedDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                        RequestCreatedByName = cu.FirstName+" "+cu.LastName
                    };

                    if (data.OpenActionBy.HasValue)
                    {
                        cu = clientuser.GetByID(data.OpenActionBy.Value);
                        openAction = new 
                        {
                            OpenActionByName = cu.FirstName+" "+cu.LastName,
                            OpenActionDateCustom = data.OpenActionDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                            OpenActionComments = data.OpenActionComments,
                            OpenActionStatusName = inventoryrequeststatus.GetByID(data.OpenActionStatusID.Value).StatusName
                        };
                    }

                    if (data.ProgressActionBy.HasValue)
                    {
                        cu = clientuser.GetByID(data.ProgressActionBy.Value);
                        progressAction = new
                        {
                            ProgressActionByName = cu.FirstName + " " + cu.LastName,
                            ProgressActionDateCustom = data.ProgressActionDate.Value.ToString("MM/dd/yyyy HH:mm tt"),
                            ProgressActionComments = data.ProgressActionComments,
                            ProgressActionStatusName = inventoryrequeststatus.GetByID(data.StatusID.Value).StatusName
                        };
                    }

                    inventoryrequeststatuses = inventoryrequeststatus.GetAll().Where(x => !x.StatusName.Equals("Open") && !x.StatusName.Equals("Open-Deleted") && !x.StatusName.Equals("Progress-Deleted")).ToList();
                    if (openAction != null)
                    {
                        inventoryrequeststatuses.Remove(inventoryrequeststatuses.Where(x => x.StatusName.Equals("Progress")).FirstOrDefault());
                    }
                }

                return WebJSResponse.ResponseSimple(new { inventoryrequestobjjson = returnlist, openactionjson = openAction, progressactionjson = progressAction, inventoryrequeststatusesjson = inventoryrequeststatuses });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("UpdateStatus")]
        [HttpPost]
        public JsonResult UpdateStatus(int inventoryrequestid, string comments, int statusid)
        {
            try
            {
                Repositories.User.IInventoryRequestRepository inventoryrequest = this.currentdomaindb.InventoryRequestRepository();
                Repositories.User.IInventoryRequestStatusRepository inventoryrequeststatus = this.currentdomaindb.InventoryRequestStatusRepository();

                var inventoryrequestobj = inventoryrequest.GetByID(inventoryrequestid);

                string statusname = string.Empty;
                if (statusid != -100)
                {
                    statusname = inventoryrequeststatus.GetByID(statusid).StatusName;
                }
                else
                {
                    statusname = "Deleted";
                    BusinessPOCO.User.Cl_InventoryRequestStatus reqstatus = null;
                    if (inventoryrequestobj.OpenActionBy.HasValue)
                    {
                        reqstatus = inventoryrequeststatus.GetAll().Where(x => x.StatusName.Equals("Progress-Deleted")).FirstOrDefault();
                        statusid = reqstatus.InventoryRequestStatusID;
                    }
                    else
                    {
                        reqstatus = inventoryrequeststatus.GetAll().Where(x => x.StatusName.Equals("Open-Deleted")).FirstOrDefault();
                        statusid = reqstatus.InventoryRequestStatusID;
                    }
                }


                inventoryrequestobj.StatusID = statusid;
                if (inventoryrequestobj.OpenActionBy.HasValue)
                {

                    inventoryrequestobj.ProgressActionBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                    inventoryrequestobj.ProgressActionComments = comments;
                    inventoryrequestobj.ProgressActionDate = DateTime.Now;
                }

                else
                {
                    inventoryrequestobj.OpenActionBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                    inventoryrequestobj.OpenActionComments = comments;
                    inventoryrequestobj.OpenActionDate = DateTime.Now;
                    inventoryrequestobj.OpenActionStatusID = statusid;
                }

                inventoryrequest.Update(inventoryrequestobj);

                inventoryrequest.Save();

                if (statusname.Contains("Deleted"))
                {
                    return WebJSResponse.ResponseSWAL(SwalEnum.success, "Deleted Successfully !", "Request has been deleted successfully<br>", new { inventoryrequestjson = inventoryrequestobj });
                }
                else
                {
                    return WebJSResponse.ResponseSWAL(SwalEnum.success, "Updated Successfully !", "Status has been updated successfully<br>", new { inventoryrequestjson = inventoryrequestobj });
                }

            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [Route("ClosedRequests")]
        [ClientAuthorizeMember]
        // [SystemAuthorizeMember]
        [HttpGet]
        public ActionResult ClosedRequests()
        {
            return View("~/Views/User/InventoryRequest/ClosedRequests.cshtml");
        }
        [Route("OpenRequests")]
        [ClientAuthorizeMember]
        //[SystemAuthorizeMember]
        [HttpGet]
        public ActionResult OpenRequests()
        {
            return View("~/Views/User/InventoryRequest/OpenRequests.cshtml");
        }
        [Route("RaiseRequest")]
        [ClientAuthorizeMember]
        //[SystemAuthorizeMember]
        [HttpGet]
        public ActionResult RaiseRequest()
        {
            return View("~/Views/User/InventoryRequest/RaiseRequest.cshtml");
        }

    }
}
