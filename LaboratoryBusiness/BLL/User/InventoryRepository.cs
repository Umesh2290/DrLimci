using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class InventoryRepository : LaboratoryBusiness.Repositories.User.IInventory
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_Inventory Inventory_entity = new Tbl_Cl_Inventory();
        private LaboratoryBusiness.POCO.User.Cl_Inventory Inventory_poco = new POCO.User.Cl_Inventory();

        public InventoryRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public InventoryRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_Inventory> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_Inventory.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_Inventory
                           {
                              Code=p.Code,
                              CreatedBy=p.CreatedBy,
                              CreatedDate=p.CreatedDate,
                              Description=p.Description,
                              ExpiryDate=p.ExpiryDate,
                              InventoryID=p.InventoryID,
                              InventoryStatusID=p.InventoryStatusID,
                              ItemName=p.ItemName,
                              OrderedHistory=p.OrderedHistory,
                              QuantityLeft=p.QuantityLeft,
                              QuantityOrdered=p.QuantityOrdered,
                              UpdatedBy=p.UpdatedBy,
                              UpdatedDate=p.UpdatedDate
                            


                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_Inventory GetByID(int InventoryID)
        {
            var record = (from p in _context.Tbl_Cl_Inventory.AsEnumerable()
                          where p.InventoryID == InventoryID
                          select new LaboratoryBusiness.POCO.User.Cl_Inventory
                          {

                              Code = p.Code,
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              Description = p.Description,
                              ExpiryDate = p.ExpiryDate,
                              InventoryID = p.InventoryID,
                              InventoryStatusID = p.InventoryStatusID,
                              ItemName = p.ItemName,
                              OrderedHistory = p.OrderedHistory,
                              QuantityLeft = p.QuantityLeft,
                              QuantityOrdered = p.QuantityOrdered,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate
                            

                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_Inventory p)
        {
            Tbl_Cl_Inventory inp = new Tbl_Cl_Inventory()
            {

                Code = p.Code,
                CreatedBy = p.CreatedBy,
                CreatedDate = p.CreatedDate,
                Description = p.Description,
                ExpiryDate = p.ExpiryDate,
                InventoryID = p.InventoryID,
                InventoryStatusID = p.InventoryStatusID,
                ItemName = p.ItemName,
                OrderedHistory = p.OrderedHistory,
                QuantityLeft = p.QuantityLeft,
                QuantityOrdered = p.QuantityOrdered,
                UpdatedBy = p.UpdatedBy,
                UpdatedDate = p.UpdatedDate
                            
            };
            _context.Tbl_Cl_Inventory.Add(inp);

            Inventory_entity = inp;
            Inventory_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_Inventory p)
        {
            var record = _context.Tbl_Cl_Inventory.Where(x => x.InventoryID == p.InventoryID).SingleOrDefault();
            if (record != null)
            {
                 record.Code = p.Code;
                record.CreatedBy = p.CreatedBy;
                record.CreatedDate = p.CreatedDate;
                record.Description = p.Description;
                record.ExpiryDate = p.ExpiryDate;
                record.InventoryID = p.InventoryID;
                record.InventoryStatusID = p.InventoryStatusID;
                record.ItemName = p.ItemName;
               record.OrderedHistory = p.OrderedHistory;
                record.QuantityLeft = p.QuantityLeft;
                record.QuantityOrdered = p.QuantityOrdered;
                record.UpdatedBy = p.UpdatedBy;
                record.UpdatedDate = p.UpdatedDate;
            
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int InventoryID)
        {
            var record = _context.Tbl_Cl_Inventory.Where(x => x.InventoryID == InventoryID).SingleOrDefault();
            _context.Tbl_Cl_Inventory.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            Inventory_poco.InventoryID = Inventory_entity.InventoryID;
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
