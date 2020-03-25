using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface INotificationRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.Notification> GetAll();
        LaboratoryBusiness.POCO.Admin.Notification GetByID(int NotificationID);
        void Insert(LaboratoryBusiness.POCO.Admin.Notification notification);
        void Update(LaboratoryBusiness.POCO.Admin.Notification notification);
        void Delete(int NotificationID);
        void Save();
    }
}
