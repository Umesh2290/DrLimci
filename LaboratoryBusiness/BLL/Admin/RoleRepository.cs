using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.Admin
{
    public class RoleRepository : LaboratoryBusiness.Repositories.Admin.IRoleRepository
    {
        private readonly LabSystemDBEntities _context;

        public RoleRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public RoleRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.Role> GetAll()
        {

            var records = (from p in _context.Tbl_Role.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.Role
                           {
                               RoleID = p.RoleID,
                               RoleName = p.RoleName,
                               IsActive = p.IsActive,
                               Description = p.Description,
                             
                               CreatedBy=p.CreatedBy,
                               CreatedDate=p.CreatedDate,
                               UpdatedBy=p.UpdatedBy,
                               UpdatedDate=p.UpdatedDate
                           });
            return records;
            
        }

        public LaboratoryBusiness.POCO.Admin.Role GetByID(int RoleID)
        {
            var record = (from p in _context.Tbl_Role.AsEnumerable()
                          where p.RoleID==RoleID
                          select new LaboratoryBusiness.POCO.Admin.Role
                          {
                              RoleID = p.RoleID,
                              RoleName = p.RoleName,
                              IsActive = p.IsActive,
                              Description = p.Description,

                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate
                          }).FirstOrDefault();

            return record;
           
        }

        public void Insert(LaboratoryBusiness.POCO.Admin.Role role)
        {
                _context.Tbl_Role.Add(new Tbl_Role()
                {
                   // RoleID = p.RoleID,
                    RoleName = role.RoleName,
                    IsActive = role.IsActive,
                    Description = role.Description,

                    CreatedBy = role.CreatedBy,
                    CreatedDate = role.CreatedDate,
                    UpdatedBy = role.UpdatedBy,
                    UpdatedDate = role.UpdatedDate

                });
        }

        public void Update(LaboratoryBusiness.POCO.Admin.Role role)
        {
            var record = _context.Tbl_Role.Where(x => x.RoleID == role.RoleID).SingleOrDefault();
                if (record != null)
                {
                record.RoleID = role.RoleID;
                record.RoleName = role.RoleName;
                record.IsActive = role.IsActive;
                record.Description = role.Description;

                record.CreatedBy = role.CreatedBy;
                record.CreatedDate = role.CreatedDate;
                record.UpdatedBy = role.UpdatedBy;
                record.UpdatedDate = role.UpdatedDate;
                }
                else
                {
                    throw new Exception("Record not found");
                }
        }

        public void Delete(int RoleID)
        {
            var record = _context.Tbl_Role.Where(x => x.RoleID == RoleID).SingleOrDefault();
            _context.Tbl_Role.Remove(record);
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
