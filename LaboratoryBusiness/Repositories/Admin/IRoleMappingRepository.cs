using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface IRoleMappingRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.RoleMapping> GetAll();
        LaboratoryBusiness.POCO.Admin.RoleMapping GetByID(int RoleMappingID);
        void Insert(LaboratoryBusiness.POCO.Admin.RoleMapping rolemapping);
        void Update(LaboratoryBusiness.POCO.Admin.RoleMapping rolemapping);
        void Delete(int RoleMappingID);
        void Save();
    }
}
