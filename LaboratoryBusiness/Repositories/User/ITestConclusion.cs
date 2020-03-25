using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface ITestConclusionRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_TestConclusion> GetAll();
        LaboratoryBusiness.POCO.User.Cl_TestConclusion GetByID(int ClientUserTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_TestConclusion clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Cl_TestConclusion clientusertype);
        void Delete(int ClientUserTypeID);
        void Save();
    }
}
