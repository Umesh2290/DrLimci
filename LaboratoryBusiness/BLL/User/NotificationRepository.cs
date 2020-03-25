using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class NotificationRepository : LaboratoryBusiness.Repositories.User.INotificationRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_Notification _notification_entity = new Tbl_Cl_Notification();
        private LaboratoryBusiness.POCO.User.Cl_Notification _notification_poco = new POCO.User.Cl_Notification();

        public NotificationRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public NotificationRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_Notification> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_Notification.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_Notification
                           {
                               Title = p.Title,
                               Type = p.Type,
                               ViewedDatetime = p.ViewedDatetime,
                               NotificationID = p.NotificationID,
                               Isviewed = p.Isviewed,
                               Icon = p.Icon,
                               EmployeeID = p.EmployeeID,
                               Description = p.Description,
                               CreatedDatetime = p.CreatedDatetime,
                               CreatedBy = p.CreatedBy,
                               ClickLink = p.ClickLink

                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_Notification GetByID(int NotificationID)
        {
            var record = (from p in _context.Tbl_Cl_Notification.AsEnumerable()
                          where p.NotificationID == NotificationID
                          select new LaboratoryBusiness.POCO.User.Cl_Notification
                          {
                              Title = p.Title,
                              Type = p.Type,
                              ViewedDatetime = p.ViewedDatetime,
                              NotificationID = p.NotificationID,
                              Isviewed = p.Isviewed,
                              Icon = p.Icon,
                              EmployeeID = p.EmployeeID,
                              Description = p.Description,
                              CreatedDatetime = p.CreatedDatetime,
                              CreatedBy = p.CreatedBy,
                              ClickLink = p.ClickLink
                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_Notification notification)
        {
            Tbl_Cl_Notification notif = new Tbl_Cl_Notification()
            {
                NotificationID = notification.NotificationID,
                Title = notification.Title,
                Type = notification.Type,
                ViewedDatetime = notification.ViewedDatetime,
                Isviewed = notification.Isviewed,
                Icon = notification.Icon,
                EmployeeID = notification.EmployeeID,
                Description = notification.Description,
                CreatedDatetime = notification.CreatedDatetime,
                CreatedBy = notification.CreatedBy,
                ClickLink = notification.ClickLink
            };
            _context.Tbl_Cl_Notification.Add(notif);

            _notification_entity = notif;
            _notification_poco = notification;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_Notification notification)
        {
            var record = _context.Tbl_Cl_Notification.Where(x => x.NotificationID == notification.NotificationID).SingleOrDefault();
            if (record != null)
            {
                record.Title = notification.Title;
                record.Type = notification.Type;
                record.ViewedDatetime = notification.ViewedDatetime;
                record.Isviewed = notification.Isviewed;
                record.Icon = notification.Icon;
                record.EmployeeID = notification.EmployeeID;
                record.Description = notification.Description;
                record.CreatedDatetime = notification.CreatedDatetime;
                record.CreatedBy = notification.CreatedBy;
                record.ClickLink = notification.ClickLink;
             }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int NotificationID)
        {
            var record = _context.Tbl_Cl_Notification.Where(x => x.NotificationID == NotificationID).SingleOrDefault();
            _context.Tbl_Cl_Notification.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            _notification_poco.NotificationID = _notification_entity.NotificationID;
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
