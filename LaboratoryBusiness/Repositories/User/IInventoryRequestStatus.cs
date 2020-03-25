using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface IInventoryRequestStatusRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_InventoryRequestStatus> GetAll();
        LaboratoryBusiness.POCO.User.Cl_InventoryRequestStatus GetByID(int ClientUserTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_InventoryRequestStatus clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Cl_InventoryRequestStatus clientusertype);
        void Delete(int ClientUserTypeID);
        void Save();
    }

}
