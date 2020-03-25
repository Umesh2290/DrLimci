using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LaboratoryBusiness.BLL.Admin
{
    public class SystemUserTypeRepository : LaboratoryBusiness.Repositories.Admin.ISystemUserTypeRepository
    {
        private readonly LabSystemDBEntities _context;

        public SystemUserTypeRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public SystemUserTypeRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.SystemUserType> GetAll()
        {
            var records = (from p in _context.Tbl_SystemUserType.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.SystemUserType
                           {
                              Description=p.Description,
                              SystemUserTypeID=p.SystemUserTypeID,
                              TypeName=p.TypeName
                              

                           });
            return records;

        }

        public LaboratoryBusiness.POCO.Admin.SystemUserType GetByID(int SystemUserTypeID)
        {
            var record = (from p in _context.Tbl_SystemUserType.AsEnumerable()
                          where p.SystemUserTypeID == SystemUserTypeID
                          select new LaboratoryBusiness.POCO.Admin.SystemUserType
                          {
                              Description = p.Description,
                              SystemUserTypeID = p.SystemUserTypeID,
                              TypeName = p.TypeName
                          }).FirstOrDefault();

            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.Admin.SystemUserType p)
        {
            _context.Tbl_SystemUserType.Add(new Tbl_SystemUserType()
            {
                Description = p.Description,
              //  SystemUserTypeID = p.SystemUserTypeID,
                TypeName = p.TypeName


            });
        }

        public void Update(LaboratoryBusiness.POCO.Admin.SystemUserType p)
        {
            var record = _context.Tbl_SystemUserType.Where(x => x.SystemUserTypeID == p.SystemUserTypeID).SingleOrDefault();
            if (record != null)
            {
                record.Description = p.Description;
                record.SystemUserTypeID = p.SystemUserTypeID;
                record.TypeName = p.TypeName;


            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int SystemUserTypeID)
        {
            var record = _context.Tbl_SystemUserType.Where(x => x.SystemUserTypeID == SystemUserTypeID).SingleOrDefault();
            _context.Tbl_SystemUserType.Remove(record);
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
