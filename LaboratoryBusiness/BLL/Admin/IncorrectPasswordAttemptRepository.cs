using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.Admin
{
    public class IncorrectPasswordAttemptRepository : LaboratoryBusiness.Repositories.Admin.IIncorrectPasswordAttemptRepository
    {
        private readonly LabSystemDBEntities _context;
        
        public IncorrectPasswordAttemptRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public IncorrectPasswordAttemptRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.IncorrectPasswordAttempt> GetAll()
        {

            var records = (from p in _context.Tbl_IncorrectPasswordAttempt.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.IncorrectPasswordAttempt
                           {
                             IncorrectPasswordID=p.IncorrectPasswordID,
                             CreatedDate=p.CreatedDate,
                             Password=p.Password,
                             SystemUserID=p.SystemUserID,
                             IsInclude = p.IsInclude


                           });
            return records;

        }

        public LaboratoryBusiness.POCO.Admin.IncorrectPasswordAttempt GetByID(int IncorrectPasswordAttemptID)
        {
            var record = (from p in _context.Tbl_IncorrectPasswordAttempt.AsEnumerable()
                          where p.IncorrectPasswordID == IncorrectPasswordAttemptID
                          select new LaboratoryBusiness.POCO.Admin.IncorrectPasswordAttempt
                          {

                              IncorrectPasswordID = p.IncorrectPasswordID,
                              CreatedDate = p.CreatedDate,
                              Password = p.Password,
                              SystemUserID = p.SystemUserID,
                              IsInclude = p.IsInclude

                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.Admin.IncorrectPasswordAttempt IncorrectPasswordAttempt)
        {
            _context.Tbl_IncorrectPasswordAttempt.Add(new Tbl_IncorrectPasswordAttempt()
            {
                // IncorrectPasswordAttemptID = p.IncorrectPasswordAttemptID,

                //   IncorrectPasswordAttemptID = p.IncorrectPasswordAttemptID,

               // IncorrectPasswordID = IncorrectPasswordAttempt.IncorrectPasswordID,
                CreatedDate = IncorrectPasswordAttempt.CreatedDate,
                Password = IncorrectPasswordAttempt.Password,
                SystemUserID = IncorrectPasswordAttempt.SystemUserID,
                IsInclude = IncorrectPasswordAttempt.IsInclude
            });
        }

        public void Update(LaboratoryBusiness.POCO.Admin.IncorrectPasswordAttempt IncorrectPasswordAttempt)
        {
            var record = _context.Tbl_IncorrectPasswordAttempt.Where(x => x.IncorrectPasswordID == IncorrectPasswordAttempt.IncorrectPasswordID).SingleOrDefault();
            if (record != null)
            {
                record.CreatedDate = IncorrectPasswordAttempt.CreatedDate;
                record.Password = IncorrectPasswordAttempt.Password;
                record.SystemUserID = IncorrectPasswordAttempt.SystemUserID;
                record.IsInclude = IncorrectPasswordAttempt.IsInclude;
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int IncorrectPasswordAttemptID)
        {
            var record = _context.Tbl_IncorrectPasswordAttempt.Where(x => x.IncorrectPasswordID == IncorrectPasswordAttemptID).SingleOrDefault();
            _context.Tbl_IncorrectPasswordAttempt.Remove(record);
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
