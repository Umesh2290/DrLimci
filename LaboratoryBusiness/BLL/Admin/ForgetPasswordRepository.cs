using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.Admin
{
    public class ForgetPasswordRepository : LaboratoryBusiness.Repositories.Admin.IForgetPasswordRepository
    {
        private readonly LabSystemDBEntities _context;

        public ForgetPasswordRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public ForgetPasswordRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.ForgetPassword> GetAll()
        {

            var records = (from p in _context.Tbl_ForgetPassword.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.ForgetPassword
                           {
                             ForgetPasswordID=p.ForgetPasswordID,
                             CreatedDate=p.CreatedDate,
                             ExpiresInMinutes=p.ExpiresInMinutes,
                             SystemUserID=p.SystemUserID,
                             UniqueCode=p.UniqueCode


                           });
            return records;

        }

        public LaboratoryBusiness.POCO.Admin.ForgetPassword GetByID(int ForgetPasswordID)
        {
            var record = (from p in _context.Tbl_ForgetPassword.AsEnumerable()
                          where p.ForgetPasswordID == ForgetPasswordID
                          select new LaboratoryBusiness.POCO.Admin.ForgetPassword
                          {

                              ForgetPasswordID = p.ForgetPasswordID,
                              CreatedDate = p.CreatedDate,
                              ExpiresInMinutes = p.ExpiresInMinutes,
                              SystemUserID = p.SystemUserID,
                              UniqueCode = p.UniqueCode
                          }).FirstOrDefault();
            return record;

        }
        public LaboratoryBusiness.POCO.Admin.ForgetPassword GetByGUID(string guid)
        {
            var record = (from p in _context.Tbl_ForgetPassword.AsEnumerable()
                          where p.UniqueCode == guid
                          select new LaboratoryBusiness.POCO.Admin.ForgetPassword
                          {

                              ForgetPasswordID = p.ForgetPasswordID,
                              CreatedDate = p.CreatedDate,
                              ExpiresInMinutes = p.ExpiresInMinutes,
                              SystemUserID = p.SystemUserID,
                              UniqueCode = p.UniqueCode
                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.Admin.ForgetPassword ForgetPassword)
        {
            _context.Tbl_ForgetPassword.Add(new Tbl_ForgetPassword()
            {
                // ForgetPasswordID = p.ForgetPasswordID,

             //   ForgetPasswordID = p.ForgetPasswordID,
                CreatedDate = ForgetPassword.CreatedDate,
                ExpiresInMinutes = ForgetPassword.ExpiresInMinutes,
                SystemUserID = ForgetPassword.SystemUserID,
                UniqueCode = ForgetPassword.UniqueCode
            });
            Save();
        }

        public void Update(LaboratoryBusiness.POCO.Admin.ForgetPassword ForgetPassword)
        {
            var record = _context.Tbl_ForgetPassword.Where(x => x.ForgetPasswordID == ForgetPassword.ForgetPasswordID).SingleOrDefault();
            if (record != null)
            {
                record.CreatedDate = ForgetPassword.CreatedDate;
                record.ExpiresInMinutes = ForgetPassword.ExpiresInMinutes;
                record.SystemUserID = ForgetPassword.SystemUserID;
                record.UniqueCode = ForgetPassword.UniqueCode;
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int ForgetPasswordID)
        {
            var record = _context.Tbl_ForgetPassword.Where(x => x.ForgetPasswordID == ForgetPasswordID).SingleOrDefault();
            _context.Tbl_ForgetPassword.Remove(record);
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
