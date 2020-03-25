using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LaboratoryBusiness.BLL.Admin
{
    public class SalesAndContactQueryStatusRepository : LaboratoryBusiness.Repositories.Admin.ISalesAndContactQueryStatusRepository
    {
        private readonly LabSystemDBEntities _context;

        public SalesAndContactQueryStatusRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public SalesAndContactQueryStatusRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.SalesAndContactQueryStatus> GetAll()
        {
            var records = (from p in _context.Tbl_SalesAndContactQueryStatus.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.SalesAndContactQueryStatus
                           {
                              SalesAndContactQueryStatusID=p.SalesAndContactQueryStatusID,
                              StatusDescription=p.StatusDescription,
                              StatusName=p.StatusName
                              
                              

                           });
            return records;

        }

        public LaboratoryBusiness.POCO.Admin.SalesAndContactQueryStatus GetByID(int SalesAndContactQueryStatusID)
        {
            var record = (from p in _context.Tbl_SalesAndContactQueryStatus.AsEnumerable()
                          where p.SalesAndContactQueryStatusID == SalesAndContactQueryStatusID
                          select new LaboratoryBusiness.POCO.Admin.SalesAndContactQueryStatus
                          {
                              SalesAndContactQueryStatusID = p.SalesAndContactQueryStatusID,
                              StatusDescription = p.StatusDescription,
                              StatusName = p.StatusName
                          }).FirstOrDefault();

            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.Admin.SalesAndContactQueryStatus SalesAndContactQueryStatusID)
        {
            _context.Tbl_SalesAndContactQueryStatus.Add(new Tbl_SalesAndContactQueryStatus()
            {
                // SalesAndContactQueryStatusID = SalesAndContactQueryStatusID.SalesAndContactQueryStatusID,
                StatusDescription = SalesAndContactQueryStatusID.StatusDescription,
            StatusName = SalesAndContactQueryStatusID.StatusName


            });
        }

        public void Update(LaboratoryBusiness.POCO.Admin.SalesAndContactQueryStatus SalesAndContactQueryStatus)
        {
            var record = _context.Tbl_SalesAndContactQueryStatus.Where(x => x.SalesAndContactQueryStatusID == SalesAndContactQueryStatus.SalesAndContactQueryStatusID).SingleOrDefault();
            if (record != null)
            {
                record.StatusDescription = SalesAndContactQueryStatus.StatusDescription;
            record.StatusName = SalesAndContactQueryStatus.StatusName;

            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int SalesAndContactQueryStatus)
        {
            var record = _context.Tbl_SalesAndContactQueryStatus.Where(x => x.SalesAndContactQueryStatusID == SalesAndContactQueryStatus).SingleOrDefault();
            _context.Tbl_SalesAndContactQueryStatus.Remove(record);
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
