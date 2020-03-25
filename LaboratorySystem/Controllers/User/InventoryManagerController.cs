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
    [RoutePrefix("User/InventoryManager")]
    public class InventoryManagerController : RedirectController
    {
        // GET: InventoryManager
        [Route]
        [ClientAuthorizeMember]
        public ActionResult Index()
        {
            ViewBag.currentdomaindb = this.currentdomaindb;
            return View("~/Views/User/InventoryManager/Index.cshtml");
        }


        [HttpPost]
        [Route("Create")]
        public JsonResult Create(string itemname, string code, string description, string quantityordered, string quantityleft, string orderedhistory,DateTime expirydate, int status, int editid)
        {
            try
            {
                Repositories.User.IInventory inventory = this.currentdomaindb.InventoryRepository();
                Repositories.User.IInventoryStatusTypeRepository inventorystatustype = this.currentdomaindb.InventoryStatusTypeRepository();


                if (editid == 0)
                {
                    if (inventory.GetAll().Where(x => x.ItemName.ToLower().Trim().Equals(itemname.ToLower().Trim()) ||  x.Code.ToLower().Trim().Equals(code.ToLower().Trim())).Count() > 0)
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "Item Name or Code already exist !", "Please try another !", new { });

                    }

                    BusinessPOCO.User.Cl_Inventory inventoryobj = new BusinessPOCO.User.Cl_Inventory();
                    inventoryobj.ItemName = itemname.Trim();
                    inventoryobj.Code = code.Trim();
                    inventoryobj.Description = description.Trim();
                    inventoryobj.ExpiryDate = expirydate;
                    inventoryobj.InventoryStatusID = status;
                    inventoryobj.OrderedHistory = orderedhistory.Trim();
                    inventoryobj.QuantityLeft = quantityleft.Trim();
                    inventoryobj.QuantityOrdered = quantityordered.Trim();
                    inventoryobj.CreatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                    inventoryobj.CreatedDate = DateTime.Now;

                    inventory.Insert(inventoryobj);

                    inventory.Save();


                    BusinessPOCO.User.Cl_InventoryStatusType requeststatustype = inventorystatustype.GetByID(status);

                    return WebJSResponse.ResponseSWAL(SwalEnum.success, "Created Successfully !", "Inventory has been created successfully<br>", new { inventoryjson = inventoryobj, inventorystatusjson = requeststatustype });

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
        public JsonResult Update(string code, string description, string quantityordered, string quantityleft, string orderedhistory, DateTime expirydate, int status, int editid)
        {
            try
            {
                Repositories.User.IInventory inventory = this.currentdomaindb.InventoryRepository();
                Repositories.User.IInventoryStatusTypeRepository inventorystatustype = this.currentdomaindb.InventoryStatusTypeRepository();

                if (editid > 0)
                {
                    if (inventory.GetAll().Where(x => x.Code.ToLower().Trim().Equals(code.ToLower().Trim()) && x.InventoryID != editid).Count() > 0)
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "Item Name or Code already exist !", "Please try another !", new { });

                    }

                    var inventoryobj = inventory.GetByID(editid);
                    if (inventoryobj != null)
                    {
                        inventoryobj.Code = code.Trim();
                        inventoryobj.Description = description.Trim();
                        inventoryobj.ExpiryDate = expirydate;
                        inventoryobj.InventoryStatusID = status;
                        inventoryobj.OrderedHistory = orderedhistory.Trim();
                        inventoryobj.QuantityLeft = quantityleft.Trim();
                        inventoryobj.QuantityOrdered = quantityordered.Trim();
                        inventoryobj.UpdatedBy = MySession.GetClientSession(this.subdomainurl).ClientUserID;
                        inventoryobj.UpdatedDate = DateTime.Now;

                        inventory.Update(inventoryobj);

                        inventory.Save();

                        BusinessPOCO.User.Cl_InventoryStatusType requeststatustype = inventorystatustype.GetByID(status);

                        return WebJSResponse.ResponseSWAL(SwalEnum.success, "Updated Successfully !", "Inventory has been updated successfully<br>", new { inventoryjson = inventoryobj, inventorystatusjson = requeststatustype });
                    }

                    else
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "No inventory found !", "There is no inventory associated to this inventory id.", new { });
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
        public JsonResult Get(int editid)
        {
            try
            {
                Repositories.User.IInventory inventory = this.currentdomaindb.InventoryRepository();

                if (editid > 0)
                {

                    var inventoryobj = inventory.GetByID(editid);
                    if (inventoryobj != null)
                    {
                        return WebJSResponse.ResponseSimple(new { inventoryjson = inventoryobj, customjson = new {ExpiryDateCustom=inventoryobj.ExpiryDate.Value.ToString("yyyy-MM-dd")} });
                    }

                    else
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "No inventory found !", "There is no inventory associated to this inventory id.", new { });
                    }


                }

                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Incorrect inventory-id !", "Kindly provide correct inventory id.", new { });


            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [HttpPost]
        [Route("GetList")]
        public JsonResult GetList(int status)
        {
            try
            {
                Repositories.User.IInventory inventory = this.currentdomaindb.InventoryRepository();
                Repositories.User.IInventoryStatusTypeRepository inventorystatustype = this.currentdomaindb.InventoryStatusTypeRepository();

                object returnlist = null;
                returnlist = (from inv in inventory.GetAll()
                              join invst in inventorystatustype.GetAll() on inv.InventoryStatusID equals invst.InventoryStatusTypeID
                              select new
                              {
                                  inv.ItemName,
                                  inv.OrderedHistory,
                                  inv.QuantityLeft,
                                  inv.QuantityOrdered,
                                  inv.UpdatedBy,
                                  inv.UpdatedDate,
                                  inv.InventoryStatusID,
                                  inv.InventoryID,
                                  ExpiryDate = inv.ExpiryDate.Value.ToString("MM/dd/yyyy"),
                                  InventoryStatusCustom = invst.StatusName,
                                  inv.Description,
                                  inv.CreatedDate,
                                  inv.CreatedBy,
                                  inv.Code
                              }).Where(x => (x.InventoryStatusID==status)).OrderByDescending(x => x.InventoryID).ToList();



                return WebJSResponse.ResponseSimple(new { inventoryjson = returnlist });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [HttpPost]
        [Route("GetDetail")]
        public JsonResult GetDetail(int inventoryid)
        {
            try
            {
                Repositories.User.IInventory inventory = this.currentdomaindb.InventoryRepository();
                Repositories.User.IInventoryStatusTypeRepository inventorystatustype = this.currentdomaindb.InventoryStatusTypeRepository();

                object returnlist = null;
                var data = inventory.GetByID(inventoryid);
                if (data != null)
                {
                        returnlist = new
                        {
                            data.InventoryID,
                            data.ItemName,
                            data.OrderedHistory,
                            data.QuantityLeft,
                            data.QuantityOrdered,
                            data.UpdatedBy,
                            data.UpdatedDate,
                            data.ExpiryDate,
                            data.Description,
                            data.CreatedDate,
                            data.CreatedBy,
                            data.Code,
                            StatusName = inventorystatustype.GetByID(data.InventoryStatusID.Value).StatusName,
                            ExpiryDateCustom = data.ExpiryDate.Value.ToString("MM/dd/yyyy")
                        };
                }

                return WebJSResponse.ResponseSimple(new { inventoryjson = returnlist });




            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }


        [Route("AvailableItems")]
        [ClientAuthorizeMember]
        //[SystemAuthorizeMember]
        [HttpGet]
        public ActionResult AvailableItems()
        {
            return View("~/Views/User/InventoryManager/AvailableItems.cshtml");
        }
        [Route("DiscontinuedItems")]
        [ClientAuthorizeMember]
        //[SystemAuthorizeMember]
        [HttpGet]
        public ActionResult DiscontinuedItems()
        {
            return View("~/Views/User/InventoryManager/DiscontinuedItems.cshtml");
        }
        [Route("OutofstockItems")]
        [ClientAuthorizeMember]
        // [SystemAuthorizeMember]
        [HttpGet]
        public ActionResult OutofstockItems()
        {
            return View("~/Views/User/InventoryManager/OutofstockItems.cshtml");
        }
        

    }
}
