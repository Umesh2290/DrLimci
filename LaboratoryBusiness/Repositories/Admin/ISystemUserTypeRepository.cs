using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface ISystemUserTypeRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.SystemUserType> GetAll();
        LaboratoryBusiness.POCO.Admin.SystemUserType GetByID(int SystemUserTypeID);
        void Insert(LaboratoryBusiness.POCO.Admin.SystemUserType systemuser);
        void Update(LaboratoryBusiness.POCO.Admin.SystemUserType systemuser);
        void Delete(int SystemUserTypeID);
        void Save();
    }
}
