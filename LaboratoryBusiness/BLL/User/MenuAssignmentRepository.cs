using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class MenuAssignmentRepository : LaboratoryBusiness.Repositories.User.IMenuAssignmentRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;

        public MenuAssignmentRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public MenuAssignmentRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_MenuAssignment> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_MenuAssignment.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_MenuAssignment
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

        public LaboratoryBusiness.POCO.User.Cl_MenuAssignment GetByID(int MenuAssignmentID)
        {
            var record = (from p in _context.Tbl_Cl_MenuAssignment.AsEnumerable()
                          where p.AssignmentID == MenuAssignmentID
                          select new LaboratoryBusiness.POCO.User.Cl_MenuAssignment
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

        public void Insert(LaboratoryBusiness.POCO.User.Cl_MenuAssignment menuassignment)
        {
            _context.Tbl_Cl_MenuAssignment.Add(new Tbl_Cl_MenuAssignment()
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

        public void Update(LaboratoryBusiness.POCO.User.Cl_MenuAssignment menuassignment)
        {
            var record = _context.Tbl_Cl_MenuAssignment.Where(x => x.AssignmentID == menuassignment.AssignmentID).SingleOrDefault();
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
            var record = _context.Tbl_Cl_MenuAssignment.Where(x => x.AssignmentID == MenuAssignmentID).SingleOrDefault();
            _context.Tbl_Cl_MenuAssignment.Remove(record);
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
