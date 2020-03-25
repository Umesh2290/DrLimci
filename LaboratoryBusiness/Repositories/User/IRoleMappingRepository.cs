using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface IRoleMappingRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_RoleMapping> GetAll();
        LaboratoryBusiness.POCO.User.Cl_RoleMapping GetByID(int RoleMappingID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_RoleMapping rolemapping);
        void Update(LaboratoryBusiness.POCO.User.Cl_RoleMapping rolemapping);
        void Delete(int RoleMappingID);
        void Save();
    }
}
