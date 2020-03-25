using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface INotificationRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_Notification> GetAll();
        LaboratoryBusiness.POCO.User.Cl_Notification GetByID(int NotificationID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_Notification notification);
        void Update(LaboratoryBusiness.POCO.User.Cl_Notification notification);
        void Delete(int NotificationID);
        void Save();
    }
}
