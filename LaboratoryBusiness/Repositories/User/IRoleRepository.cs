using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface IRoleRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_Role> GetAll();
        LaboratoryBusiness.POCO.User.Cl_Role GetByID(int RoleID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_Role role);
        void Update(LaboratoryBusiness.POCO.User.Cl_Role role);
        void Delete(int RoleID);
        void Save();
    }
}
