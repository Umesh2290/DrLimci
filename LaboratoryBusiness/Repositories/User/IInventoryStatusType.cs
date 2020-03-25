using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
   
    public interface IInventoryStatusTypeRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_InventoryStatusType> GetAll();
        LaboratoryBusiness.POCO.User.Cl_InventoryStatusType GetByID(int ClientUserTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_InventoryStatusType clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Cl_InventoryStatusType clientusertype);
        void Delete(int ClientUserTypeID);
        void Save();
    }
}
