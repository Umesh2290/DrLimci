using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface ISystemUserRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.SystemUser> GetAll();
        LaboratoryBusiness.POCO.Admin.SystemUser GetByID(int SystemUserID);
        void Insert(LaboratoryBusiness.POCO.Admin.SystemUser systemuser);
        void Update(LaboratoryBusiness.POCO.Admin.SystemUser systemuser);
        void Delete(int SystemUserID);
        bool Unblock(int SystemUserID,bool isAutoUnblock,bool isclientadmin=false);
        void Save();
    }
}
