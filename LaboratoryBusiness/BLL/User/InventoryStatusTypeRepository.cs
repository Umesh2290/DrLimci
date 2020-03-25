using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class InventoryStatusTypeRepository : LaboratoryBusiness.Repositories.User.IInventoryStatusTypeRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_InventoryStatusType InventoryRequestStatus_entity = new Tbl_Cl_InventoryStatusType();
        private LaboratoryBusiness.POCO.User.Cl_InventoryStatusType InventoryRequestStatus_poco = new POCO.User.Cl_InventoryStatusType();

        public InventoryStatusTypeRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public InventoryStatusTypeRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_InventoryStatusType> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_InventoryStatusType.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_InventoryStatusType
                           {

                             CreatedBy=p.CreatedBy,
                             CreatedDate=p.CreatedDate,
                             Description=p.Description,
                             InventoryStatusTypeID=p.InventoryStatusTypeID,
                             StatusName=p.StatusName,
                             UpdatedBy=p.UpdatedBy,
                             UpdatedDate=p.UpdatedDate
                             


                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_InventoryStatusType GetByID(int InventoryStatusTypeID)
        {
            var record = (from p in _context.Tbl_Cl_InventoryStatusType.AsEnumerable()
                          where p.InventoryStatusTypeID == InventoryStatusTypeID
                          select new LaboratoryBusiness.POCO.User.Cl_InventoryStatusType
                          {


                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              Description = p.Description,
                              InventoryStatusTypeID = p.InventoryStatusTypeID,
                              StatusName = p.StatusName,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate


                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_InventoryStatusType p)
        {
            Tbl_Cl_InventoryStatusType inp = new Tbl_Cl_InventoryStatusType()
            {

                CreatedBy = p.CreatedBy,
                CreatedDate = p.CreatedDate,
                Description = p.Description,
                InventoryStatusTypeID = p.InventoryStatusTypeID,
                StatusName = p.StatusName,
                UpdatedBy = p.UpdatedBy,
                UpdatedDate = p.UpdatedDate

            };
            _context.Tbl_Cl_InventoryStatusType.Add(inp);

            InventoryRequestStatus_entity = inp;
            InventoryRequestStatus_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_InventoryStatusType p)
        {
            var record = _context.Tbl_Cl_InventoryStatusType.Where(x => x.InventoryStatusTypeID == p.InventoryStatusTypeID).SingleOrDefault();
            if (record != null)
            {
              
                             record.CreatedBy = p.CreatedBy;
                              record.CreatedDate = p.CreatedDate;
                              record.Description = p.Description;
                              record.InventoryStatusTypeID = p.InventoryStatusTypeID;
                              record.StatusName = p.StatusName;
                              record.UpdatedBy = p.UpdatedBy;
                              record.UpdatedDate = p.UpdatedDate;

            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int InventoryStatusTypeID)
        {
            var record = _context.Tbl_Cl_InventoryStatusType.Where(x => x.InventoryStatusTypeID == InventoryStatusTypeID).SingleOrDefault();
            _context.Tbl_Cl_InventoryStatusType.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            InventoryRequestStatus_poco.InventoryStatusTypeID = InventoryRequestStatus_entity.InventoryStatusTypeID;
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
