using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
   public interface ITestRate
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_TestRate> GetAll();
        LaboratoryBusiness.POCO.User.Cl_TestRate GetByID(int ClientUserTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_TestRate clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Cl_TestRate clientusertype);
        void Delete(int ClientUserTypeID);
        void Save();
    }
}
