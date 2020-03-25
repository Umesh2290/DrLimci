using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
 
    public interface IInventory
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_Inventory> GetAll();
        LaboratoryBusiness.POCO.User.Cl_Inventory GetByID(int ClientUserTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_Inventory clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Cl_Inventory clientusertype);
        void Delete(int ClientUserTypeID);
        void Save();
    }
}
