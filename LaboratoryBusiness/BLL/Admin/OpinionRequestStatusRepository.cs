using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.Admin
{
    public class OpinionRequestStatusRepository : LaboratoryBusiness.Repositories.Admin.IOpinionRequestStatusRepository
    {
        private readonly LabSystemDBEntities _context;
        private Tbl_OpinionRequestStatus _opinionrequeststatus_entity = new Tbl_OpinionRequestStatus();
        private LaboratoryBusiness.POCO.Admin.OpinionRequestStatus _opinionrequeststatus_poco = new POCO.Admin.OpinionRequestStatus();

        public OpinionRequestStatusRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public OpinionRequestStatusRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.OpinionRequestStatus> GetAll()
        {
            var records = (from p in _context.Tbl_OpinionRequestStatus.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.OpinionRequestStatus
                           {
                              OpinionRequestStatusID=p.OpinionRequestStatusID,
                              Description=p.Description,
                              Name=p.Name
                              
                              

                           });
            return records;

        }

        public LaboratoryBusiness.POCO.Admin.OpinionRequestStatus GetByID(int OpinionRequestStatusID)
        {
            var record = (from p in _context.Tbl_OpinionRequestStatus.AsEnumerable()
                          where p.OpinionRequestStatusID == OpinionRequestStatusID
                          select new LaboratoryBusiness.POCO.Admin.OpinionRequestStatus
                          {
                              OpinionRequestStatusID = p.OpinionRequestStatusID,
                              Description = p.Description,
                              Name = p.Name
                          }).FirstOrDefault();

            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.Admin.OpinionRequestStatus opinionrequeststatus)
        {
            Tbl_OpinionRequestStatus ors = new Tbl_OpinionRequestStatus()
            {
                // SalesAndContactQueryStatusID = SalesAndContactQueryStatusID.SalesAndContactQueryStatusID,
                Description = opinionrequeststatus.Description,
                Name = opinionrequeststatus.Name


            };
            _context.Tbl_OpinionRequestStatus.Add(ors);

            _opinionrequeststatus_entity = ors;
            _opinionrequeststatus_poco = opinionrequeststatus;
        }

        public void Update(LaboratoryBusiness.POCO.Admin.OpinionRequestStatus opinionrequeststatus)
        {
            var record = _context.Tbl_OpinionRequestStatus.Where(x => x.OpinionRequestStatusID == opinionrequeststatus.OpinionRequestStatusID).SingleOrDefault();
            if (record != null)
            {
                record.Description = opinionrequeststatus.Description;
                record.Name = opinionrequeststatus.Name;

            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int OpinionRequestStatusID)
        {
            var record = _context.Tbl_OpinionRequestStatus.Where(x => x.OpinionRequestStatusID == OpinionRequestStatusID).SingleOrDefault();
            _context.Tbl_OpinionRequestStatus.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            _opinionrequeststatus_poco.OpinionRequestStatusID = _opinionrequeststatus_entity.OpinionRequestStatusID;
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
