using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.Admin
{
    public class NotificationRepository : LaboratoryBusiness.Repositories.Admin.INotificationRepository
    {
        private readonly LabSystemDBEntities _context;

        public NotificationRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public NotificationRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.Notification> GetAll()
        {

            var records = (from p in _context.Tbl_Notification.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.Notification
                           {
                               Title = p.Title,
                               Description = p.Description,
                               Icon = p.Icon,
                               Type = p.Type,
                               Isviewed = p.Isviewed,
                               ClickLink = p.ClickLink,
                               CreatedDatetime = p.CreatedDatetime,
                               ViewedDatetime = p.ViewedDatetime,
                               EmployeeID = p.EmployeeID,
                               NotificationID=p.NotificationID,
                               CreatedBy = p.CreatedBy
                               
                               
                           });
            return records;
            
        }

        public LaboratoryBusiness.POCO.Admin.Notification GetByID(int NotificationID)
        {
            var record = (from p in _context.Tbl_Notification.AsEnumerable()
                          where p.NotificationID==NotificationID
                          select new LaboratoryBusiness.POCO.Admin.Notification
                          {
                              Title = p.Title,
                              Description = p.Description,
                              Icon = p.Icon,
                              Type = p.Type,
                              Isviewed = p.Isviewed,
                              ClickLink = p.ClickLink,
                              CreatedDatetime = p.CreatedDatetime,
                              ViewedDatetime = p.ViewedDatetime,
                              EmployeeID = p.EmployeeID,
                              NotificationID = p.NotificationID,
                              CreatedBy = p.CreatedBy
                          }).FirstOrDefault();

            return record;
           
        }

        public void Insert(LaboratoryBusiness.POCO.Admin.Notification notification)
        {
                _context.Tbl_Notification.Add(new Tbl_Notification()
                {
                    Title = notification.Title,
                    Description = notification.Description,
                    Icon = notification.Icon,
                    Type = notification.Type,
                    Isviewed = notification.Isviewed,
                    ClickLink = notification.ClickLink,
                    CreatedDatetime = notification.CreatedDatetime,
                    ViewedDatetime = notification.ViewedDatetime,
                    EmployeeID = notification.EmployeeID,
                    CreatedBy = notification.CreatedBy
                    
                });
        }

        public void Update(LaboratoryBusiness.POCO.Admin.Notification notification)
        {
            var record = _context.Tbl_Notification.Where(x => x.NotificationID == notification.NotificationID).SingleOrDefault();
                if (record != null)
                {
                    record.Title = notification.Title;
                    record.Description = notification.Description;
                    record.Icon = notification.Icon;
                    record.Type = notification.Type;
                    record.Isviewed = notification.Isviewed;
                    record.ClickLink = notification.ClickLink;
                    record.CreatedDatetime = notification.CreatedDatetime;
                    record.ViewedDatetime = notification.ViewedDatetime;
                    record.EmployeeID = notification.EmployeeID;
                    record.CreatedBy = notification.CreatedBy;
                }
                else
                {
                    throw new Exception("Record not found");
                }
        }

        public void Delete(int NotificationID)
        {
            var record = _context.Tbl_Notification.Where(x => x.NotificationID == NotificationID).SingleOrDefault();
            _context.Tbl_Notification.Remove(record);
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
