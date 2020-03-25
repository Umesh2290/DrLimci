using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.Admin
{
    public class PlanRepository : LaboratoryBusiness.Repositories.Admin.IPlanRepository
    {
        private readonly LabSystemDBEntities _context;

        public PlanRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public PlanRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.Plan> GetAll()
        {

            var records = (from Plan in _context.Tbl_Plan.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.Plan
                           {
                             CreatedBy=Plan.CreatedBy,
                             PlanID=Plan.PlanID,
                             CreatedDate=Plan.CreatedDate,
                             PlanCost=Plan.PlanCost,
                             PlanDescription=Plan.PlanDescription,
                             PlanDetail=Plan.PlanDetail,
                             PlanName=Plan.PlanName,
                             PlanStatus=Plan.PlanStatus,
                             UpdatedBy=Plan.UpdatedBy,
                              UpdatedDate=Plan.UpdatedDate
                              

                           
                           });
            return records;

        }

        public LaboratoryBusiness.POCO.Admin.Plan GetByID(int PlanID)
        {
            var record = (from Plan in _context.Tbl_Plan.AsEnumerable()
                          where Plan.PlanID == PlanID
                          select new LaboratoryBusiness.POCO.Admin.Plan
                          {
                              CreatedBy = Plan.CreatedBy,
                              PlanID = Plan.PlanID,
                              CreatedDate = Plan.CreatedDate,
                              PlanCost = Plan.PlanCost,
                              PlanDescription = Plan.PlanDescription,
                              PlanDetail = Plan.PlanDetail,
                              PlanName = Plan.PlanName,
                              PlanStatus = Plan.PlanStatus,
                              UpdatedBy = Plan.UpdatedBy,
                              UpdatedDate = Plan.UpdatedDate
                          }).FirstOrDefault();

            return record;


        }

        public void Insert(LaboratoryBusiness.POCO.Admin.Plan Plan)
        {
            _context.Tbl_Plan.Add(new Tbl_Plan()
            {
                CreatedBy = Plan.CreatedBy,
                PlanID = Plan.PlanID,
                CreatedDate = Plan.CreatedDate,
                PlanCost = Plan.PlanCost,
                PlanDescription = Plan.PlanDescription,
                PlanDetail = Plan.PlanDetail,
                PlanName = Plan.PlanName,
                PlanStatus = Plan.PlanStatus,
                UpdatedBy = Plan.UpdatedBy,
                UpdatedDate = Plan.UpdatedDate

            });
        }

        public void Update(LaboratoryBusiness.POCO.Admin.Plan Plan)
        {
            var record = _context.Tbl_Plan.Where(x => x.PlanID == Plan.PlanID).SingleOrDefault();
            if (record != null)
            {
                record.CreatedBy = Plan.CreatedBy;
                record.PlanID = Plan.PlanID;
                record.CreatedDate = Plan.CreatedDate;
                record.PlanCost = Plan.PlanCost;
                record.PlanDescription = Plan.PlanDescription;
                record.PlanDetail = Plan.PlanDetail;
                record.PlanName = Plan.PlanName;
                record.PlanStatus = Plan.PlanStatus;
                record.UpdatedBy = Plan.UpdatedBy;
                record.UpdatedDate = Plan.UpdatedDate;
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int PlanID)
        {
            var record = _context.Tbl_Plan.Where(x => x.PlanID == PlanID).SingleOrDefault();
            _context.Tbl_Plan.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
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
