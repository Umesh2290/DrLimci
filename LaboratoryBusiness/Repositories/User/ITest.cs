using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface ITestRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_Test> GetAll();
        LaboratoryBusiness.POCO.User.Cl_Test GetByID(int ClientUserTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_Test clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Cl_Test clientusertype);
        void Delete(int ClientUserTypeID);
        void Save();
    }
}
