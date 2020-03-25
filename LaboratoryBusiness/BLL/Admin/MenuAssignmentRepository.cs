using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.Admin
{
    public class MenuAssignmentRepository : LaboratoryBusiness.Repositories.Admin.IMenuAssignmentRepository
    {
        private readonly LabSystemDBEntities _context;

        public MenuAssignmentRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public MenuAssignmentRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.MenuAssignment> GetAll()
        {

            var records = (from p in _context.Tbl_MenuAssignment.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.MenuAssignment
                           {
                               AssignmentID = p.AssignmentID,
                               CreatedBy = p.CreatedBy,
                               CreatedDate = p.CreatedDate,
                               EmployeeID = p.EmployeeID,
                               MenuID = p.MenuID,
                               RoleID = p.RoleID,
                               UpdatedBy = p.UpdatedBy,
                               UpdatedDate = p.UpdatedDate
                           });
            return records;

        }

        public LaboratoryBusiness.POCO.Admin.MenuAssignment GetByID(int MenuAssignmentID)
        {
            var record = (from p in _context.Tbl_MenuAssignment.AsEnumerable()
                          where p.AssignmentID == MenuAssignmentID
                          select new LaboratoryBusiness.POCO.Admin.MenuAssignment
                          {
                              AssignmentID = p.AssignmentID,
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              EmployeeID = p.EmployeeID,
                              MenuID = p.MenuID,
                              RoleID = p.RoleID,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate
                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.Admin.MenuAssignment menuassignment)
        {
            _context.Tbl_MenuAssignment.Add(new Tbl_MenuAssignment()
            {
                // AssignmentID = p.AssignmentID,
                CreatedBy = menuassignment.CreatedBy,
                CreatedDate = menuassignment.CreatedDate,
                EmployeeID = menuassignment.EmployeeID,
                MenuID = menuassignment.MenuID,
                RoleID = menuassignment.RoleID,
                UpdatedBy = menuassignment.UpdatedBy,
                UpdatedDate = menuassignment.UpdatedDate
            });
        }

        public void Update(LaboratoryBusiness.POCO.Admin.MenuAssignment menuassignment)
        {
            var record = _context.Tbl_MenuAssignment.Where(x => x.AssignmentID == menuassignment.AssignmentID).SingleOrDefault();
            if (record != null)
            {
                record.CreatedBy = menuassignment.CreatedBy;
                    record.CreatedDate = menuassignment.CreatedDate;
                    record.EmployeeID = menuassignment.EmployeeID;
                    record.MenuID = menuassignment.MenuID;
                    record.RoleID = menuassignment.RoleID;
                    record.UpdatedBy = menuassignment.UpdatedBy;
                    record.UpdatedDate = menuassignment.UpdatedDate;
                }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int MenuAssignmentID)
        {
            var record = _context.Tbl_MenuAssignment.Where(x => x.AssignmentID == MenuAssignmentID).SingleOrDefault();
            _context.Tbl_MenuAssignment.Remove(record);
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
