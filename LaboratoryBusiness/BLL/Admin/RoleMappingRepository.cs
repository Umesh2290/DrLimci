using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.Admin
{
    public class RoleMappingRepository : LaboratoryBusiness.Repositories.Admin.IRoleMappingRepository
    {
         private readonly LabSystemDBEntities _context;

        public RoleMappingRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public RoleMappingRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.RoleMapping> GetAll()
        {

            var records = (from p in _context.Tbl_RoleMapping.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.RoleMapping
                           {
                               UserID = p.UserID,
                               RoleID = p.RoleID,
                               IsDefault = p.IsDefault,
                               RoleMappingID = p.RoleMappingID,
                               CreatedBy=p.CreatedBy,
                               CreatedDate=p.CreatedDate,
                               UpdatedBy=p.UpdatedBy,
                               UpdatedDate=p.UpdatedDate
                           });
            return records;
            
        }

        public LaboratoryBusiness.POCO.Admin.RoleMapping GetByID(int RoleMappingID)
        {
            var record = (from p in _context.Tbl_RoleMapping.AsEnumerable()
                          where p.RoleMappingID==RoleMappingID
                          select new LaboratoryBusiness.POCO.Admin.RoleMapping
                          {
                              UserID = p.UserID,
                              RoleID = p.RoleID,
                              IsDefault = p.IsDefault,
                              RoleMappingID = p.RoleMappingID,
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate
                          }).FirstOrDefault();

            return record;
           
        }

        public void Insert(LaboratoryBusiness.POCO.Admin.RoleMapping rolemapping)
        {
            _context.Tbl_RoleMapping.Add(new Tbl_RoleMapping()
                {
                //RoleMappingID = rolemapping.RoleMappingID,
                UserID = rolemapping.UserID,
                RoleID = rolemapping.RoleID,
                IsDefault = rolemapping.IsDefault,
                CreatedBy = rolemapping.CreatedBy,
                CreatedDate = rolemapping.CreatedDate,
                UpdatedBy = rolemapping.UpdatedBy,
                UpdatedDate = rolemapping.UpdatedDate

            });
        }

        public void Update(LaboratoryBusiness.POCO.Admin.RoleMapping rolemapping)
        {
            var record = _context.Tbl_RoleMapping.Where(x => x.RoleMappingID == rolemapping.RoleMappingID).SingleOrDefault();
                if (record != null)
                {

                record.RoleID = rolemapping.RoleID;
                record.IsDefault = rolemapping.IsDefault;
                record.UserID = rolemapping.UserID;
                record.CreatedBy = rolemapping.CreatedBy;
                record.CreatedDate = rolemapping.CreatedDate;
                record.UpdatedBy = rolemapping.UpdatedBy;
                record.UpdatedDate = rolemapping.UpdatedDate;
                }
                else
                {
                    throw new Exception("Record not found");
                }
        }

        public void Delete(int RoleMappingID)
        {
            var record = _context.Tbl_RoleMapping.Where(x => x.RoleMappingID == RoleMappingID).SingleOrDefault();
            _context.Tbl_RoleMapping.Remove(record);
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
