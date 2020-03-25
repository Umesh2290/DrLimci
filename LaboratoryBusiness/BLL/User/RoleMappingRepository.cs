using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class RoleMappingRepository : LaboratoryBusiness.Repositories.User.IRoleMappingRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_RoleMapping _rolemapping_entity = new Tbl_Cl_RoleMapping();
        private LaboratoryBusiness.POCO.User.Cl_RoleMapping _rolemapping_poco = new POCO.User.Cl_RoleMapping();

        public RoleMappingRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public RoleMappingRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_RoleMapping> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_RoleMapping.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_RoleMapping
                           {
                               UserID = p.UserID,
                               RoleID = p.RoleID,
                               IsDefault = p.IsDefault,
                               RoleMappingID = p.RoleMappingID,
                               CreatedBy = p.CreatedBy,
                               CreatedDate = p.CreatedDate,
                               UpdatedBy = p.UpdatedBy,
                               UpdatedDate = p.UpdatedDate
                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_RoleMapping GetByID(int RoleMappingID)
        {
            var record = (from p in _context.Tbl_Cl_RoleMapping.AsEnumerable()
                          where p.RoleMappingID == RoleMappingID
                          select new LaboratoryBusiness.POCO.User.Cl_RoleMapping
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

        public void Insert(LaboratoryBusiness.POCO.User.Cl_RoleMapping rolemapping)
        {
            Tbl_Cl_RoleMapping rlmapping = new Tbl_Cl_RoleMapping()
            {
                //RoleMappingID = rolemapping.RoleMappingID,
                UserID = rolemapping.UserID,
                RoleID = rolemapping.RoleID,
                IsDefault = rolemapping.IsDefault,
                CreatedBy = rolemapping.CreatedBy,
                CreatedDate = rolemapping.CreatedDate,
                UpdatedBy = rolemapping.UpdatedBy,
                UpdatedDate = rolemapping.UpdatedDate

            };
            _context.Tbl_Cl_RoleMapping.Add(rlmapping);

            _rolemapping_entity = rlmapping;
            _rolemapping_poco = rolemapping;
        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_RoleMapping rolemapping)
        {
            var record = _context.Tbl_Cl_RoleMapping.Where(x => x.RoleMappingID == rolemapping.RoleMappingID).SingleOrDefault();
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
            var record = _context.Tbl_Cl_RoleMapping.Where(x => x.RoleMappingID == RoleMappingID).SingleOrDefault();
            _context.Tbl_Cl_RoleMapping.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            _rolemapping_poco.RoleMappingID = _rolemapping_poco.RoleMappingID;
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
