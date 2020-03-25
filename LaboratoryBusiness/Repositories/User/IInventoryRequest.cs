using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
     public interface IInventoryRequestRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_InventoryRequest> GetAll();
        LaboratoryBusiness.POCO.User.Cl_InventoryRequest GetByID(int ClientUserTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_InventoryRequest clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Cl_InventoryRequest clientusertype);
        void Delete(int ClientUserTypeID);
        void Save();
    }
}

