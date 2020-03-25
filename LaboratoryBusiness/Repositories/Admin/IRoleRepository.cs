using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface IRoleRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.Role> GetAll();
        LaboratoryBusiness.POCO.Admin.Role GetByID(int RoleID);
        void Insert(LaboratoryBusiness.POCO.Admin.Role role);
        void Update(LaboratoryBusiness.POCO.Admin.Role role);
        void Delete(int RoleID);
        void Save();
    }
}
