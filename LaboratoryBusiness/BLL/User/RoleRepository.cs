using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class RoleRepository : LaboratoryBusiness.Repositories.User.IRoleRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_Role _role_entity = new Tbl_Cl_Role();
        private LaboratoryBusiness.POCO.User.Cl_Role _role_poco = new POCO.User.Cl_Role();

        public RoleRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public RoleRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_Role> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_Role.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_Role
                           {
                               RoleID = p.RoleID,
                               RoleName = p.RoleName,
                               UpdatedBy = p.UpdatedBy,
                               UpdatedDate = p.UpdatedDate,
                               IsActive = p.IsActive,
                               Description = p.Description,
                               CreatedDate = p.CreatedDate,
                               CreatedBy = p.CreatedBy

                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_Role GetByID(int RoleID)
        {
            var record = (from p in _context.Tbl_Cl_Role.AsEnumerable()
                          where p.RoleID == RoleID
                          select new LaboratoryBusiness.POCO.User.Cl_Role
                          {
                              RoleID = p.RoleID,
                              RoleName = p.RoleName,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate,
                              IsActive = p.IsActive,
                              Description = p.Description,
                              CreatedDate = p.CreatedDate,
                              CreatedBy = p.CreatedBy

                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_Role rolepoco)
        {
            Tbl_Cl_Role role = new Tbl_Cl_Role()
            {
                RoleID = rolepoco.RoleID,
                RoleName = rolepoco.RoleName,
                UpdatedBy = rolepoco.UpdatedBy,
                UpdatedDate = rolepoco.UpdatedDate,
                IsActive = rolepoco.IsActive,
                Description = rolepoco.Description,
                CreatedDate = rolepoco.CreatedDate,
                CreatedBy = rolepoco.CreatedBy
            };
            _context.Tbl_Cl_Role.Add(role);

            _role_entity = role;
            _role_poco = rolepoco;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_Role rolepoco)
        {
            var record = _context.Tbl_Cl_Role.Where(x => x.RoleID == rolepoco.RoleID).SingleOrDefault();
            if (record != null)
            {
                record.RoleID = rolepoco.RoleID;
                record.RoleName = rolepoco.RoleName;
                record.UpdatedBy = rolepoco.UpdatedBy;
                record.UpdatedDate = rolepoco.UpdatedDate;
                record.IsActive = rolepoco.IsActive;
                record.Description = rolepoco.Description;
             }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int RoleID)
        {
            var record = _context.Tbl_Cl_Role.Where(x => x.RoleID == RoleID).SingleOrDefault();
            _context.Tbl_Cl_Role.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            _role_poco.RoleID = _role_entity.RoleID;
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
