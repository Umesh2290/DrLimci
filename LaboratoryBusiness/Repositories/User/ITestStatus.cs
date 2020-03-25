using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface ITestStatusRepositories
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_TestStatus> GetAll();
        LaboratoryBusiness.POCO.User.Cl_TestStatus GetByID(int TestStatusID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_TestStatus TestInvestigation);
        void Update(LaboratoryBusiness.POCO.User.Cl_TestStatus TestInvestigation);
        void Delete(int TestStatusID);
        void Save();
    }
}
