using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class SectionRestrictionRepository : LaboratoryBusiness.Repositories.User.ISectionRestrictionRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;

        public SectionRestrictionRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public SectionRestrictionRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_SectionRestriction> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_SectionRestriction.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_SectionRestriction
                           {
                               SectionID = p.SectionID,
                               RoleID = p.RoleID,
                               MenuID = p.MenuID,
                               EmployeeID = p.EmployeeID,
                               SectionSelector = p.SectionSelector,

                               CreatedBy = p.CreatedBy,
                               CreatedDate = p.CreatedDate,
                               UpdatedBy = p.UpdatedBy,
                               UpdatedDate = p.UpdatedDate
                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_SectionRestriction GetByID(int SectionRestrictionID)
        {
            var record = (from p in _context.Tbl_Cl_SectionRestriction.AsEnumerable()
                          where p.SectionID == SectionRestrictionID
                          select new LaboratoryBusiness.POCO.User.Cl_SectionRestriction
                          {
                              SectionID = p.SectionID,
                              RoleID = p.RoleID,
                              MenuID = p.MenuID,
                              EmployeeID = p.EmployeeID,
                              SectionSelector = p.SectionSelector,

                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate
                          }).FirstOrDefault();

            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_SectionRestriction sectionrestriction)
        {
            _context.Tbl_Cl_SectionRestriction.Add(new Tbl_Cl_SectionRestriction()
            {
                //SectionID = p.SectionID,
                RoleID = sectionrestriction.RoleID,
                MenuID = sectionrestriction.MenuID,
                EmployeeID = sectionrestriction.EmployeeID,
                SectionSelector = sectionrestriction.SectionSelector,

                CreatedBy = sectionrestriction.CreatedBy,
                CreatedDate = sectionrestriction.CreatedDate,
                UpdatedBy = sectionrestriction.UpdatedBy,
                UpdatedDate = sectionrestriction.UpdatedDate

            });
        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_SectionRestriction sectionrestriction)
        {
            var record = _context.Tbl_Cl_SectionRestriction.Where(x => x.SectionID == sectionrestriction.SectionID).SingleOrDefault();
            if (record != null)
            {
                sectionrestriction.RoleID = sectionrestriction.RoleID;
                sectionrestriction.MenuID = sectionrestriction.MenuID;
                sectionrestriction.EmployeeID = sectionrestriction.EmployeeID;
                sectionrestriction.SectionSelector = sectionrestriction.SectionSelector;

                sectionrestriction.CreatedBy = sectionrestriction.CreatedBy;
                sectionrestriction.CreatedDate = sectionrestriction.CreatedDate;
                sectionrestriction.UpdatedBy = sectionrestriction.UpdatedBy;
                sectionrestriction.UpdatedDate = sectionrestriction.UpdatedDate;
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int SectionRestrictionID)
        {
            var record = _context.Tbl_Cl_SectionRestriction.Where(x => x.SectionID == SectionRestrictionID).SingleOrDefault();
            _context.Tbl_Cl_SectionRestriction.Remove(record);
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
