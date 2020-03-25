using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface IClientUserTypeRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_ClientUserType> GetAll();
        LaboratoryBusiness.POCO.User.Cl_ClientUserType GetByID(int  ClientUserTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_ClientUserType clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Cl_ClientUserType clientusertype);
        void Delete(int ClientUserTypeID);
        void Save();
    }
}
