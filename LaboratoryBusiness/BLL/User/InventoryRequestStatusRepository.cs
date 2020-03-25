using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class InventoryRequestStatusRepository : LaboratoryBusiness.Repositories.User.IInventoryRequestStatusRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_InventoryRequestStatus InventoryRequestStatus_entity = new Tbl_Cl_InventoryRequestStatus();
        private LaboratoryBusiness.POCO.User.Cl_InventoryRequestStatus InventoryRequestStatus_poco = new POCO.User.Cl_InventoryRequestStatus();

        public InventoryRequestStatusRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public InventoryRequestStatusRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_InventoryRequestStatus> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_InventoryRequestStatus.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_InventoryRequestStatus
                           {
                             
                               Description=p.Description,
                               InventoryRequestStatusID=p.InventoryRequestStatusID,
                               StatusName=p.StatusName


                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_InventoryRequestStatus GetByID(int InventoryRequestStatusID)
        {
            var record = (from p in _context.Tbl_Cl_InventoryRequestStatus.AsEnumerable()
                          where p.InventoryRequestStatusID == InventoryRequestStatusID
                          select new LaboratoryBusiness.POCO.User.Cl_InventoryRequestStatus
                          {

                              Description = p.Description,
                              InventoryRequestStatusID = p.InventoryRequestStatusID,
                              StatusName = p.StatusName



                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_InventoryRequestStatus p)
        {
            Tbl_Cl_InventoryRequestStatus inp = new Tbl_Cl_InventoryRequestStatus()
            {
                Description = p.Description,
                InventoryRequestStatusID = p.InventoryRequestStatusID,
                StatusName = p.StatusName

            };
            _context.Tbl_Cl_InventoryRequestStatus.Add(inp);

            InventoryRequestStatus_entity = inp;
            InventoryRequestStatus_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_InventoryRequestStatus p)
        {
            var record = _context.Tbl_Cl_InventoryRequestStatus.Where(x => x.InventoryRequestStatusID == p.InventoryRequestStatusID).SingleOrDefault();
            if (record != null)
            {
                 record.Description = p.Description;
                 record.InventoryRequestStatusID = p.InventoryRequestStatusID;
                record.StatusName = p.StatusName;
            
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int InventoryRequestStatusID)
        {
            var record = _context.Tbl_Cl_InventoryRequestStatus.Where(x => x.InventoryRequestStatusID == InventoryRequestStatusID).SingleOrDefault();
            _context.Tbl_Cl_InventoryRequestStatus.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            InventoryRequestStatus_poco.InventoryRequestStatusID = InventoryRequestStatus_entity.InventoryRequestStatusID;
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
