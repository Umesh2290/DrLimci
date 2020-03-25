using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.Admin
{
    public class MenuRepository : LaboratoryBusiness.Repositories.Admin.IMenuRepository
    {
        private readonly LabSystemDBEntities _context;

        public MenuRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public MenuRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.Menu> GetAll()
        {

            var records = (from p in _context.Tbl_Menu.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.Menu
                           {
                               MenuID = p.MenuID,
                               MenuName = p.MenuName,
                               Description = p.Description,
                               Icon = p.Icon,
                               Link = p.Link,
                               ParentID = p.ParentID,
                               ToolTip = p.ToolTip,
                               IsViewable = p.IsViewable,
                               CreatedBy = p.CreatedBy,
                               CreatedDate = p.CreatedDate,
                               UpdatedBy = p.UpdatedBy,
                               UpdatedDate = p.UpdatedDate
                           });
            return records;

        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.Menu> GetByLink(string link)
        {

            var records = (from p in _context.Tbl_Menu.AsEnumerable()
                           where p.Link == link
                           select new LaboratoryBusiness.POCO.Admin.Menu
                           {
                               MenuID = p.MenuID,
                               MenuName = p.MenuName,
                               Description = p.Description,
                               Icon = p.Icon,
                               Link = p.Link,
                               ParentID = p.ParentID,
                               ToolTip = p.ToolTip,
                               IsViewable = p.IsViewable,
                               CreatedBy = p.CreatedBy,
                               CreatedDate = p.CreatedDate,
                               UpdatedBy = p.UpdatedBy,
                               UpdatedDate = p.UpdatedDate
                           });
            return records;

        }
        public LaboratoryBusiness.POCO.Admin.Menu GetByID(int MenuID)
        {
            var record = (from p in _context.Tbl_Menu.AsEnumerable()
                          where p.MenuID == MenuID
                          select new LaboratoryBusiness.POCO.Admin.Menu
                          {
                              MenuID = p.MenuID,
                              MenuName = p.MenuName,
                              Description = p.Description,
                              Icon = p.Icon,
                              Link = p.Link,
                              ParentID = p.ParentID,
                              ToolTip = p.ToolTip,
                              IsViewable = p.IsViewable,
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate
                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.Admin.Menu menu)
        {
            _context.Tbl_Menu.Add(new Tbl_Menu()
            {
                // MenuID = p.MenuID,
                MenuName = menu.MenuName,
                Description = menu.Description,
                Icon = menu.Icon,
                Link = menu.Link,
                ParentID = menu.ParentID,
                ToolTip = menu.ToolTip,
                IsViewable = menu.IsViewable,
                CreatedBy = menu.CreatedBy,
                CreatedDate = menu.CreatedDate,
                UpdatedBy = menu.UpdatedBy,
                UpdatedDate = menu.UpdatedDate
            });
        }

        public void Update(LaboratoryBusiness.POCO.Admin.Menu menu)
        {
            var record = _context.Tbl_Menu.Where(x => x.MenuID == menu.MenuID).SingleOrDefault();
            if (record != null)
            {
                record.MenuName = menu.MenuName;
                record.Description = menu.Description;
                record.Icon = menu.Icon;
                record.Link = menu.Link;
                record.ParentID = menu.ParentID;
                record.ToolTip = menu.ToolTip;
                record.IsViewable = menu.IsViewable;
                record.CreatedBy = menu.CreatedBy;
                record.CreatedDate = menu.CreatedDate;
                record.UpdatedBy = menu.UpdatedBy;
                record.UpdatedDate = menu.UpdatedDate;
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int MenuID)
        {
            var record = _context.Tbl_Menu.Where(x => x.MenuID == MenuID).SingleOrDefault();
            _context.Tbl_Menu.Remove(record);
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
